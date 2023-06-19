using EcommerceProject.Models;
using EcommerceProject.Models.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class SlidesController : Controller
    {
        private readonly MyDbContext _context;
        public SlidesController(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page)
        {
            int _RecordPerPage = 20;
            int _CurrentPage = page ?? 1;
            List<ItemSlide> _ListRecord = await _context.Slides.OrderByDescending(item => item.Id).ToListAsync();

            return View("Index", _ListRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }

        public IActionResult Create()
        {

            ViewBag.action = "/Admin/Slides/CreatePost";
            return View("CreateUpdate");
           
        }
      
        [HttpPost]
        public async Task<IActionResult> CreatePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Title = fc["Title"].ToString().Trim();
            string _SubTitle = fc["SubTitle"].ToString().Trim();
            string _Info = fc["Info"].ToString().Trim();
            string _Link = fc["Link"].ToString().Trim();
            ItemSlide record = new ItemSlide();
            record.Name = _Name;
            record.Title = _Title;
            record.SubTitle = _SubTitle;
            record.Info = _Info;
            record.Link = _Link;

            string _FileName = "";
            try
            {
                _FileName = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!String.IsNullOrEmpty(_FileName))
            {

                if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Slides", record.Photo)))
                {
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Slides", record.Photo));
                }

                var timestap = DateTime.Now.ToFileTime();
                _FileName = timestap + "_" + _FileName;

                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Slides", _FileName);

                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }

                record.Photo = _FileName;
                _context.Slides.Add(record);
                await _context.SaveChangesAsync();
            }
            return Redirect("/Admin/Slides");
        }
        

        public async Task<IActionResult> Delete(int? id)
        {
            int _id = id ?? 0;
            var item = await _context.Slides.FirstOrDefaultAsync(c => c.Id == _id);
            if (item != null)
            {
                _context.Slides.Remove(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy mục để xóa.";
            }
            return Redirect("/Admin/Slides");
        }
        public async Task<IActionResult> Update (int? id)
        {
            int _id = id ?? 0;
            var record = await _context.Slides.FirstOrDefaultAsync(c => c.Id == _id);
            if (record != null)
            {
                ViewBag.action = "/Admin/Slides/UpdatesPost/" + _id;
                return View("CreateUpdate", record);
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy mục để cập nhập";
            }
            return Redirect("/Admin/Slides");
        }
           
           
        
        [HttpPost]
        public async Task<IActionResult> UpdatesPost(IFormCollection fc, int? id)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Title = fc["Title"].ToString().Trim();
            string _SubTitle = fc["SubTitle"].ToString().Trim();
            string _Info = fc["Info"].ToString().Trim();
            string _Link = fc["Link"].ToString().Trim();
            int _id = id ?? 0;
            var record = await _context.Slides.FirstOrDefaultAsync(c => c.Id == _id);
            if(record != null)
            {
                record.Name = _Name;
                record.Title = _Title;
                record.SubTitle = _SubTitle;
                record.Info = _Info;
                record.Link = _Link;

                string _FileName = "";
                try
                {
                    _FileName = Request.Form.Files[0].FileName;
                }
                catch {; }
                if (!String.IsNullOrEmpty(_FileName))
                {
                  
                    if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Slides", record.Photo)))
                    {
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Slides", record.Photo));
                    }
                  
                    var timestap = DateTime.Now.ToFileTime();
                    _FileName = timestap + "_" + _FileName;
                    
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Slides", _FileName);
                    
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream);
                    }
                  
                    record.Photo = _FileName;
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy mục để cập nhập";
            }
            return Redirect("/Admin/Slides");
        }
    
    }
}
