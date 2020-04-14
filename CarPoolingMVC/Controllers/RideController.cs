using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;
using CarPoolingMVC.Models;

namespace CarPoolingMVC.Controllers
{
    public class RideController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRideService _rideService;

        public RideController(IUserService userService, IRideService rideService,IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            this._userService = userService;
            this._rideService = rideService;
        }

        public ActionResult OfferRide()
        {
            RideVM ride = new RideVM
            {
                AvailableVehicles = new List<string>(),
                Route = new RouteVM()
            };
            ride.Route.ViaPoints = new List<ViaPointVM>();
            HttpResponseMessage response = GetApi("UserApi/GetVehicles?userId=" + HttpContext.Session.GetString("UserId"));
            ride.AvailableVehicles = response.Content.ReadAsAsync<List<string>>().Result;
            if (ride.AvailableVehicles.Count == 0)
            {
                ViewBag.Message = "Please add vehicle to offer ride";
                return View("../User/AddVehicle");
            }
            return View(ride);
        }       

        [HttpPost]
        public async Task<ActionResult> OfferRide(RideVM ride)
        {
            HttpResponseMessage response = await RequestApi("RideApi/CreateRide?userId=" + HttpContext.Session.GetString("UserId"), ride, "post");
            return View(ride);
        }

        
        public ActionResult SearchRide()
        {
            return View();
        }

        [HttpPost]
        public  IActionResult SearchRide(RequestVM request)
        {
            //HttpResponseMessage response = await RequestApi("RideApi/SearchRide?userId=" + HttpContext.Session.GetString("UserId"), request, "post");
            //SearchRideVM SearchRide = new SearchRideVM()
            //{
            //    Request = request,
            //    AvailableRides = response.Content.ReadAsAsync<List<AvailableRideVM>>().Result
            //};
            return PartialView("AvailableRides", request);
        }

        [HttpPost]
        public ActionResult AddViaPoint(RideVM ride)
        {
            if (ride.Route.ViaPoints == null)
            {
                ride.Route.ViaPoints = new List<ViaPointVM>();
            }
            ride.Route.ViaPoints.Add(new ViaPointVM());
            return PartialView("ViaPoint", ride);
        }

        [HttpPost]
        public IActionResult Save(RideVM ride)
        {
            return PartialView("DisplayViaPoints", ride);
        }

        [HttpPost]
        public async Task<IActionResult> RequestRide(RequestVM request, string rideId)
        {
            HttpResponseMessage response = await RequestApi("RideApi/RequestRide?userId=" + HttpContext.Session.GetString("UserId")+"&rideId="+rideId, request, "post");
            return RedirectToAction("MyBookings");
        }        

        public IActionResult MyBookings()
        {
            HttpResponseMessage response = GetApi("RideApi/GetBookings?userId=" + HttpContext.Session.GetString("UserId"));
            var Bookings = response.Content.ReadAsAsync<List<BookingDetailsVM>>().Result;
            //return View(Bookings);
            return PartialView(Bookings);
        }

        public IActionResult ViewOfferedRides()
        {
            HttpResponseMessage response = GetApi("RideApi/GetOfferedRides?userId=" + HttpContext.Session.GetString("UserId"));
            var OfferedRides = response.Content.ReadAsAsync<List<OfferedRideVM>>().Result;
            return PartialView(OfferedRides);
        }

        public IActionResult ViewRequests(string rideId)
        {
            //HttpResponseMessage response = GetApi("RideApi/GetRequests?rideId=" + rideId);
            //var RideRequests = response.Content.ReadAsAsync<List<RequestDetailsVM>>().Result;
            ViewBag.RideId = rideId;
            return View();
        }

        public IActionResult ApproveRequest(string rideId,string requestId,string decision)
        {
            HttpResponseMessage response = GetApi("RideApi/ApproveRequest?rideId=" + rideId+"&providerId=" +
                HttpContext.Session.GetString("UserId")+"&requestId="+requestId+"&isApprove"+(decision=="Accept"));
            
            return RedirectToAction("ViewRequests",rideId);
        }

        public IActionResult MyRides()
        {
            return View();
        }
    }
}