using EcommerceProject.DTO;
using EcommerceProject.Models;
using EcommerceProject.Models.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
namespace EcommerceProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MyDbContext _context;
        public ProductsController(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page, int? id)
        {
            int CategoryId = id ?? 0;
            ViewBag.CategoryId = CategoryId;
            int current_page = page ?? 1;
            int record_per_page = 12;
            List<ItemProduct> list_record = await _context.Products.OrderByDescending(item => item.Id).ToListAsync();

            if (CategoryId > 0)
            {
                var list_products = await _context.CategoriesProducts.Where(item => item.CategoryId == CategoryId).Select(item => new { item.ProductId }).ToListAsync();
                List<int> id_products = new List<int>();
                foreach (var item in list_products)
                {
                    id_products.Add(item.ProductId);
                }
                
                list_record = list_record.Where(item => id_products.Contains(item.Id)).ToList();
                
                string strOrder = "";
                if (!String.IsNullOrEmpty(Request.Query["order"]))
                    strOrder = Request.Query["order"];
                switch (strOrder)
                {
                    case "name-asc":
                        list_record = list_record.OrderBy(item => item.Name).ToList();
                        break;
                    case "newness":
                        list_record = list_record.OrderByDescending(item => item.Id).ToList();
                        break;
                    case "price-asc":
                        list_record = list_record.OrderBy(item => item.Price).ToList();
                        break;
                    case "price-desc":
                        list_record = list_record.OrderByDescending(item => item.Price).ToList();
                        break;
                }

            }

            ItemAdv _AdvSiderbar = await _context.Adv.Where(item=> item.Position == 4).FirstOrDefaultAsync();
            ViewBag.AdvSiderbar = _AdvSiderbar;
            ViewBag.CategoryList = await _context.Categories.ToListAsync();
            ViewBag.TagList = await _context.Tags.ToListAsync();    
            return View("Index", list_record.ToPagedList(current_page, record_per_page));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            int _id = id ?? 0;
            List<ProductHomeDTO> _ProductsList = await _context.Products
            .OrderByDescending(item => item.Id)
            .Select(item => new ProductHomeDTO
            {
                Name = item.Name,
                Price = item.Price,
                Discount = item.Discount,
                Id = item.Id,
                Photo = item.Photo,
                Hot = item.Hot
            })
            .ToListAsync();
            ViewBag.ProductsList = _ProductsList;
            ItemProduct record = await _context.Products.Where(item => item.Id == _id).FirstOrDefaultAsync();
            string _CategoriesName = string.Join(", ", _context.CategoriesProducts
                       .Where(cp => cp.ProductId == record.Id)
                       .Join(_context.Categories, cp => cp.CategoryId, c => c.Id, (cp, c) => c.Name));
            ViewBag.CategoriesName = _CategoriesName;
            ViewBag.breadcrumb = record.Name;    
            if (record != null)
            {
                return View("Detail",record);
            }
            return Redirect("/Home/Error404");
        }
    }
}
