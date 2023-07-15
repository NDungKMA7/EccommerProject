using EcommerceProject.DTO;
using EcommerceProject.Models.Mapping;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Controllers
{
    public class NewsController : Controller
    {
        private readonly MyDbContext _context;
        public NewsController(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page, int? id)
        {
            int CategoryId = id ?? 0;
            ViewBag.CategoryId = CategoryId;
            int current_page = page ?? 1;
            int record_per_page = 6;
            List<ItemNew> list_record = await _context.News.OrderByDescending(item => item.Id).ToListAsync();

            if (CategoryId > 0)
            {
                var list_products = await _context.News.Where(item => item.ListArticleId == CategoryId).ToListAsync();
            }
            
            return View("Index", list_record.ToPagedList(current_page, record_per_page));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            int _id = id ?? 0;
            ItemAdv _AdvSiderbar = await _context.Adv.Where(item => item.Position == 4).FirstOrDefaultAsync();
            ViewBag.AdvSiderbar = _AdvSiderbar;
            ViewBag.CategoryList = await _context.Categories.ToListAsync();
            ViewBag.TagList = await _context.Tags.ToListAsync();
            ItemNew record = await _context.News.Where(item => item.Id == _id).FirstOrDefaultAsync();
            ItemListArticle _CategoriesName =  _context.ListArticle.Where(item => item.Id == record.ListArticleId).FirstOrDefault();
            ViewBag.CategoriesName = _CategoriesName.Name;
            if (record != null)
            {
                return View("Detail", record);
            }
            return Redirect("/Home/Error404");
        }
    }
}
