using Microsoft.AspNetCore.Mvc;

namespace CarPoolingMVC.Controllers
{
    public class RideController:Controller
    {
        public IActionResult OfferRide()
        {
            return View();
        }       

        public IActionResult BookRide()
        {
            return View();
        }

        public IActionResult MyRides()
        {
            return View();
        }
    }
}