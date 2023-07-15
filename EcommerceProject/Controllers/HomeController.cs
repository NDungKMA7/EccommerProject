using EcommerceProject.DTO;
using EcommerceProject.Models;
using EcommerceProject.Models.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _context;
        public HomeController(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.SlidesList = await _context.Slides.OrderByDescending(item => item.Id).ToListAsync();
            List<ProductHomeDTO> _ProductsList = await _context.Products
              .OrderByDescending(item => item.Id)
              .Select(item => new ProductHomeDTO
              {
                  Name = item.Name,
                  Price = item.Price,
                  Discount = item.Discount,
                  Id = item.Id,
                  Photo = item.Photo,
                  Hot = item.Hot
              })
              .ToListAsync();
            ViewBag.ProductsList = _ProductsList;

                 List<NewHomeDTO> _NewList = await _context.News
              .OrderByDescending(item => item.Id).Where(item => item.Hot == 1)
              .Select(item => new NewHomeDTO
              {
                  Name = item.Name,
                  Id = item.Id,
                  Photo = item.Photo,
                  Hot = item.Hot,
                  Description = item.Description
              })
              .ToListAsync();
            ViewBag.NewList = _NewList;

            List<ItemAdv> _AdvHomeList = await _context.Adv.ToListAsync();
            ViewBag.AdvList = _AdvHomeList;

            return View("Index");
        }

        public IActionResult Error404()
        {
            return View("Error404");
        }
    }
}