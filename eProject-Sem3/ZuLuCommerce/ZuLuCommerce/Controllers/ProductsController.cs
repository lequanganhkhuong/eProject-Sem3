using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ZuLuCommerce.Models;
using System.Net;

namespace ZuLuCommerce.Controllers
{
    public class ProductsController : Controller
    {
        eCommerceEntities db = new eCommerceEntities();
        public ActionResult UserRating(string str)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content("NotLogin");
            }
            try
            {
                int cur_acc = int.Parse(User.Identity.Name);
                var acc = db.Accounts.Find(cur_acc);
                var cus = db.Customers.Where(x => x.Id == acc.CustomerId).FirstOrDefault();
                var rate = int.Parse(str.Split('-')[0]);
                var productid = int.Parse((str.Split('-')[1]));
                var check = db.Ratings.Where(x => x.Id == cur_acc && x.ProductId == productid).FirstOrDefault();
                if(check == null)
                {
                    Rating r = new Rating()
                    {
                        ProductId = productid,
                        UserId = cus.Id,
                        Rating1 = rate
                    };
                    db.Ratings.Add(r);
                    
                }
                else
                {
                    check.Rating1 = rate;
                }

            }
            catch (Exception ex)
            {
                
            }
            db.SaveChanges();
            return Content("OK");
        }
        // GET: Products
        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;
            var products = db.Products.OrderBy(x => x.Id);
            return View(products.ToPagedList(pageNumber, pageSize));
        }
        //GET:  Products/Category/id
        public ActionResult Category(int id)
        {
            var product = db.Products.Where(x => x.CategoryId == id);
            return View(product.ToList());
        }
        public ActionResult Search(string kw)
        {

            return View();
        }
        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
    }
}