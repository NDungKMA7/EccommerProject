using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
