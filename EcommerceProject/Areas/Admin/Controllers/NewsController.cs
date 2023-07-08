using EcommerceProject.Models.Mapping;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceProject.DTO;
using System.Security.Cryptography;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Author")]
    public class NewsController : Controller
    {
        private readonly MyDbContext _context;
        public NewsController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("Index");
        }
        public async Task<List<InfoNews>> GetListRecord()
        {
           
            var news = await _context.News
                .OrderByDescending(item => item.Id)
                .Select(item => new InfoNews
                {
                    Id = item.Id,
                    Name = item.Name,
                    Hot = item.Hot,
                    Photo = item.Photo,
                    Categories = string.Join(", ", _context.ListArticle
                        .Where(cp => cp.Id == item.ListArticleId).Select(c => c.Name))
                })
                .ToListAsync();
            return news;
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.ListArticle = await _context.ListArticle.ToListAsync();
            ViewBag.action = "/Admin/News/CreatePost";
   
            return View("CreateUpdate");
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(IFormCollection fc)
        {

            int _idListArticle = fc["_idListArticle"].ToString().Trim() == "" ? 0 : Convert.ToInt32(fc["_idListArticle"].ToString().Trim());
            string _name = fc["name"].ToString().Trim();
            int _hot = fc["hot"] != "" && fc["hot"] == "on" ? 1 : 0;
            string _description = fc["description"].ToString().Trim();
            string _content = fc["content"].ToString().Trim();
            ItemNew record = new ItemNew();
            record.Name = _name;
            record.Hot = _hot;
            record.Description = _description;
            record.Content = _content;
            record.ListArticleId = _idListArticle;  
            string _FileName = "";
            if (Request.Form.Files.Count > 0)
            {

                var file = Request.Form.Files[0];
                var timestamp = DateTime.Now.ToFileTime();
                _FileName = timestamp + "_" + file.FileName;
                string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/News", _FileName);
                using (var stream = new FileStream(_Path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                record.Photo = _FileName;
            }
            _context.News.Add(record);
            await _context.SaveChangesAsync();
            return Redirect("/Admin/News");
        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.ListArticle = await _context.ListArticle.ToListAsync();
            int _id = id ?? 0;
            var record = await _context.News.FirstOrDefaultAsync(c => c.Id == _id);
            if (record != null)
            {
                ViewBag.action = "/Admin/News/UpdatesPost/" + _id;
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
            int _id = id ?? 0;
            int _idListArticle = fc["_idListArticle"].ToString().Trim() == "" ? 0 : Convert.ToInt32(fc["_idListArticle"].ToString().Trim());
            string _name = fc["name"].ToString().Trim();
            int _hot = fc["hot"] != "" && fc["hot"] == "on" ? 1 : 0;
            string _description = fc["description"].ToString().Trim();
            string _content = fc["content"].ToString().Trim();
            ItemNew record = await _context.News.Where(item => item.Id == _id).FirstOrDefaultAsync();
            if(record != null)
            {
                record.Name = _name;
                record.Hot = _hot;
                record.Description = _description;
                record.Content = _content;
                record.ListArticleId = _idListArticle;
                string _FileName = "";
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (record.Photo != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "News", record.Photo)))
                    {
                        string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/News", record.Photo);
                        System.IO.File.Delete(oldFilePath);
                    }
                    var timestamp = DateTime.Now.ToFileTime();
                    _FileName = timestamp + "_" + file.FileName;
                    string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/News", _FileName);
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    record.Photo = _FileName;
                }
            }
            await _context.SaveChangesAsync();
            return Redirect("/Admin/News");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            int _id = id ?? 0;
            var item = await _context.News.FirstOrDefaultAsync(c => c.Id == _id);
            if (item != null)
            {
                _context.News.Remove(item);
                await _context.SaveChangesAsync();
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "News", item.Photo));
            }
            else
            {
                return Redirect("/Admin/Home/ErrorPage");
            }
            return Redirect("/Admin/News");
        }
    }
}
