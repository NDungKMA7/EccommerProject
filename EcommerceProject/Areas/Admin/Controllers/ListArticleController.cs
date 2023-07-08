using EcommerceProject.Models.Mapping;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Author")]
    public class ListArticleController : Controller
    {
        private readonly MyDbContext _context;
        public ListArticleController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("Index");
        }
        public async Task<List<ItemListArticle>> GetListRecord()
        {
            return await _context.ListArticle.OrderByDescending(item => item.Id).ToListAsync();
        }

        public IActionResult Create()
        {
            ViewBag.action = "/Admin/ListArticle/CreatePost";
            return View("CreateUpdate");
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(IFormCollection fc)
        {
            string _name = fc["name"].ToString().Trim();
            ItemListArticle item = new ItemListArticle();
            item.Name = _name;
            _context.ListArticle.Add(item);
            await _context.SaveChangesAsync();
            return Redirect("/Admin/ListArticle");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            int _id = id ?? 0;
            var item = await _context.ListArticle.FirstOrDefaultAsync(c => c.Id == _id);
            if (item != null)
            {
                _context.ListArticle.Remove(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                return Redirect("/Admin/Home/ErrorPage");
            }
            return Redirect("/Admin/ListArticle");
        }
        public async Task<IActionResult> Update(int? id)
        {
            int _id = id ?? 0;
            ItemListArticle record = await _context.ListArticle.Where(c => c.Id == _id).FirstOrDefaultAsync();
            if (record != null)
            {
                ViewBag.action = "/Admin/ListArticle/UpdatePost/" + _id;
                return View("CreateUpdate", record);
            }
            else
            {
                return Redirect("/Admin/Home/ErrorPage");
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdatePost(int? id, IFormCollection fc)
        {
            int _id = id ?? 0;
            string _name = fc["name"].ToString().Trim();
         
            var item = await _context.ListArticle.FirstOrDefaultAsync(c => c.Id == _id);
            if (item != null)
            {

                item.Name = _name;
                await _context.SaveChangesAsync();
                return Redirect("/Admin/ListArticle");
            }
            else
            {
                return Redirect("/Admin/Home/ErrorPage");
            }

        }
    }
}
