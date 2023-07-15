using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
