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
    public class TagsController : Controller
    {
        private readonly MyDbContext _context;
        public TagsController(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int _RecordPerPage = 20;
            int _CurrentPage = page ?? 1;
            List<ItemTag> _ListRecord = await _context.Tags.OrderByDescending(item => item.Id).ToListAsync();
            return View("Index", _ListRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }

        public  IActionResult Create()
        {
            ViewBag.action = "/Admin/Tags/CreatePost";
            return View("CreateUpdate");
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(IFormCollection fc)
        {
            string _name = fc["name"].ToString().Trim();
            ItemTag item = new ItemTag();
            item.Name = _name;
            _context.Tags.Add(item);
            await _context.SaveChangesAsync();
            return Redirect("/Admin/Tags");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            int _id = id ?? 0;
            var item = await _context.Tags.FirstOrDefaultAsync(c => c.Id == _id);
            if (item != null)
            {
                _context.Tags.Remove(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy mục để xóa.";
            }
            return Redirect("/Admin/Tags");
        }
        public async Task<IActionResult> Update(int? id)
        {
            int _id = id ?? 0;
            ItemTag record = await _context.Tags.Where(c => c.Id == _id).FirstOrDefaultAsync();
            if(record != null) {
                ViewBag.action = "/Admin/Tags/UpdatePost/" + _id;
                return View("CreateUpdate", record);
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy mục để cập nhập";
            }
           return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePost(int? id, IFormCollection fc)
        {
            int _id = id ?? 0;
            string _name = fc["name"].ToString().Trim();
           
            var item = await _context.Tags.FirstOrDefaultAsync(c => c.Id == _id);
            if (item != null)
            {
      
                item.Name = _name;
 
                await _context.SaveChangesAsync();
            }
            return Redirect("/Admin/Tags");
        }
    }
}
