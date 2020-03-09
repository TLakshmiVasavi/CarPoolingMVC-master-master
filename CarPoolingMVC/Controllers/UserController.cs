using CarPoolingMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace CarPoolingMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IUserService userService, IHttpClientFactory httpClientFactory)
        {
            this._userService = userService;
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
            var res= JsonConvert.DeserializeObject<Models.ResponseVM>(response.Content.ReadAsStringAsync().Result);
            if (res.Result)
            {
                HttpContext.Session.SetString("UserId", user.Id);
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
            return RedirectToAction("Index", "Home");
        }

        public async System.Threading.Tasks.Task<ActionResult> ViewBalance()
        {
            HttpResponseMessage response = await RequestApi("UserApi/GetBalance?userId=" + HttpContext.Session.GetString("UserId"), null, "get");
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
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public JsonResult IsSeatsAvailable(int noOfOfferedSeats, string vehicleId)
        {
            return Json(!(_userService.FindVehicle(vehicleId, HttpContext.Session.GetString("UserId")).Capacity > noOfOfferedSeats));
        }

        public JsonResult IsUserExists(string mail)
        {
            return Json(!_userService.IsUserExist(mail));
        }
    }
}