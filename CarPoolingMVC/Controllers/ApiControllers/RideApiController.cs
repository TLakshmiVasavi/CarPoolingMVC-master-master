using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CarPoolingMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Enums;
using Models.Interfaces;
using Newtonsoft.Json;

namespace CarPoolingMVC.Controllers.ApiControllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RideApiController : Controller
    {
        private readonly IRideService _rideService;
        private readonly IUserService _userService;

        public RideApiController(IRideService rideService, IUserService userService)
        {
            _rideService = rideService;
            _userService = userService;
        }        

        [HttpPost]
        [Route("CreateRide")]
        public void CreateRide([FromBody]RideVM ride, [FromQuery]string userId)
        {
            int[] res = new int[2];
            Ride newRide = new Ride
            {
                Route = new Route()
            };
           // User user = _userService.FindUser(userId);
            newRide.ProviderId = userId;
            //newRide.ProviderName = user.Name;
            newRide.Route.Source = ride.Route.Source;
            newRide.Route.Destination = ride.Route.Destination;
            newRide.StartDateTime = ride.StartDateTime;
            newRide.NoOfOfferedSeats = ride.NoOfOfferedSeats;
            newRide.UnitDistanceCost = ride.UnitDistanceCost;
            newRide.VehicleId = ride.VehicleId;
            newRide.EndDateTime = newRide.StartDateTime;
            //newRide.VehicleType = _userService.FindVehicle(ride.VehicleId, user.Id).Type;
            //newRide.VehicleType = _userService.FindVehicle(ride.VehicleId).Type;
            newRide.Route.ViaPoints = new List<ViaPoint>();
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
                    //newRide.Distance += viaPoint.Distance;
                    newRide.Route.ViaPoints.Add(viaPoint);
                }
                From = ride.Route.ViaPoints.Last().Location;
                newRide.Distance = newRide.Route.ViaPoints.Last().Distance;
                newRide.EndDateTime +=  newRide.Route.ViaPoints.Last().Duration;
            }
            else
            {
                From = ride.Route.Source;
            }
            res = GetDetails(From, ride.Route.Destination);
            newRide.Distance += res[0]/1000;
            newRide.EndDateTime += TimeSpan.FromSeconds(res[1]);
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
        [Route("SearchRide")]
        public IEnumerable<AvailableRideVM> SearchRide([FromBody]RequestVM request, [FromQuery]string userId)//
        {
            Request request1 = new Request()
            {
                PickUp = request.Source,
                Drop = request.Destination,
                RiderId = userId,
                NoOfSeats = request.NoOfSeats,
                StartDateTime = request.Date,
                VehicleType = Enum.Parse<VehicleType>(request.VehicleType.ToString())
            };
            List<Ride> rides = _rideService.FindRides(request1);
            List<AvailableRideVM> availableRides = new List<AvailableRideVM>();
            foreach (var item in rides)
            {
                AvailableRideVM availableRide = new AvailableRideVM()
                {
                    StartTime = item.StartDateTime,
                    ProviderName = item.ProviderName,
                    ProviderId = item.ProviderId,
                    Id = item.Id
                };
                //var Vehicle = _userService.FindVehicle(item.VehicleId, item.ProviderId);
                var Vehicle = _userService.FindVehicle(item.VehicleId);
                availableRide.Vehicle = new VehicleVM()
                {
                    Model = Vehicle.Model,
                    Number = Vehicle.Number,
                    Capacity = Vehicle.Capacity,
                    Type = Enum.Parse<VehicleTypeVM>(Vehicle.Type.ToString())
                };
                availableRide.Route = new RouteVM()
                {
                    Source = item.Route.Source,
                    Destination = item.Route.Destination,
                    TotalDistance = item.Route.TotalDistance,
                    ViaPoints = new List<ViaPointVM>()
                };
                foreach (var viaPoint in item.Route.ViaPoints)
                {
                    availableRide.Route.ViaPoints.Add(new ViaPointVM() { Location = viaPoint.Location });
                }
                availableRide.Cost = _rideService.CalculateCostForRide(item.Id, request.Source, request.Destination);
                availableRides.Add(availableRide);
            }
            return availableRides;
        }

        [HttpPost]
        [Route("RequestRide")]
        public ResponseVM RequestRide([FromBody]RequestVM request, [FromQuery] int rideId, [FromQuery] string userId)
        {
            ResponseVM response = new ResponseVM();
            _rideService.RequestRide(userId, rideId, request.Source, request.Destination, request.NoOfSeats);
            return response;
        }

        [HttpGet]
        [Route("GetBookings")]
        public List<BookingDetailsVM> GetBooking([FromQuery]string userId)
        {
            List<BookingDetailsVM> Bookings = new List<BookingDetailsVM>();
            List<Booking> bookings = _rideService.FindBookings(userId);
            bookings.ForEach(x => Bookings.Add(new BookingDetailsVM() 
            { 
                Cost = x.Response.Cost,
                Status = x.Response.Status.ToString(),
                Drop = x.Request.Drop,
                PickUp = x.Request.PickUp,
                VehicleType = Enum.Parse<VehicleTypeVM>(x.Request.VehicleType.ToString()),
                NoOfSeats = x.Request.NoOfSeats,

            }));
           // UserDal 
            return Bookings;
        }

        [HttpGet]
        [Route("GetOfferedRides")]
        public List<OfferedRideVM> GetOfferedRides([FromQuery]string userId)
        {
            List<Ride> Rides = _rideService.FindOfferedRides(userId);
            List<OfferedRideVM> OfferedRides = new List<OfferedRideVM>();
            foreach (var item in Rides)
            {
                OfferedRideVM OfferedRide = new OfferedRideVM
                {
                    StartDateTime = item.StartDateTime,
                    Route = new RouteVM()
                    {
                        Source = item.Route.Source,
                        Destination = item.Route.Destination,
                        TotalDistance = item.Route.TotalDistance,
                        ViaPoints = new List<ViaPointVM>(),
                    }
                };
                foreach (var viaPoint in item.Route.ViaPoints)
                {
                    OfferedRide.Route.ViaPoints.Add(new ViaPointVM() { Location = viaPoint.Location });
                }
                OfferedRide.VehicleId = item.VehicleId;
                OfferedRide.VehicleType = Enum.Parse<VehicleTypeVM>(item.VehicleType.ToString());
                OfferedRide.IsRideCompleted = item.IsRideCompleted;
                OfferedRide.NoOfOfferedSeats = item.NoOfOfferedSeats;
                OfferedRide.Id = item.Id;
                OfferedRides.Add(OfferedRide);
            }
            return OfferedRides;
        }

        [HttpGet]
        [Route("GetRequests")]
        public List<RequestDetailsVM> GetRequests([FromQuery]int rideId)
        {
            List<RequestDetailsVM> RideRequests = new List<RequestDetailsVM>();
            List<Request> Requests = _rideService.GetRequests(rideId);
            foreach (var item in Requests)
            {
                RequestDetailsVM RideRequest = new RequestDetailsVM()
                {
                    Source = item.PickUp,
                    Destination = item.Drop,
                    NoOfSeats = item.NoOfSeats,
                    Id = item.RequestId,
                };
                RideRequest.Cost = _rideService.CalculateCostForRide(rideId, item.PickUp, item.Drop);
                RideRequests.Add(RideRequest);
            }
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
                Request request = _rideService.FindRide(rideId).Requests.Find(_=>_.RequestId==requestId);
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
