using System.Collections.Generic;
using CarPoolingMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System;
using Models;
using Models.Enums;
using System.Net.Http;
using System.Text;

namespace CarPoolingMVC.Controllers
{
    public class RideController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRideService _rideService;
        private readonly IHttpClientFactory _httpClientFactory;

        public RideController(IUserService userService, IRideService rideService, IHttpClientFactory httpClientFactory)
        {
            this._userService = userService;
            this._rideService = rideService;
            this._httpClientFactory = httpClientFactory;
        }

        public async System.Threading.Tasks.Task<HttpResponseMessage> RequestApi(string absoluteUri, object _object, string method)
        {
            string baseUri = "https://localhost:44302/api/";
            using (HttpClient client = _httpClientFactory.CreateClient())
            {
                if (method == "post")
                {
                    return await client.PostAsync(baseUri + absoluteUri, new StringContent(JsonConvert.SerializeObject(_object), Encoding.UTF8, "application/json"));
                }
                else
                {
                    return client.GetAsync(baseUri + absoluteUri).Result;
                }
            }
        }

        public ActionResult OfferRide()
        {
            RideVM ride = new RideVM
            {
                AvailableVehicles = new List<string>(),
                Route = new RouteVM()
            };
            ride.Route.ViaPoints = new List<ViaPointVM>();
            //HttpResponseMessage response = RequestApi("UserApi/GetVehicles?userId=" + HttpContext.Session.GetString("UserId"), null, "get");
            HttpResponseMessage response;
            using (HttpClient client=_httpClientFactory.CreateClient())
            {
                response = client.GetAsync("https://localhost:44302/api/UserApi/GetVehicles?userId=" + HttpContext.Session.GetString("UserId")).Result;
            }
            
            ride.AvailableVehicles = response.Content.ReadAsAsync<List<string>>().Result;
            if (ride.AvailableVehicles.Count == 0)
            {
                ViewBag.Message = "Please add vehicle to offer ride";
                return View("../User/AddVehicle");
            }
            return View(ride);
        }       

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> OfferRide(RideVM ride)
        {
            HttpResponseMessage response = await RequestApi("RideApi/CreateRide?userId=" + HttpContext.Session.GetString("UserId"), ride, "post");
            return View(ride);
        }

        
        public ActionResult SearchRide()
        {
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> SearchRide(RequestVM request)
        {
            HttpResponseMessage response = await RequestApi("RideApi/SearchRide?userId=" + HttpContext.Session.GetString("UserId"), request, "post");
            SearchRideVM SearchRide = new SearchRideVM()
            {
                Request = request,
                AvailableRides = response.Content.ReadAsAsync<List<AvailableRideVM>>().Result
            };
            return View("AvailableRides", SearchRide);
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
        public async System.Threading.Tasks.Task<IActionResult> RequestRide(RequestVM request, string rideId)
        {
            HttpResponseMessage response = await RequestApi("RideApi/RequestRide?userId=" + HttpContext.Session.GetString("UserId")+"+rideId="+rideId, request, "post");
            return RedirectToAction("MyBookings");
        }        

        public async System.Threading.Tasks.Task<IActionResult> MyBookings()
        {
            HttpResponseMessage response = await RequestApi("RideApi/GetBookings?userId=" + HttpContext.Session.GetString("UserId"), null, "get");
            var Bookings = response.Content.ReadAsAsync<List<BookingDetailsVM>>().Result;
            return View(Bookings);
        }

        public async System.Threading.Tasks.Task<IActionResult> ViewOfferedRides()
        {
            HttpResponseMessage response = await RequestApi("RideApi/GetOfferedRides?userId=" + HttpContext.Session.GetString("UserId"), null, "get");
            var OfferedRides = response.Content.ReadAsAsync<List<OfferedRideVM>>().Result;
            return View(OfferedRides);
        }

        public async System.Threading.Tasks.Task<IActionResult> ViewRequests(string rideId)
        {
            HttpResponseMessage response = await RequestApi("RideApi/GetRequests?rideId=" + rideId, null, "get");
            var RideRequests = response.Content.ReadAsAsync<List<RequestDetailsVM>>().Result;
            ViewBag.RideId = rideId;
            return View(RideRequests);
        }

        public async System.Threading.Tasks.Task<IActionResult> ApproveRequest(string rideId,string requestId,string decision)
        {
            HttpResponseMessage response = await RequestApi("RideApi/ApproveRequest?rideId=" + rideId+"&providerId=" +
                HttpContext.Session.GetString("UserId")+"&requestId="+requestId+"&isApprove"+(decision=="Accept"), null, "get");
            
            return RedirectToAction("ViewRequests",rideId);
        }


    }
}