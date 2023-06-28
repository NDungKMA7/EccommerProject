using EcommerceProject.Models.Mapping;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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
            ViewBag.ListTagProducts = await _context.TagsProducts.ToListAsync();
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
          
            return View("Index", _ListRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }

        public async Task<IActionResult> Update(int? id) {
            int _id = id ?? 0;
            ItemProduct record = await _context.Products.Where(item => item.Id == _id).FirstOrDefaultAsync();
            if(record == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.ListCategoriesProducts = await _context.CategoriesProducts.ToListAsync();
            ViewBag.ListTagProducts = await _context.TagsProducts.ToListAsync();
            ViewBag.action = "/Admin/Products/UpdatePost/" + _id;
            return View("CreateUpdate", record);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePost(IFormCollection fc, int? id)
        {
            int _id = id ?? 0;
            string _name = fc["name"].ToString().Trim();
            double _price = Convert.ToDouble(fc["price"].ToString().Trim());
            double _discount = Convert.ToDouble(fc["discount"].ToString().Trim());
            int _hot = fc["hot"] != "" && fc["hot"] == "on" ? 1 : 0;
            string _description = fc["description"].ToString().Trim();
            string _content = fc["content"].ToString().Trim();

            ItemProduct record = await _context.Products.Where(item => item.Id == _id).FirstOrDefaultAsync();
            if(record != null)
            {
                record.Name = _name;
                record.Price = _price;
                record.Discount = _discount;
                record.Hot = _hot;
                record.Description = _description;
                record.Content = _content;
                string _FileName = "";
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.Photo)))
                    {
                        string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", record.Photo);
                        System.IO.File.Delete(oldFilePath);
                    }
                    var timestamp = DateTime.Now.ToFileTime();
                    _FileName = timestamp + "_" + file.FileName;
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", _FileName);
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    record.Photo = _FileName;
                }
                await _context.SaveChangesAsync();

                List<ItemCategory> list_categories = await _context.Categories.ToListAsync();
                List<ItemCategoriesProducts> list_category_product = await _context.CategoriesProducts.Where(item => item.ProductId == _id).ToListAsync();
                foreach (var item in list_category_product)
                {
                    _context.CategoriesProducts.Remove(item);
                }
                await _context.SaveChangesAsync();

                foreach (var itemCategory in list_categories)
                {
                    string formName = "category_" + itemCategory.Id;
                    if (!String.IsNullOrEmpty(Request.Form[formName]))
                    {
                        ItemCategoriesProducts recordCategoryProduct = new ItemCategoriesProducts();
                        recordCategoryProduct.CategoryId = itemCategory.Id;
                        recordCategoryProduct.ProductId = _id;
                        _context.CategoriesProducts.Add(recordCategoryProduct);
                        await _context.SaveChangesAsync();
                    }
                }

                List<ItemTagsProducts> list_tag_product = await _context.TagsProducts.Where(item => item.ProductId == _id).ToListAsync();
                foreach (var item_tag_product in list_tag_product)
                {
                    _context.TagsProducts.Remove(item_tag_product);
                }
                await _context.SaveChangesAsync();

                List<string> list_id_tags = Request.Form["tags"].ToList();
                foreach (var tag_id in list_id_tags)
                {
                    ItemTagsProducts record_tag_product = new ItemTagsProducts();
                    record_tag_product.TagId = Convert.ToInt32(tag_id);
                    record_tag_product.ProductId = _id;
                    _context.TagsProducts.Add(record_tag_product);
                    await _context.SaveChangesAsync();
                }

            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.ListCategoriesProducts = await _context.CategoriesProducts.ToListAsync();
            ViewBag.ListTagProducts = await _context.TagsProducts.ToListAsync();
            ViewBag.action = "/Admin/Products/CreatePost";
            return View("CreateUpdate");
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(IFormCollection fc)
        {
            
            string _name = fc["name"].ToString().Trim();
            double _price = Convert.ToDouble(fc["price"].ToString().Trim());
            double _discount = Convert.ToDouble(fc["discount"].ToString().Trim());
            int _hot = fc["hot"] != "" && fc["hot"] == "on" ? 1 : 0;
            string _description = fc["description"].ToString().Trim();
            string _content = fc["content"].ToString().Trim();
            ItemProduct record = new ItemProduct();
            record.Name = _name;
            record.Price = _price;
            record.Discount = _discount;
            record.Hot = _hot;
            record.Description = _description;
            record.Content = _content;
            string _FileName = "";
            if (Request.Form.Files.Count > 0)
            {

                var file = Request.Form.Files[0];
                var timestamp = DateTime.Now.ToFileTime();
                _FileName = timestamp + "_" + file.FileName;
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", _FileName);
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                record.Photo = _FileName;
            }
            _context.Products.Add(record);
            await _context.SaveChangesAsync();
            List<ItemCategory> list_categories = await _context.Categories.ToListAsync();
            int insert_id = record.Id;

            foreach (var itemCategory in list_categories)
            {
                string formName = "category_" + itemCategory.Id;
                if (!String.IsNullOrEmpty(Request.Form[formName]))
                {
                    ItemCategoriesProducts recordCategoryProduct = new ItemCategoriesProducts();
                    recordCategoryProduct.CategoryId = itemCategory.Id;
                    recordCategoryProduct.ProductId = insert_id;
         
                    _context.CategoriesProducts.Add(recordCategoryProduct);
                    await _context.SaveChangesAsync();
                }
            }

            List<string> list_id_tags = Request.Form["tags"].ToList();
            foreach (var tag_id in list_id_tags)
            {
                ItemTagsProducts record_tag_product = new ItemTagsProducts();
                record_tag_product.TagId = Convert.ToInt32(tag_id);
                record_tag_product.ProductId = insert_id;
                _context.TagsProducts.Add(record_tag_product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            int _id = id ?? 0;
          
            ItemProduct record = await _context.Products.Where(item => item.Id == _id).FirstOrDefaultAsync();
           if(record != null)
            {
                _context.Products.Remove(record);
                await _context.SaveChangesAsync();
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.Photo));

            }
           
      
            return RedirectToAction("Index");
        }
    }
}
