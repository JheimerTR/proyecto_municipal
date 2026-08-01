using Microsoft.AspNetCore.Mvc;

namespace Frontend_Dashboard.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Login/Index.cshtml");
        }
    }
}