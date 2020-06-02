using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.Interfaces;
using Newtonsoft.Json;
using AutoMapper;
using CarPoolingMVC.Models;
using Models;

namespace CarPoolingMVC.Controllers.ApiControllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RideApiController : ControllerBase
    {
        private readonly IRideService _rideService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public RideApiController(IRideService rideService, IUserService userService,IMapper mapper)
        {
            _rideService = rideService;
            _userService = userService;
            _mapper = mapper;
        }        

        [HttpPost]
        public void OfferRide([FromBody]OfferRideVM ride, [FromQuery]string userId)
        {
            int[] res = new int[2];
            Ride newRide;
            newRide = _mapper.Map<Ride>(ride);
            
            string From, To;
            if (ride.Route.Stops != null&&ride.Route.Stops.Count!=0)
            {
                for (int i = 0; i < ride.Route.Stops.Count; i++)
                {
                    Stop viaPoint = new Stop();
                    if (i == 0)
                    {
                        From = ride.Route.From;
                    }
                    else
                    {
                        From = ride.Route.Stops[i - 1];
                        viaPoint.Distance = newRide.Route.Stops[i - 1].Distance;
                        viaPoint.Duration = newRide.Route.Stops[i - 1].Duration;
                    }
                    To = ride.Route.Stops[i];
                    res = GetDetails(From, To);
                    viaPoint.Distance += res[0]/1000;
                    viaPoint.Duration += TimeSpan.FromSeconds(res[1]);
                    viaPoint.Location = ride.Route.Stops[i];
                    newRide.Route.Stops.Add(viaPoint);
                }
                From = ride.Route.Stops.Last();
                newRide.Distance = newRide.Route.Stops.Last().Distance;
//                newRide.EndDateTime +=  newRide.Route.ViaPoints.Last().Duration;
            }
            else
            {
                From = ride.Route.From;
            }
            res = GetDetails(From, ride.Route.To);
            newRide.Distance += res[0]/1000;
//            newRide.EndDateTime += TimeSpan.FromSeconds(res[1]);
            newRide.ProviderId = userId;
            _rideService.CreateRide(newRide);
        }

        public int[] GetDetails(string from, string To)
        {
            string url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + from + "&destinations=" + To + "&key=AIzaSyB3mJ4gmoEDfSjFISRXmngMlZarF0s6XcY";
            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            JourneyDetailsVM JsonResult = JsonConvert.DeserializeObject<JourneyDetailsVM>(responseFromServer);
            int[] res = new int[2];
            res[0] = JsonResult.Rows[0].Elements[0].Distance.Value;
            res[1] = JsonResult.Rows[0].Elements[0].Duration.Value;
            return res;
        }

        [HttpPost]
        public IEnumerable<AvailableRideVM> BookRide([FromBody]RequestVM request, [FromQuery]string userId)//
        {   
            RideRequest request1 = _mapper.Map<RideRequest>(request);
            request1.RiderId = userId;
            List<RideDetails> rides = _rideService.FindRides(request1);
            List<AvailableRideVM> availableRides = _mapper.Map<List<AvailableRideVM>>(rides);
            availableRides.ForEach(ride => ride.Cost = _rideService.CalculateCostForRide(ride.Id, request.From, request.To));
            return availableRides;
        }

        [HttpPost]
        public string RequestRide([FromBody]RequestVM request1,[FromQuery]int noOfSeats, [FromQuery] int rideId, [FromQuery] string userId)
        {
            
            RideRequest request = _mapper.Map<RideRequest>(request1);
            request.NoOfSeats = noOfSeats;
            request.RideId = rideId;
            request.RiderId = userId;
            return _rideService.RequestRide(request);
        }

        [HttpGet]
        public List<BookingDetailsVM> GetBookings([FromQuery]string userId)
        {   
            return _mapper.Map<List<BookingDetailsVM>>(_rideService.FindBookings(userId));
        }

        [HttpGet]
        public List<OfferedRideVM> GetOfferedRides([FromQuery]string userId)
        {
            return _mapper.Map<List<OfferedRideVM>>(_rideService.FindOfferedRides(userId));
        }

        [HttpGet]
        public List<RequestDetailsVM> GetRequests([FromQuery]int rideId)
        {
            List<RequestDetails> Requests = _rideService.GetRequests(rideId);
            List<RequestDetailsVM> RideRequests = _mapper.Map<List<RequestDetailsVM>>(Requests);
            RideRequests.ForEach(_ => _.Cost=_rideService.CalculateCostForRide(rideId, _.From, _.To));
            return RideRequests;
        }

        [HttpPost]
        public ResponseVM ApproveRequest([FromQuery]int rideId,[FromQuery]int requestId,[FromQuery]bool isApprove,[FromQuery]string providerId)
        {
            ResponseVM response = new ResponseVM
            {
                Result = false
            };
            if (isApprove)
            {
                RideRequest request = _rideService.GetRequest(requestId);
                float amount = _rideService.CalculateCostForRide(rideId, request.From,request.To);
                if (_userService.IsBalanceAvailable(amount,request.RiderId))
                {
                    if (_rideService.ApproveRequest(rideId, requestId, isApprove))
                    {
                        _userService.PayBill(providerId, request.RiderId, amount,requestId);
                        response.Result = true;
                    }
                    else
                    {
                        response.ErrorMessage = "The Request can't be Accepted ";
                    }
                }
                else
                {
                    response.ErrorMessage = "The Balance is not Sufficient";
                }
            }
            else
            {
                _rideService.ApproveRequest(rideId, requestId, isApprove);
            }
            return response;
        }
        
        [HttpGet]
        public List<OfferedRideVM> GetAllOffers()
        {
            return _mapper.Map<List<OfferedRideVM>>(_rideService.GetAllOffers());
        }

        [HttpGet]
        public List<BookingDetailsVM> GetAllBookings()
        {
            return _mapper.Map<List<BookingDetailsVM>>(_rideService.GetAllBookings());
        }

    }
}
