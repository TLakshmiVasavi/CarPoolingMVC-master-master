using CarPoolingMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

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

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> AddVehicle(VehicleVM vehicle)
        {
            HttpResponseMessage response = await RequestApi("UserApi/AddVehicle?userId=" + HttpContext.Session.GetString("UserId"), vehicle, "post");
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Login(UserVM user)
        {
            HttpResponseMessage response = await RequestApi("UserApi/Login?userId=" + user.Id, user.Password, "post");
            var res= JsonConvert.DeserializeObject<ResponseVM>(response.Content.ReadAsStringAsync().Result);
            if (res.Result)
            {
                HttpContext.Session.SetString("UserId", user.Id);
                CookieOptions options = new CookieOptions()
                {
                    Path = "/",
                    Secure = true,
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                };
                Response.Cookies.Append("Bearer", response.Headers.GetValues("Set-Cookie").FirstOrDefault().Split("=")[1], options);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Result = res.ErrorMessage;
            return View();
        }

        public IActionResult SignUp()
        {
            UserVM User = new UserVM
            {
                HasVehicle = true
            };
            return View(User);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> SignUp(UserVM user)
        {
            HttpResponseMessage response = await RequestApi("UserApi/SignUp", user, "post");
            HttpContext.Session.SetString("UserId", user.Mail);
            CookieOptions options = new CookieOptions()
            {
                Path = "/",
                Secure = true,
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("Bearer", response.Headers.GetValues("Set-Cookie").FirstOrDefault().Split("=")[1],options);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ViewBalance()
        {
            HttpResponseMessage response = GetApi("UserApi/GetBalance?userId=" + HttpContext.Session.GetString("UserId"));
            return View("ViewBalance",response.Content.ReadAsAsync<float>().Result);
        }

        public ActionResult AddAmount()
        {
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> AddAmount(float amount)
        {
            HttpResponseMessage response = await RequestApi("UserApi/AddAmountToWallet?userId=" + HttpContext.Session.GetString("UserId"), amount, "post");
            return RedirectToAction("ViewBalance");
        }

        public ActionResult Logout()
        {
           // Response.Cookies.Delete("Bearer");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public JsonResult IsSeatsAvailable(int noOfOfferedSeats, string vehicleId)
        {
           // return Json((_userService.FindVehicle(vehicleId, HttpContext.Session.GetString("UserId")).Capacity > noOfOfferedSeats));
            return Json((_userService.FindVehicle(vehicleId).Capacity > noOfOfferedSeats));
        }

        public JsonResult IsUserExists(string mail)
        {
            return Json(!_userService.IsUserExist(mail));
        }
    }
}