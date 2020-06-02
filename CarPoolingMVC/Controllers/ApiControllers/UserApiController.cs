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
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CarPoolingMVC.Helpers;
using System.Net.Http.Headers;

namespace CarPoolingMVC.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
     [Authorize]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserApiController(IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            this._userService = userService;
            this._mapper = mapper;
            this._appSettings = appSettings.Value;
        }


        [HttpPost]
        [AllowAnonymous]
        public AuthResponseVM SignUp([FromForm] UserVM userVM)
        {
            AuthResponseVM response = new AuthResponseVM();
            UserResponse userResponse;
            userResponse = _userService.SignUp(_mapper.Map<User>(userVM));
            if (userResponse.ErrorMessage == null)
            {
                response = _mapper.Map<User, AuthResponseVM>(userResponse.User);
                response.IsSuccess = true;
                var x = GenerateToken(response.Mail, response.Role);
                response.Token = x;
            }
            else
            {
                response.ErrorMessage = userResponse.ErrorMessage;
                response.IsSuccess = false;
            }
            return response;
        }

        [HttpPost]
        public byte[] UpdateImage([FromForm] IFormFile Photo, [FromQuery] string userId)
        {

            return _userService.UpdateImage(_mapper.Map<byte[]>(Photo), userId);
        }

        [HttpPost]
        public List<VehicleVM> AddVehicle([FromBody] VehicleVM vehicle, [FromQuery] string userId)
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
            NewVehicle = _mapper.Map<Vehicle>(vehicle);
            return _mapper.Map<List<VehicleVM>>(_userService.AddVehicle(userId, NewVehicle));
        }

        [HttpGet]
        public List<VehicleVM> GetVehicles([FromQuery] string userId)
        {
            return _mapper.Map<List<VehicleVM>>(_userService.GetVehicles(userId));
        }

        [HttpGet]
        public float GetBalance([FromQuery] string userId)
        {
            return _userService.GetBalance(userId);
        }

        [HttpPost]
        [AllowAnonymous]
        public AuthResponseVM Login([FromBody] UserLoginVM userDto)
        {
            AuthResponseVM response = new AuthResponseVM();
            UserResponse userResponse;
            userResponse = _userService.Login(userDto.Password, userDto.Id);
            if (userResponse.ErrorMessage == null)
            {
                response = _mapper.Map<User, AuthResponseVM>(userResponse.User);
                response.IsSuccess = true;
                var x = GenerateToken(response.Mail,response.Role);
                response.Token = x;
            }
            else
            {
                response.ErrorMessage = userResponse.ErrorMessage;
                response.IsSuccess = false;
            }
            return response;
        }

        public static string GenerateToken(string username,string role,int expireMinutes = 20)
        {
            DateTime issuedAt = DateTime.UtcNow;
            DateTime expires = DateTime.UtcNow.AddDays(7);
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role,"User")
        });
            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var now = DateTime.UtcNow;
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(sec));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "http://localhost:5001", audience: "http://localhost:3000",
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        [HttpPost]
        public void UpdateBalance([FromBody] WalletVM wallet, [FromQuery] string userId)
        {
            _userService.AddAmount(wallet.Balance, userId);
        }

        [HttpPost]
        public void Logout([FromQuery]string userId)
        {
            Response.Cookies.Delete("Bearer");
        }

        [HttpGet]
        public User GetUser([FromQuery] string userId)
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
        [Authorize(Roles = Role.User)]
        public byte[] GetImage([FromQuery] string userId)
        {
            return _userService.GetImage(userId);
        }

        [HttpGet]
        public List<UserVM> GetAllUsers()
        {
            //Response.Headers.Add("Content-Type", new MediaTypeHeaderValue("image/png"));
            //Response.ContentType = ;
            var x=_mapper.Map<List<UserVM>>(_userService.GetAllUsers());
            return x;
           // content - type: application / json; charset = utf - 8
        }

        [HttpGet]
        public List<VehicleVM> GetAllVehicles()
        {
            return _mapper.Map<List<VehicleVM>>(_userService.GetAllVehicles());
        }

        [HttpPost]
        public string ChangePassword([FromBody]UpdatePasswordVM updatePassword,[FromQuery]string userId)
        {
            return _userService.ChangePassword(_mapper.Map<UpdatePassword>(updatePassword),userId) ? "Password Changed Successfully" : "Invalid Password";
        }

        [HttpPost]
        public void UpdateVehicle([FromBody]VehicleVM vehicle,[FromQuery]string userId,string vehicleId)
        {
            _userService.UpdateVehicle(_mapper.Map<Vehicle>(vehicle), userId,vehicleId);
        }

        [HttpGet]
        public List<TransactionVM> GetTransactions([FromQuery]string userId)
        {
            List<Transaction> transactions = _userService.GetTransactions(userId);

            var result=_mapper.Map<List<TransactionVM>>(transactions);
            for(int i=0;i<transactions.Count;i++)
            {
                if (transactions[i].From == userId)
                {
                    result[i].PaymentMessage =  " paid to " + transactions[i].To;
                        }
                else
                {
                    result[i].PaymentMessage = " received from " + transactions[i].From;
                }

            }
            return result;
        }
    }
}
