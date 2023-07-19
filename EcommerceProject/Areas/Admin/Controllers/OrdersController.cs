using EcommerceProject.DTO;
using EcommerceProject.Models;
using EcommerceProject.Models.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Policy = "Seller")]
    public class OrdersController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MyDbContext _context;
        public OrdersController(MyDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
           

            return View();
        }
        public async Task<List<ItemOrderAdmin>> GetListRecord()
        {
            var listOrdersRecord = await _context.Orders
      .OrderByDescending(item => item.Id)
      .ToListAsync();

            var userIds = listOrdersRecord.Select(item => item.CustomerId).Distinct();
            var userDictionary = await _userManager.Users
                .Where(user => userIds.Contains(user.Id))
                .ToDictionaryAsync(user => user.Id);

            List<ItemOrderAdmin> listOrderInfo = listOrdersRecord.Select(itemOrder =>
            {
                var user = userDictionary[itemOrder.CustomerId];

                return new ItemOrderAdmin
                {
                    Id = itemOrder.Id,
                    Create = itemOrder.Create,
                    Status = itemOrder.Status,
                    Price = itemOrder.Price,
                    AddressUser = user.Address,
                    NameUser = user.Name,
                    Phone = Convert.ToInt32(user.PhoneNumber)
                };
            }).ToList();

            return listOrderInfo;
        }
        public async Task<IActionResult> Delivery(int? id)
        {
            int _OrderId = id ?? 0;
            ItemOrder record = await _context.Orders.Where(tbl => tbl.Id == _OrderId).FirstOrDefaultAsync();
            if (record != null)
            {
                record.Status = 1;
             
                await _context.SaveChangesAsync();
            }
            return Redirect("/Admin/Orders");
        }
        public IActionResult Detail(int? id)
        {
            int _OrderId = id ?? 0;
            ViewBag.OrderId = _OrderId;
            return View("Detail");
        }
        public async Task<List<ItemInfoDetailOrder>> GetDetailJson(int? id)
        {
         int _OrderId = id ?? 0;

            var _listDetailOrder = await _context.OrdersDetail
                .Where(tbl => tbl.OrderId == _OrderId)
                .ToListAsync();

            var productIds = _listDetailOrder.Select(order => order.ProductId).ToList();


            var productList = await _context.Products
                .Where(product => productIds.Contains(product.Id))
                .ToListAsync();

            var productDict = productList.ToDictionary(product => product.Id, product => product);

            var ListInfoDetailOrder = _listDetailOrder
                .Select(item => new ItemInfoDetailOrder
                {
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Name = productDict[item.ProductId].Name,
                    Photo = productDict[item.ProductId].Photo
                }).ToList();
            return ListInfoDetailOrder;
        }
       

    }
}
