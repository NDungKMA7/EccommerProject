using EcommerceProject.Models.Mapping;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class AdvsController : Controller
    {
        private readonly MyDbContext _context;
        public AdvsController(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page)
        {
            int _RecordPerPage = 20;
            int _CurrentPage = page ?? 1;
            List<ItemAdv> _ListRecord = await _context.Adv.OrderByDescending(item => item.Id).ToListAsync();

            return View("Index", _ListRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
        public IActionResult Create()
        {

            ViewBag.action = "/Admin/Advs/CreatePost";
            return View("CreateUpdate");

        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            int _Position = Convert.ToInt32(fc["Position"].ToString().Trim());
            ItemAdv record = new ItemAdv();
            record.Name = _Name;
            record.Position = _Position;

            string _FileName = "";
            try
            {
                _FileName = Request.Form.Files[0].FileName;
            }
            catch {; }
            if (!String.IsNullOrEmpty(_FileName))
            {

                if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Adv", record.Photo)))
                {
                    System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Adv", record.Photo));
                }

                var timestap = DateTime.Now.ToFileTime();
                _FileName = timestap + "_" + _FileName;

                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Adv", _FileName);

                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }

                record.Photo = _FileName;
                _context.Adv.Add(record);
                await _context.SaveChangesAsync();
            }
            return Redirect("/Admin/Advs");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            int _id = id ?? 0;
            var item = await _context.Adv.FirstOrDefaultAsync(c => c.Id == _id);
            if (item != null)
            {
                _context.Adv.Remove(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy mục để xóa.";
            }
            return Redirect("/Admin/Advs");
        }
        public async Task<IActionResult> Update(int? id)
        {
            int _id = id ?? 0;
            var record = await _context.Adv.FirstOrDefaultAsync(c => c.Id == _id);
            if (record != null)
            {
                ViewBag.action = "/Admin/Advs/UpdatesPost/" + _id;
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
            int _Position = Convert.ToInt32(fc["Position"].ToString().Trim());
            int _id = id ?? 0;
            var record = await _context.Adv.FirstOrDefaultAsync(c => c.Id == _id);
            if (record != null)
            {
                record.Name = _Name;
                record.Position = _Position;

                string _FileName = "";
                try
                {
                    _FileName = Request.Form.Files[0].FileName;
                }
                catch {; }
                if (!String.IsNullOrEmpty(_FileName))
                {

                    if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Adv", record.Photo)))
                    {
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Adv", record.Photo));
                    }

                    var timestap = DateTime.Now.ToFileTime();
                    _FileName = timestap + "_" + _FileName;

                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Adv", _FileName);

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
            return Redirect("/Admin/Advs");
        }



    }
}
