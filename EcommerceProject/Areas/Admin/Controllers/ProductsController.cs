using EcommerceProject.Models.Mapping;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using EcommerceProject.DTO;
using System.Diagnostics;

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
        public IActionResult Index()
        {
            return View("Index");
        }

        public async Task<List<InfoProducts>> GetListProducts()
        {
            var products = await _context.Products
                .OrderByDescending(item => item.Id)
                .Select(product => new InfoProducts
                {
                    Id = product.Id,
                    Name = product.Name,
                    Discount = product.Discount,
                    Price = product.Price,
                    Hot = product.Hot,
                    Photo = product.Photo,
                    ImgSub = product.ImgSub,
                    Categories = string.Join(", ", _context.CategoriesProducts
                        .Where(cp => cp.ProductId == product.Id)
                        .Join(_context.Categories, cp => cp.CategoryId, c => c.Id, (cp, c) => c.Name)),
                    Tags = string.Join(", ", _context.TagsProducts
                        .Where(tp => tp.ProductId == product.Id)
                        .Join(_context.Tags, tp => tp.TagId, t => t.Id, (tp, t) => t.Name))
                })
                .ToListAsync();

            return products;
        }



        public async Task<IActionResult> Update(int? id) {
            int _id = id ?? 0;
            ItemProduct record = await _context.Products.Where(item => item.Id == _id).FirstOrDefaultAsync();
            if(record == null)
            {
                return Redirect("/Admin/Home/ErrorPage");
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
                
                
                if (Request.Form.Files.Count > 0)
                {
                    var listImgSub = record.ImgSub.Split(", ");
                    for(var item =0; item < listImgSub.Length - 1; item++)
                    {
                        if (listImgSub[item] != " ")
                        {
                            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", listImgSub[item]));
                        }
                    }

                    record.ImgSub = "";
                    for (var item = 0; item < Request.Form.Files.Count; item++)
                    {
                        string imgsub = "";
                        var timestamp = DateTime.Now.ToFileTime();
                        var file = Request.Form.Files[item];
                        imgsub = timestamp + "_" + Request.Form.Files[item].FileName;
                        string _PathSub = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", imgsub);
                        using (var stream = new FileStream(_PathSub, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        if (item == 0)
                        {
                            record.Photo = imgsub;
                        }
                        else
                        {
                            record.ImgSub += imgsub + ", ";
                        }
                    }
                }
               
                List<ItemCategory> list_categories = await _context.Categories.ToListAsync();
                List<ItemCategoriesProducts> list_category_product = await _context.CategoriesProducts.Where(item => item.ProductId == _id).ToListAsync();
                foreach (var item in list_category_product)
                {
                    _context.CategoriesProducts.Remove(item);
                }
           

                foreach (var itemCategory in list_categories)
                {
                    string formName = "category_" + itemCategory.Id;
                    if (!String.IsNullOrEmpty(Request.Form[formName]))
                    {
                        ItemCategoriesProducts recordCategoryProduct = new ItemCategoriesProducts();
                        recordCategoryProduct.CategoryId = itemCategory.Id;
                        recordCategoryProduct.ProductId = _id;
                        _context.CategoriesProducts.Add(recordCategoryProduct);
                      
                    }
                }

                List<ItemTagsProducts> list_tag_product = await _context.TagsProducts.Where(item => item.ProductId == _id).ToListAsync();
                foreach (var item_tag_product in list_tag_product)
                {
                    _context.TagsProducts.Remove(item_tag_product);
                }
               
                List<string> list_id_tags = Request.Form["tags"].ToList();
                foreach (var tag_id in list_id_tags)
                {
                    ItemTagsProducts record_tag_product = new ItemTagsProducts();
                    record_tag_product.TagId = Convert.ToInt32(tag_id);
                    record_tag_product.ProductId = _id;
                    _context.TagsProducts.Add(record_tag_product);
                  
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Admin/Home/ErrorPage");
            }
            
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
         
            if (Request.Form.Files.Count > 0)
            {

                for (var item = 0; item < Request.Form.Files.Count; item++)
                {
                    string imgsub = "";
                    var timestamp = DateTime.Now.ToFileTime();
                    var file = Request.Form.Files[item];
                    imgsub = timestamp + "_" + Request.Form.Files[item].FileName;
                    string _PathSub = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Products", imgsub);
                    using (var stream = new FileStream(_PathSub, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    if (item == 0)
                    {
                        record.Photo = imgsub;
                    }
                    else
                    {
                        record.ImgSub += imgsub + ", ";
                    }
                }
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
                var listImgSub = record.ImgSub.Split(", ");
                foreach(var item in listImgSub)
                {
                    if(item != "")
                    {
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", item));
                    }
                    
                }
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Products", record.Photo));
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Admin/Home/ErrorPage");
            }
           
           
        }
    }
}
