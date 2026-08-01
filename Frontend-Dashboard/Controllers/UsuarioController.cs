using Microsoft.AspNetCore.Mvc;

namespace Frontend_Dashboard.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
