using Microsoft.AspNetCore.Mvc;

namespace Frontend_Dashboard.Controllers
{
    public class IncidenteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
