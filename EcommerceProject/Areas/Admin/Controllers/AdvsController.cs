using EcommerceProject.Models.Mapping;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View("Index");
        }
        public async Task<List<ItemAdv>> GetListRecord()
        {
            return await _context.Adv.OrderByDescending(item => item.Id).ToListAsync();
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
            string _Link = fc["linkAdv"].ToString().Trim();
            ItemAdv record = new ItemAdv();
            record.Name = _Name;
            record.Position = _Position;
            record.Link = _Link;
            string _FileName = "";
            if (Request.Form.Files.Count > 0)
            {

                var file = Request.Form.Files[0];
                var timestamp = DateTime.Now.ToFileTime();
                _FileName = timestamp + "_" + file.FileName;
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Adv", _FileName);
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                record.Photo = _FileName;
            }
            _context.Adv.Add(record);
            await _context.SaveChangesAsync();
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
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Adv", item.Photo));
            }
            else
            {
                return Redirect("/Admin/Home/ErrorPage");
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
                return Redirect("/Admin/Home/ErrorPage");
            }
        }



        [HttpPost]
        public async Task<IActionResult> UpdatesPost(IFormCollection fc, int? id)
        {
            string _Name = fc["Name"].ToString().Trim();
            int _Position = Convert.ToInt32(fc["Position"].ToString().Trim());
            string _Link = fc["linkAdv"].ToString().Trim();
            int _id = id ?? 0;
            var record = await _context.Adv.FirstOrDefaultAsync(c => c.Id == _id);
            if (record != null)
            {
                record.Name = _Name;
                record.Position = _Position;
                record.Link = _Link;
                string _FileName = "";
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Adv", record.Photo)))
                    {
                        string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Adv", record.Photo);
                        System.IO.File.Delete(oldFilePath);
                    }
                    var timestamp = DateTime.Now.ToFileTime();
                    _FileName = timestamp + "_" + file.FileName;
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Adv", _FileName);
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    record.Photo = _FileName;
                }
               
                await _context.SaveChangesAsync();
            }
            else
            {
                return Redirect("/Admin/Home/ErrorPage");
            }
            return Redirect("/Admin/Advs");
        }



    }
}
