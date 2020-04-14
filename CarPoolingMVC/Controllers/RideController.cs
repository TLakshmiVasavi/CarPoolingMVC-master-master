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

        public ActionResult SearchRide()
        {
            return View();
        }

        public IActionResult MyRides()
        {
            return View();
        }
    }
}