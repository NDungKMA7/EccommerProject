using EcommerceProject.Models.Mapping;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Seller")]
    public class ProductsController : Controller
    {


        private readonly MyDbContext _context;
        public ProductsController(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page)
        {
            int _RecordPerPage = 20;
            int _CurrentPage = page ?? 1;
            List<ItemProduct> _ListRecord = await _context.Products.OrderByDescending(item => item.Id).ToListAsync();
            ViewBag.ListCategoriesProducts = await _context.CategoriesProducts.ToListAsync();   
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View("Index", _ListRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }

        public async Task<IActionResult> Update(int? id) {
            int _id = id ?? 0;
            ItemProduct record = await _context.Products.Where(item => item.Id == _id).FirstOrDefaultAsync();
            if(record == null)
            {
                return NotFound();
            }
          
            return View("CreateUpdate", record);
        }
        [HttpPost]
        public IActionResult UpdatePost(IFormCollection fc, int? id)
        {
            return RedirectToAction("Index");
        }
    }
}
