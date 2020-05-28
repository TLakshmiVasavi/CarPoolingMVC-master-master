using System.Collections.Generic;
using CarPoolingMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Interfaces;
using RestSharp;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CarPoolingMVC.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
   // [Authorize]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserApiController(IUserService userService, IMapper mapper)
        {
            this._userService = userService;
            this._mapper = mapper;
        }


        [HttpPost]
        //[Route("SignUp")]
        [AllowAnonymous]
        public AuthResponseVM SignUp([FromForm]UserVM userVM)
        {
            AuthResponseVM authResponse = new AuthResponseVM();
            UserResponse response = _userService.SignUp(_mapper.Map<User>(userVM));
            if (response.ErrorMessage!=null)
            {
                authResponse.ErrorMessage = response.ErrorMessage;
                authResponse.IsSuccess = false;
            }
            else
            {
                authResponse.IsSuccess = true;
                authResponse.User = _mapper.Map<User, UserVM>(response.User);
                //CookieOptions options = new CookieOptions()
                //{
                //    Path = "/",
                //    Secure = true,
                //    HttpOnly = true,
                //    IsEssential = true,
                //    SameSite = SameSiteMode.None
                //};
                //Response.Cookies.Append("userId", userVM.Mail, options);
                //Response.Cookies.Append("userName", userVM.Name, options);
            }
            return authResponse;
        }

        [HttpPost]
        public byte[] UpdateImage([FromForm]IFormFile Photo,[FromQuery]string userId)
        {
            
            return _userService.UpdateImage(_mapper.Map<byte[]>(Photo), userId);
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
        //[Route("AddVehicle")]
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
        //[Route("GetVehicles")]
        public List<VehicleVM> GetVehicles([FromQuery]string userId)
        {
            return _mapper.Map<List<VehicleVM>>(_userService.GetVehicles(userId));
        }

        [HttpGet]
        //[Route("GetBalance")]
        public float GetBalance([FromQuery]string userId)
        {
            return _userService.GetBalance(userId);
        }

        [HttpPost]
        //[Route("Login")]
        [AllowAnonymous]
        //public LoginResponse Login([FromQuery]string userId, [FromBody]string password)
        public AuthResponseVM Login([FromBody]UserLoginVM userDto)
        {
            AuthResponseVM response = new AuthResponseVM();
            UserResponse userResponse;
            userResponse = _userService.Login(userDto.Password, userDto.Id);
            if (userResponse.ErrorMessage == null)
            {
                response.IsSuccess = true;
                response.User = _mapper.Map<User, UserVM>(userResponse.User);
                //CookieOptions options = new CookieOptions()
                //{
                //    Path = "/",
                //    Secure = true,
                //    HttpOnly = true,
                //    IsEssential = true,
                //    SameSite = SameSiteMode.None
                //};
                //Response.Cookies.Append("userId", userResponse.User.Mail, options);
                //Response.Cookies.Append("userName", userResponse.User.Name, options);
                //string res = GenerateToken();
                //Response.Cookies.Append("Bearer", res, options);
            }
            else
            {
                response.ErrorMessage = userResponse.ErrorMessage;
                response.IsSuccess = false;
            }
            return response;
        }

        [HttpPost]
        //[Route("AddAmountToWallet")]
        public void UpdateBalance([FromBody]Wallet wallet, [FromQuery]string userId)
        {
            _userService.AddAmount(wallet.Balance, userId);
        }

       [HttpPost]
        //[Route("Logout")]
        public void Logout([FromQuery]string userId)
        {
            Response.Cookies.Delete("Bearer");
        }

        //[Route("GetUser")]
        public User GetUser([FromQuery]string userId)
        {
            return _userService.FindUser(userId);
        }

        [HttpPost]
        public UserVM UpdateUser([FromForm] UserVM userDto)
        {
            User user = _mapper.Map<User>(userDto);
            return _mapper.Map<UserVM>(_userService.UpdateUserDetails(user));
        }

        

        [HttpGet]
        public byte[] GetImage([FromQuery]string userId)
        {
            return _userService.GetImage(userId);
        }
    }
}
