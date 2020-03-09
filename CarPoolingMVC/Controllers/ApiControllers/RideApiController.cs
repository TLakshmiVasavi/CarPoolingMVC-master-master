using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CarPoolingMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Enums;
using Models.Interfaces;
using Newtonsoft.Json;

namespace CarPoolingMVC.Controllers.ApiControllers
{

    [Route("api/[controller]")]
    [ApiController]
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
        public void CreateRide([FromBody]Ride ride, [FromQuery]string userId)
        {
            Ride newRide = new Ride
            {
                Route = new Route()
            };
            User user = _userService.FindUser(userId);
            newRide.ProviderId = user.Id;
            newRide.ProviderName = user.Name;
            newRide.Route.Source = ride.Route.Source;
            newRide.Route.Destination = ride.Route.Destination;
            newRide.StartDateTime = ride.StartDateTime;
            newRide.NoOfOfferedSeats = ride.NoOfOfferedSeats;
            newRide.UnitDistanceCost = ride.UnitDistanceCost;
            newRide.VehicleId = ride.VehicleId;
            newRide.VehicleType = _userService.FindVehicle(ride.VehicleId, user.Id).Type;
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
                    }
                    To = ride.Route.ViaPoints[i].Location;
                    viaPoint.Distance = GetDistance(From, To);
                    viaPoint.Location = ride.Route.ViaPoints[i].Location;
                    newRide.Distance += viaPoint.Distance;
                    newRide.Route.ViaPoints.Add(viaPoint);
                }
                From = ride.Route.ViaPoints.Last().Location;
            }
            else
            {
                From = ride.Route.Source;
            }
            newRide.Distance += GetDistance(From, ride.Route.Destination);
            _rideService.CreateRide(newRide);
        }

        public float GetDistance(string from, string To)
        {
            string url = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + from + "&destinations=" + To + "&key=AIzaSyB3mJ4gmoEDfSjFISRXmngMlZarF0s6XcY";
            WebRequest request = WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            JourneyDetailsVM JsonResult = JsonConvert.DeserializeObject<JourneyDetailsVM>(responseFromServer);
            float dist = JsonResult.Rows[0].Elements[0].Distance.Value;
            return dist / 1000;
        }

        [HttpPost]
        [Route("SearchRide")]
        public IEnumerable<AvailableRideVM> SearchRide([FromBody]RequestVM request, [FromQuery]string userId)
        {
            List<Ride> rides = _rideService.FindRides(request.Source, request.Destination, request.Date, userId, request.NoOfSeats, Enum.Parse<VehicleType>(request.VehicleType.ToString()));
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
                var Vehicle = _userService.FindVehicle(item.VehicleId, item.ProviderId);
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
                    availableRide.Route.ViaPoints.Add(new ViaPointVM() { Location = viaPoint.Location, Distance = viaPoint.Distance });
                }
                availableRide.Cost = _rideService.CalculateCostForRide(item.Id, request.Source, request.Destination);
                availableRides.Add(availableRide);
            }
            return availableRides;
        }

        [HttpPost]
        [Route("RequestRide")]
        public ResponseVM RequestRide([FromBody]RequestVM request, [FromQuery] string rideId, [FromQuery] string userId)
        {
            ResponseVM response = new ResponseVM();
            _rideService.RequestRide(userId, rideId, request.Source, request.Destination, request.NoOfSeats);
            return response;
        }

        [HttpGet]
        [Route("GetBookings")]
        public List<BookingDetailsVM> GetBooking([FromQuery]string userId)
        {

            List<Ride> Rides = _rideService.FindBookings(userId);
            List<BookingDetailsVM> Bookings = new List<BookingDetailsVM>();
            foreach (var item in Rides)
            {
                List<Request> Requests = item.Requests.FindAll(_ => _.RiderId == userId);
                foreach (var request in Requests)
                {
                    var details = AddDetails(request, item);
                    details.Status = "Not Accepted";
                    details.Cost = _rideService.CalculateCostForRide(item.Id, request.PickUp, request.Drop) * request.NoOfSeats;
                    Bookings.Add(details);
                }
                List<Booking> bookings = item.Bookings.FindAll(_ => _.Request.RiderId == userId);
                foreach (var booking in bookings)
                {
                    var details = AddDetails(booking.Request, item);
                    details.Status = booking.Response.Status.ToString();
                    details.Cost = booking.Response.Cost;
                    Bookings.Add(details);
                }
            }
            return Bookings;
        }
        
        [NonAction]
        public BookingDetailsVM AddDetails(Request request, Ride item)
        {
            BookingDetailsVM details = new BookingDetailsVM
            {
                StartDateTime = item.StartDateTime,
                IsRideCompleted = item.IsRideCompleted,
                PickUp = request.PickUp,
                Drop = request.Drop,
                ProviderName = item.ProviderName,
                Route = new RouteVM()
                {
                    Source = item.Route.Source,
                    Destination = item.Route.Destination,
                    TotalDistance = item.Route.TotalDistance,
                    ViaPoints = new List<ViaPointVM>()
                },
            };
            foreach (var viaPoint in item.Route.ViaPoints)
            {
                details.Route.ViaPoints.Add(new ViaPointVM() { Location = viaPoint.Location, Distance = viaPoint.Distance });
            }
            Vehicle Vehicle = _userService.FindVehicle(item.VehicleId, item.ProviderId);
            switch (Vehicle.Type)
            {
                case VehicleType.Car:
                    details.Vehicle = new CarVM();
                    break;
                case VehicleType.Bike:
                    details.Vehicle = new BikeVM();
                    break;
            }
            details.Vehicle.Capacity = Vehicle.Capacity;
            details.Vehicle.Model = Vehicle.Model;
            details.Vehicle.Number = Vehicle.Number;
            return details;
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
                    OfferedRide.Route.ViaPoints.Add(new ViaPointVM() { Location = viaPoint.Location, Distance = viaPoint.Distance });
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
        public List<RequestDetailsVM> GetRequests([FromQuery]string rideId)
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
        public ResponseVM ApproveRequest([FromQuery]string rideId,[FromQuery]string requestId,[FromQuery]bool isApprove,[FromQuery]string providerId)
        {
            ResponseVM response = new ResponseVM
            {
                Result = true
            };
            if (isApprove)
            {
                Request request = _rideService.FindRide(rideId).Requests.Find(_=>_.RequestId==requestId);
                float amount = _rideService.CalculateCostForRide(rideId, requestId);
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
