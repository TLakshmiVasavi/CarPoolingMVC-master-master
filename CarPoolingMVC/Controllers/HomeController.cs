using Microsoft.AspNetCore.Mvc;
using Models;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace CarPoolingMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        

    }
}
