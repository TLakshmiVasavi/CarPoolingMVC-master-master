using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarPoolingMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.image = HttpContext.Session.GetString("UserImage");
            ViewBag.name = HttpContext.Session.GetString("UserName");
            return View();
        }
    }
}
