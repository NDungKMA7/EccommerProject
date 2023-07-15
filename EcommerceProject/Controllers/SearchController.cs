using EcommerceProject.Models;
using EcommerceProject.Models.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using X.PagedList;

namespace EcommerceProject.Controllers
{
    public class SearchController : Controller
    {
        private readonly MyDbContext _context;
        public SearchController(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> SearchPrice(int? page )
        {
            int current_page = page ?? 1;
            int record_per_page = 12;
            string fromPriceText = Request.Query["fromPrice"];
            string toPricetext = Request.Query["toPrice"];
            float fromPrice  ;
            float toPrice ;
            var numbers = Regex.Matches(fromPriceText, @"\d+")
                             .Cast<Match>()
                             .Select(m => float.Parse(m.Value))
                             .ToList();
            if(numbers.Count > 1)
            {
                 fromPrice = numbers[0];
                 toPrice = numbers[1];
            }
            else
            {
                fromPrice = float.Parse(fromPriceText);
                toPrice = float.Parse(toPricetext);
            }
            

            ViewBag.fromPrice = fromPrice;
            ViewBag.toPrice = toPrice;  
            List<ItemProduct> list_record = await _context.Products.Where(item => item.Price - item.Price * item.Discount / 100 >= fromPrice && item.Price - item.Price * item.Discount / 100 <= toPrice).OrderByDescending(item => item.Id).ToListAsync();
            ItemAdv _AdvSiderbar = await _context.Adv.Where(item => item.Position == 4).FirstOrDefaultAsync();
            ViewBag.AdvSiderbar = _AdvSiderbar;
            ViewBag.CategoryList = await _context.Categories.ToListAsync();
            
            string strOrder = "";
            if (!String.IsNullOrEmpty(Request.Query["order"]))
                strOrder = Request.Query["order"];
            list_record = sortRequest(list_record, strOrder);

            return View("Search", list_record.ToPagedList(current_page, record_per_page));
        }
        public IActionResult Tag(int? page, int? id)
        {
            int _id = id ?? 0;
            ViewBag._id = id;
            int current_page = page ?? 1;
            int record_per_page = 12;
            var list_record = from tp in _context.TagsProducts
                                            join p in _context.Products on tp.ProductId equals p.Id
                                            where tp.TagId == id
                                            select p;
            List<ItemProduct> list_product = list_record.ToList();
            string strOrder = "";
            if (!String.IsNullOrEmpty(Request.Query["order"]))
                strOrder = Request.Query["order"];
            list_product = sortRequest(list_product, strOrder);
            return View("Search", list_product.ToPagedList(current_page, record_per_page));
        }
        public List<ItemProduct> sortRequest(List<ItemProduct> list_record, string strOrder)
        {
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
            return list_record;
        }

    }
}
