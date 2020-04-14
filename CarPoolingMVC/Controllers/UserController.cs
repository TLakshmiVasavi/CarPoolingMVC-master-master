using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;
using System.Net.Http;

namespace CarPoolingMVC.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _userService = userService;
        }        

        public IActionResult AddVehicle()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
       

        public IActionResult SignUp()
        {
            return View();
        }

        public ActionResult ViewBalance()
        {
            return View();
        }

        public ActionResult AddAmount()
        {
            return View();
        }

        
        public IActionResult GetProfile()
        {
            return View();
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Login");
        }

    }
}