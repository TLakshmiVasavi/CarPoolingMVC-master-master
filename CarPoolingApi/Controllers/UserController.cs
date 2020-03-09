using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace CarPoolingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
       [HttpPost]
       [Route("Login")]
       
        public void Post(User user)
        { 
        //{
        //    if (UserService.IsUserExist(user.Id))
        //    {
        //        ModelState.AddModelError("", "User name not valid");
        //    }
        //    else
        //    if (UserService.Login(user.Password, user.Id))
        //    {
        //        HttpContext.Session.SetString("UserId", user.Id);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Password not valid");
        //    }
        //    return View();
        }
    }
}
