using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
