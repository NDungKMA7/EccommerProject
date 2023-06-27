using EcommerceProject.Models;
using EcommerceProject.Models.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography;
using X.PagedList;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Seller")]
    public class CategoriesController : Controller
    {
        private readonly MyDbContext _context;
        public CategoriesController(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int _RecordPerPage = 20;
            int _CurrentPage = page ?? 1;
            List<ItemCategory> _ListRecord = await _context.Categories.Where(x => x.ParentId == 0).OrderByDescending(item => item.Id).ToListAsync();
           ViewBag.ListCategories = await _context.Categories.ToListAsync();
            return View("Index", _ListRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
        
        public async Task<IActionResult> Create()
        {
            List<ItemCategory> _ListRecord = await _context.Categories.ToListAsync();
            ViewBag.listCategories = _ListRecord;
            ViewBag.action = "/Admin/Categories/CreatePost";
            return View("CreateUpdate");
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(IFormCollection fc)
        {
            string _name = fc["name"].ToString().Trim();
            int _parent_id = Convert.ToInt32(fc["parent_id"].ToString().Trim()) ;
            int _displayHomePage = fc["displayHomePage"] != "" && fc["displayHomePage"] == "on" ? 1 : 0;
            ItemCategory item = new ItemCategory();
            item.Name = _name;
            item.ParentId = _parent_id;
            item.DisplayHomePage = _displayHomePage;    
            _context.Categories.Add(item);
            await _context.SaveChangesAsync();
            return Redirect("/Admin/Categories");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            int _id = id ?? 0;
            var item = await _context.Categories.FirstOrDefaultAsync(c => c.Id == _id);   
            if (item != null)
            {
                 _context.Categories.Remove(item);
                await _context.SaveChangesAsync();  
            }
            return Redirect("/Admin/Categories");
        }
        public async Task<IActionResult> Update(int? id)
        {
            int _id = id ?? 0;
            List<ItemCategory> _ListRecord = await _context.Categories.ToListAsync();
            ViewBag.listCategories = _ListRecord;
            ItemCategory record = await _context.Categories.Where(c => c.Id == _id).FirstOrDefaultAsync(); 
            ViewBag.action = "/Admin/Categories/UpdatePost/" + _id;
            return View("CreateUpdate", record);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePost(int? id, IFormCollection fc)
        {
            int _id = id ?? 0;
            string _name = fc["name"].ToString().Trim();
            int _parent_id = Convert.ToInt32(fc["parent_id"].ToString().Trim());
            int _displayHomePage = fc["displayHomePage"] != "" && fc["displayHomePage"] == "on" ? 1 : 0;
            var item = await _context.Categories.FirstOrDefaultAsync(c => c.Id == _id);
            if (item != null)
            {
                item.DisplayHomePage = _displayHomePage;
                item.Name = _name;
                item.ParentId = _parent_id;
                await _context.SaveChangesAsync();  
            }
            return Redirect("/Admin/Categories");
        }
    }
}
