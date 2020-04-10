using System.Collections.Generic;
using Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Interfaces;
using RestSharp;
using AutoMapper;

namespace CarPoolingMVC.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize]
    public class UserApiController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserApiController(IUserService userService, IMapper mapper)
        {
            this._userService = userService;
            this._mapper = mapper;
        }


        [HttpPost]
        [Route("SignUp")]
        [AllowAnonymous]
        public void SignUp([FromBody]UserVM user)
        {
            User User = _mapper.Map<User>(user);
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
                        user.Vehicle.Capacity = 2;
                        break;
                    default:
                        Vehicle = new Vehicle();
                        break;
                }
                Vehicle = _mapper.Map<Vehicle>(user.Vehicle);
                User.Vehicles.Add(Vehicle);
            }
            _userService.SignUp(User);
            //CookieOptions options = new CookieOptions()
            //{
            //    Path = "/",
            //    Secure = true,
            //    HttpOnly = true,
            //    IsEssential = true,
            //    SameSite = SameSiteMode.None,
            //    Expires=DateTime.Now.AddDays(1)
            //};
            //string res = GenerateToken();
            //Response.Cookies.Append("Bearer", res,options);
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
                    vehicle.Capacity = 2;
                    break;
                default:
                    NewVehicle = new Vehicle();
                    break;
            }
            NewVehicle =_mapper.Map<Vehicle>( vehicle);
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
            return _userService.GetBalance(userId);
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public LoginResponse Login([FromQuery]string userId, [FromBody]string password)
        {
            LoginResponse response = new LoginResponse();
            if (_userService.IsUserExist(userId))
            {
                User user = _userService.Login(password, userId);
                if (user!=null)
                {
                    response.User = _mapper.Map<UserVM>(user);
                    //CookieOptions options = new CookieOptions()
                    //{
                    //    Path = "/",
                    //    Secure = true,
                    //    HttpOnly = true,
                    //    IsEssential = true,
                    //    SameSite = SameSiteMode.None
                    //};
                    //string res = GenerateToken();
                    //Response.Cookies.Append("Bearer", res, options);
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
        [Route("Logout")]
        public void Logout()
        {
            Response.Cookies.Delete("Bearer");
        }

        [Route("GetUser")]
        public User GetUser([FromQuery]string userId)
        {
            return _userService.FindUser(userId);
        }
    }
}
