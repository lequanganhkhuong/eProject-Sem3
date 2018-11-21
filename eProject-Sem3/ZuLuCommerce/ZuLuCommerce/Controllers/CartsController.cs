using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZuLuCommerce.Models;
namespace ZuLuCommerce.Controllers
{
    public class CartsController : Controller
    {
        eCommerceEntities db = new eCommerceEntities();

        //public ActionResult AddToCart(int id,int qty)
        //{
        //    try
        //    {
        //        var item = db.Products.Where(x => x.Id == id).FirstOrDefault();

        //        return Content("OK");
        //    }
        //    catch (Exception)
        //    {

        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
            
        //}
        // GET: Carts
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(string data)
        {
            var cart_items = JsonConvert.DeserializeObject<List<LocalCartVM>>(data);
            if (cart_items == null || cart_items.Count == 0)
            {
                return Content("Bay roi");
            }
            Order o = new Order()
            {
                CreatedDate = DateTime.Now,
                StatusId = 1,
                Tax = 0
            };
            db.Orders.Add(o);
            db.SaveChanges();
            foreach (var item in cart_items)
            {
                OrderDetail od = new OrderDetail
                {
                    OrderId = o.Id,
                    ProductId = item.productid,
                    ProductName = item.productname,
                    Quantity = item.quantity,
                    Price = db.Products.Where(x => x.Id == item.productid).FirstOrDefault().Price
                };
                db.OrderDetails.Add(od);
            }
            db.SaveChanges();
            return Content("OK");
        }
    }
}