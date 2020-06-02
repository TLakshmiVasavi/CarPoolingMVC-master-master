using CarPoolingMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;
using System.Net.Http;
using System.IdentityModel.Tokens.Jwt;

namespace CarPoolingMVC.Controllers
{
    public class UserController : Controller
    {
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
            return View(new UserVM());
        }

        public ActionResult ViewBalance()
        {
            return View();
        }

        public ActionResult UpdateBalance()
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