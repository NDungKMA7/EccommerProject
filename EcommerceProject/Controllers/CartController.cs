using EcommerceProject.DTO;
using EcommerceProject.Models;
using EcommerceProject.Models.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EcommerceProject.Controllers
{
    public class CartController : Controller
    {
        private readonly MyDbContext _context;
        public CartController(MyDbContext context)
        {
            _context = context;
        }
        public static T GetObjectFromJson<T>(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        public IActionResult Index()
        {
            List<ItemInCart> cart = GetObjectFromJson<List<ItemInCart>>(HttpContext.Session, "cart");
            if (cart != null)
            {
                ViewBag._total = cart.Sum(tbl => (tbl.ProductRecord.Price - (tbl.ProductRecord.Price * tbl.ProductRecord.Discount / 100)) * tbl.Quantity);
                return View(cart);           
            }
            return View();
        }
        public  async Task<IActionResult> Add(int  id)
        {
            if (GetObjectFromJson<List<ItemInCart>>(HttpContext.Session, "cart") == null)
            {
                List<ItemInCart> cart = new List<ItemInCart>();
                ItemProduct item =await _context.Products.Where(tbl => tbl.Id == id).FirstOrDefaultAsync();
                cart.Add(new ItemInCart { ProductRecord = item, Quantity = 1 });
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
            else
            {
                List<ItemInCart> cart = GetObjectFromJson<List<ItemInCart>>(HttpContext.Session, "cart");

                int index = isExist(HttpContext.Session, id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    ItemProduct item = await _context.Products.Where(tbl => tbl.Id == id).FirstOrDefaultAsync();
                    cart.Add(new ItemInCart { ProductRecord = item, Quantity = 1 });
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
            return RedirectToAction("Index");
        }
        public IActionResult Destroy()
        {
            List<ItemInCart> cart = new List<ItemInCart>();
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return Redirect("/Cart");
        }

        public IActionResult Remove(int id)
        {

            List<ItemInCart> cart = GetObjectFromJson<List<ItemInCart>>(HttpContext.Session, "cart");
            int index = isExist(HttpContext.Session, id);
            cart.RemoveAt(index);
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return Redirect("/Cart");
        }
        [HttpPost]
        public IActionResult Update()
         {

            List<ItemInCart> cart = GetObjectFromJson<List<ItemInCart>>(HttpContext.Session, "cart");
            foreach (var item in cart)
            {
                int quantity = Convert.ToInt32(Request.Form["product_" + item.ProductRecord.Id]);
                for (int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].ProductRecord.Id == item.ProductRecord.Id)
                    {
                        cart[i].Quantity = quantity;
                    }
                }
            }
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return Redirect("/Cart");
        }
        public async Task<IActionResult> Checkout()
        {
            bool isUserSignedIn = HttpContext.User.Identity.IsAuthenticated;
            if (isUserSignedIn)
            {
                List<ItemInCart> cart = GetObjectFromJson<List<ItemInCart>>(HttpContext.Session, "cart");
                string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                ItemOrder order = new ItemOrder();
                order.CustomerId = userId;
                order.Create = DateTime.Now;
                order.Price = cart.Sum(tbl => tbl.ProductRecord.Price * tbl.Quantity);
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                int order_id = order.Id;

                foreach (var item in cart)
                {
                    ItemOrderDetail _RecordOrdersDetail = new ItemOrderDetail();
                    _RecordOrdersDetail.OrderId = order_id;
                    _RecordOrdersDetail.ProductId = item.ProductRecord.Id;
                    _RecordOrdersDetail.Price = item.ProductRecord.Price - (item.ProductRecord.Price * item.ProductRecord.Discount) / 100;
                    _RecordOrdersDetail.Quantity = item.Quantity;
                  
                    _context.OrdersDetail.Add(_RecordOrdersDetail);
                  
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Destroy");
            }
            else
            {
                return Redirect("/Customers/Login");
            }
        }

        private static int isExist(ISession session, int id)
        {
            List<ItemInCart> cart = GetObjectFromJson<List<ItemInCart>>(session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].ProductRecord.Id == id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
