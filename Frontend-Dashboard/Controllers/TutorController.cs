using Microsoft.AspNetCore.Mvc;

namespace Frontend_Dashboard.Controllers
{
    public class TutorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
