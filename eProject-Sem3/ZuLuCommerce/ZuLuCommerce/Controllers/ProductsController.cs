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
            var rate = int.Parse(str.Split('-')[0]);
            var productid = int.Parse((str.Split('-')[1]));
            if (!User.Identity.IsAuthenticated)
            {
                return Content("NotLogin");
            }
            try
            {
                int cur_acc = int.Parse(User.Identity.Name);
                var acc = db.Accounts.Find(cur_acc);
                if(acc == null)
                {
                    return Content("current user not exist");
                }
               
                var check = db.Ratings.Where(x => x.UserId == acc.Id && x.ProductId == productid).FirstOrDefault();
                if(check == null)
                {
                    Rating r = new Rating()
                    {
                        ProductId = productid,
                        UserId = acc.Id,
                        Rating1 = rate
                    };
                    db.Ratings.Add(r);
                    
                }
                else
                {
                    check.Rating1 = rate;
                }
                db.SaveChanges();
                return Content("OK");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }
        public ActionResult UserComment(string comment)
        {
            var content = comment.Split('-')[0];
            var productid = int.Parse((comment.Split('-')[1]));
            if (!User.Identity.IsAuthenticated)
            {
                return Content("NotLogin");
            }
            try
            {
                var cur_acc = int.Parse(User.Identity.Name);
                var acc = db.Accounts.Find(cur_acc);
                if (acc == null)
                {
                    return Content("current user not exist");
                }
                Comment c = new Comment()
                {
                    ProductId = productid,
                    UserId = acc.Id,
                    Content = content,
                    ParentId = null,
                    DateComment = DateTime.Now
                };
                db.Comments.Add(c);
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }
        public ActionResult UserReplyComment(string comment)
        {
            var content = comment.Split('-')[0];
            var productid = int.Parse((comment.Split('-')[1]));
            var parentid = int.Parse((comment.Split('-')[2]));
            if (!User.Identity.IsAuthenticated)
            {
                return Content("NotLogin");
            }
            try
            {
                var cur_acc = int.Parse(User.Identity.Name);
                var acc = db.Accounts.Find(cur_acc);
                if (acc == null)
                {
                    return Content("current user not exist");
                }
                Comment c = new Comment()
                {
                    ProductId = productid,
                    UserId = acc.Id,
                    Content = content,
                    ParentId = parentid,
                    DateComment = DateTime.Now
                };
                db.Comments.Add(c);
                db.SaveChanges();
                return Content("OK");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
        // GET: Products
        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;
            var products = db.Products.Where(x=>x.IsActive).OrderBy(x => x.Id);
            return View(products.ToPagedList(pageNumber, pageSize));
        }
        //GET:  Products/Category/id
        public ActionResult Categories(int id, int? page)
        {
            int pageSize = 9;
            int pageNumber = page ?? 1;
            var products = db.Products.Where(x => x.CategoryId == id && x.IsActive).OrderBy(x => x.Id);

            return View(products.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Topics(int id)
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
            if (!product.IsActive)
            {
                return HttpNotFound();
            }
            ViewBag.features = db.ProductFeatures.Where(x => x.ProductId == id).SingleOrDefault();
            return View(product);
        }
    }
}