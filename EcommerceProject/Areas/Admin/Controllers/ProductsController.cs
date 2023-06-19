using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Seller")]
    public class ProductsController : Controller
    {
      
        
        public IActionResult Index()
        {
            return View("Index", "nguvc");
        }
    }
}
