using Microsoft.AspNetCore.Mvc;

namespace Frontend_Dashboard.Controllers
{
    public class CursoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
