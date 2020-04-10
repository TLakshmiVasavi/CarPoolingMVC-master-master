using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System;
using Models.ViewModels;

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
            var res= JsonConvert.DeserializeObject<LoginResponse>(response.Content.ReadAsStringAsync().Result);
            if (res.User!=null)
            {
                HttpContext.Session.SetString("UserId", res.User.Mail);
                HttpContext.Session.SetString("UserName", res.User.Name);
                HttpContext.Session.SetString("UserImage", string.Format("data:image/png;base64,{0}", Convert.ToBase64String(res.User.Photo)));
                //CookieOptions options = new CookieOptions()
                //{
                //    Path = "/",
                //    Secure = true,
                //    HttpOnly = true,
                //    IsEssential = true,
                //    SameSite = SameSiteMode.None
                //};
                //Response.Cookies.Append("Bearer", response.Headers.GetValues("Set-Cookie").FirstOrDefault().Split("=")[1], options);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Result = res.ErrorMessage;
            }
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
        public async System.Threading.Tasks.Task<IActionResult> SignUp(UserVM user,IFormFile photo)
        {
            using (var stream=new MemoryStream()) 
            {
                await photo.CopyToAsync(stream);
                user.Photo = stream.ToArray();
            }
            HttpResponseMessage response = await RequestApi("UserApi/SignUp", user, "post");
            HttpContext.Session.SetString("UserId", user.Mail);
            //CookieOptions options = new CookieOptions()
            //{
            //    Path = "/",
            //    Secure = true,
            //    HttpOnly = true,
            //    IsEssential = true,
            //    SameSite = SameSiteMode.None
            //};
            //Response.Cookies.Append("Bearer", response.Headers.GetValues("Set-Cookie").FirstOrDefault().Split("=")[1],options);
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

        public IActionResult GetProfile()
        {
            HttpResponseMessage response = GetApi("UserApi/GetUser?userId=" + HttpContext.Session.GetString("UserId"));
            var user = response.Content.ReadAsAsync<UserVM>().Result;
            
            ViewBag.Base64String = string.Format("data:image/png;base64,{0}",Convert.ToBase64String(user.Photo));
            return View(user);
        }

        public ActionResult Logout()
        {
           // Response.Cookies.Delete("Bearer");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public JsonResult IsSeatsAvailable(int noOfOfferedSeats, string vehicleId)
        {
           return Json((_userService.FindVehicle(vehicleId).Capacity > noOfOfferedSeats));
        }

        public JsonResult IsUserExists(string mail)
        {
            return Json(!_userService.IsUserExist(mail));
        }
    }
}