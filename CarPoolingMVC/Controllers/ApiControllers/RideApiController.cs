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

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RideApiController : Controller
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
        [Route("CreateRide")]
        public void CreateRide([FromBody]RideVM ride, [FromQuery]string userId)
        {
            int[] res = new int[2];
            Ride newRide;
            newRide = _mapper.Map<Ride>(ride);
            
            string From, To;
            if (ride.Route.ViaPoints != null)
            {
                for (int i = 0; i < ride.Route.ViaPoints.Count; i++)
                {
                    ViaPoint viaPoint = new ViaPoint();
                    if (i == 0)
                    {
                        From = ride.Route.Source;
                    }
                    else
                    {
                        From = ride.Route.ViaPoints[i - 1].Location;
                        viaPoint.Distance = newRide.Route.ViaPoints[i - 1].Distance;
                        viaPoint.Duration = newRide.Route.ViaPoints[i - 1].Duration;
                    }
                    To = ride.Route.ViaPoints[i].Location;
                    res = GetDetails(From, To);
                    viaPoint.Distance += res[0]/1000;
                    viaPoint.Duration += TimeSpan.FromSeconds(res[1]);
                    viaPoint.Location = ride.Route.ViaPoints[i].Location;
                    newRide.Route.ViaPoints.Add(viaPoint);
                }
                From = ride.Route.ViaPoints.Last().Location;
                newRide.Distance = newRide.Route.ViaPoints.Last().Distance;
//                newRide.EndDateTime +=  newRide.Route.ViaPoints.Last().Duration;
            }
            else
            {
                From = ride.Route.Source;
            }
            res = GetDetails(From, ride.Route.Destination);
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
        [Route("BookRide")]
        public IEnumerable<AvailableRideVM> BookRide([FromBody]RequestVM request, [FromQuery]string userId)//
        {
            
            RideRequest request1 = _mapper.Map<RideRequest>(request);
            request1.RiderId = userId;
            List<Ride> rides = _rideService.FindRides(request1);
            List<AvailableRideVM> availableRides = _mapper.Map<List<AvailableRideVM>>(rides);
            availableRides.ForEach(_ => _.Cost = _rideService.CalculateCostForRide(_.Id, request.Source, request.Destination));
            return availableRides;
        }

        [HttpPost]
        [Route("RequestRide")]
        public void RequestRide([FromBody]RequestVM request1, [FromQuery] int rideId, [FromQuery] string userId)
        {
            RideRequest request = _mapper.Map<RideRequest>(request1);
            request.RideId = rideId;
            request.RiderId = userId;
            _rideService.RequestRide(request);
        }

        [HttpGet]
        [Route("GetBookings")]
        public List<BookingDetailsVM> GetBooking([FromQuery]string userId)
        {   
            List<Booking> bookings = _rideService.FindBookings(userId);
            List<BookingDetailsVM> Bookings = _mapper.Map<List<BookingDetailsVM>>(bookings);
            return Bookings;
        }

        [HttpGet]
        [Route("GetOfferedRides")]
        public List<OfferedRideVM> GetOfferedRides([FromQuery]string userId)
        {
            List<Ride> Rides = _rideService.FindOfferedRides(userId);
            List<OfferedRideVM> OfferedRides = _mapper.Map<List<OfferedRideVM>>(Rides);
            return OfferedRides;
        }

        [HttpGet]
        [Route("GetRequests")]
        public List<RequestDetailsVM> GetRequests([FromQuery]int rideId)
        {
            List<RideRequest> Requests = _rideService.GetRequests(rideId);
            List<RequestDetailsVM> RideRequests = _mapper.Map<List<RequestDetailsVM>>(Requests);
            RideRequests.ForEach(_ => _.Cost=_rideService.CalculateCostForRide(rideId, _.Source, _.Destination));
            return RideRequests;
        }

        [HttpPost]
        [Route("ApproveRequest")]
        public ResponseVM ApproveRequest([FromQuery]int rideId,[FromQuery]string requestId,[FromQuery]bool isApprove,[FromQuery]string providerId)
        {
            ResponseVM response = new ResponseVM
            {
                Result = true
            };
            if (isApprove)
            {
                RideRequest request = _rideService.FindRide(rideId).Requests.Find(_=>_.Id==requestId);
                float amount = _rideService.CalculateCostForRide(rideId, request.PickUp,request.Drop);
                if (_userService.IsBalanceAvailable(amount,request.RiderId))
                {
                    if (_rideService.ApproveRequest(rideId, requestId, isApprove))
                    {
                        _userService.PayBill(providerId, request.RiderId, amount);
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

    }
}
