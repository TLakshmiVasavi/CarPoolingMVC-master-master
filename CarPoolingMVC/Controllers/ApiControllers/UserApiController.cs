using System;
using System.Collections.Generic;
using CarPoolingMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Enums;
using Models.Interfaces;
using RestSharp;

namespace CarPoolingMVC.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserApiController : Controller
    {
        private readonly IUserService _userService;

        public UserApiController(IUserService userService)
        {
            this._userService = userService;
        }


        [HttpPost]
        [Route("SignUp")]
        [AllowAnonymous]
        public void SignUp([FromBody]UserVM user)
        {
            User User = new User
            {
                Age = user.Age,
                Name = user.Name,
                Mail = user.Mail,
                Number = user.Number,
                Password = user.Password,
                Gender = Enum.Parse<Gender>(user.Gender),
            };
            User.Vehicles = new List<Vehicle>();
            if (user.HasVehicle)
            {
                Vehicle Vehicle;
                switch (user.VehicleType)
                {
                    case VehicleTypeVM.Car:
                        Vehicle = new Car();
                        break;
                    case VehicleTypeVM.Bike:
                        Vehicle = new Bike();
                        break;
                    default:
                        Vehicle = new Vehicle();
                        break;
                }
                Vehicle.Model = user.Vehicle.Model;
                Vehicle.Number = user.Vehicle.Number;
                Vehicle.Capacity = user.Vehicle.Capacity ?? 2;
                User.Vehicles.Add(Vehicle);
            }
            _userService.SignUp(User);
            CookieOptions options = new CookieOptions()
            {
                Path = "/",
                //Domain = Request.Host.Value,
                //  Domain= HttpUtility.ParseQueryString(Request.u .Url.Query),
                //Expires = DateTime.Now.AddDays(1),
                Secure = true,
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Expires=DateTime.Now.AddDays(1)
            };
            string res = GenerateToken();
            Response.Cookies.Append("Bearer", res, options);
        }

        private static string GenerateToken()
        {
            var client = new RestClient("https://lakshmivasavi.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"xqngInHfNmrDL2wuMY2LTdRZjG8WOAKU\",\"client_secret\":\"nVWut_9QqOcaiBK9imKQIgkdsK75psbdZoY_Cy31n_Dwzhr_awgh6d67DIghG9bJ\",\"audience\":\"https://lakshmivasavi.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            return client.Execute(request).Content.Split(",")[0].Split(":")[1].Trim('"');
        }

        [HttpPost]
        [Route("AddVehicle")]
        public void AddVehicle([FromBody]VehicleVM vehicle, [FromQuery]string userId)
        {
            Vehicle NewVehicle;
            switch (vehicle.Type)
            {
                case VehicleTypeVM.Car:
                    NewVehicle = new Car();
                    break;
                case VehicleTypeVM.Bike:
                    NewVehicle = new Bike();
                    break;
                default:
                    NewVehicle = new Vehicle();
                    break;
            }
            NewVehicle.Model = vehicle.Model;
            NewVehicle.Number = vehicle.Number;
            NewVehicle.Capacity = vehicle.Capacity ?? 2;
            _userService.AddVehicle(userId, NewVehicle);
        }

        [HttpGet]
        [Route("GetVehicles")]
        public IEnumerable<string> GetVehicles([FromQuery]string userId)
        {
            return _userService.GetVehiclesId(userId);
        }

        [HttpGet]
        [Route("GetBalance")]
        public float GetBalance([FromQuery]string userId)
        {
            return _userService.ViewBalance(userId);
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public ResponseVM Login([FromQuery]string userId, [FromBody]string password)
        {
            ResponseVM response = new ResponseVM
            {
                Result = false
            };
            if (_userService.IsUserExist(userId))
            {
                if (_userService.Login(password, userId))
                {
                    response.Result = true;
                    CookieOptions options = new CookieOptions()
                    {
                        Path = "/",
                        //Domain = Request.Host.Value,
                        //  Domain= HttpUtility.ParseQueryString(Request.u .Url.Query),
                        //Expires = DateTime.Now.AddDays(1),
                        Secure = true,
                        HttpOnly = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.None
                    };
                    string res = GenerateToken();
                    Response.Cookies.Append("Bearer", res, options);
                }
                else
                {                    
                    response.ErrorMessage = "Invalid Password";
                }
            }
            else
            {
                response.ErrorMessage = "Invalid UserName";
            }
            return response;
        }

        [HttpPost]
        [Route("AddAmountToWallet")]
        public void AddAmount([FromBody]float amount, [FromQuery]string userId)
        {
            _userService.AddAmount(amount, userId);
        }

        [Authorize]
        public void Logout()
        {
            Response.Cookies.Delete("Bearer");
        }
    }
}
