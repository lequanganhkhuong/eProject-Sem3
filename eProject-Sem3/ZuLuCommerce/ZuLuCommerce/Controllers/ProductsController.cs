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
            if(rate <1 || rate > 5)
            {
                return Content("Invalid");
            }
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
        public ActionResult Index(int? page, string kw, int? cat, int? sup, string sort,int? topic,string price, string processor, string storage, string graphic, string cap)
        {
            
            int pageSize = 6;
            int pageNumber = page ?? 1;
            IQueryable<Product> p = db.Products.Where(x=>x.IsActive).OrderBy(x => x.Id);

            if (topic == null)
            {
                topic = 0;
            }
            if (topic != 0)
            {
                cat = 0;
                p = p.Where(x => x.Category.TopicId == topic);
                ViewBag.topicname = db.Topics.Where(x => x.Id == topic).FirstOrDefault().TopicName;
            }
            if (cat == null)
            {
                cat = 0;
            }
            if(cat != 0)
            {
                p = p.Where(x => x.CategoryId == cat);
                ViewBag.catname = db.Categories.Where(x => x.Id == cat).FirstOrDefault().Name;
            }
            if (sup == null)
            {
                sup = 0;
            }
            if (sup != 0)
            {
                p = p.Where(x => x.SupplierId == sup);
                ViewBag.supname = db.Suppliers.Where(x => x.Id == sup).FirstOrDefault().Name;
            }
            
            ViewBag.cat = cat;
            ViewBag.topic = topic;
            ViewBag.sup = sup;

            if (!string.IsNullOrEmpty(processor) && !processor.Equals("all"))
            {
                p = p.Where(x => x.ProductFeatures.Select(a => a.Processor).Contains(processor));
                ViewBag.processor = processor;
            }
            else
            {
                ViewBag.processor = "all";
            }
            if (!string.IsNullOrEmpty(storage) && !storage.Equals("all"))
            {
                p = p.Where(x => x.ProductFeatures.Select(a => a.StorageType).Contains(storage));
                ViewBag.storage = storage;
            }
            else
            {
                ViewBag.storage = "all";
            }
            if (!string.IsNullOrEmpty(graphic) && !graphic.Equals("all"))
            {
                p = p.Where(x => x.ProductFeatures.Select(a => a.Graphic).Contains(graphic));
                ViewBag.graphic = graphic;
            }
            else
            {
                ViewBag.graphic = "all";
            }
            if (!string.IsNullOrEmpty(cap) && !cap.Equals("all"))
            {
                p = p.Where(x => x.ProductFeatures.Select(a => a.StorageCap).Contains(cap));
                ViewBag.cap = cap;
            }
            else
            {
                ViewBag.cap = "all";
            }
            //search
            if (!string.IsNullOrEmpty(kw))
            {
                p = p.Where(x => x.Id.ToString().Equals(kw) || x.Name.ToLower().Contains(kw.ToLower()));
                ViewBag.kw = kw;
            }
            //price
            if (string.IsNullOrEmpty(price))
            {
                ViewBag.price = "all";
                price = "all";
            }
            else
            {
                ViewBag.price = price;
            }
            if (!price.Equals("all"))
            {
                switch (price)
                {
                    case "5-10":
                        p = p.Where(x => x.Price >= 5000000 && x.Price <= 10000000);
                        break;
                    case "0-5":
                        p = p.Where(x => x.Price <= 5000000 && x.Price >= 0);
                        break;
                    case "10-20":
                        p = p.Where(x => x.Price >= 10000000 && x.Price <= 20000000);
                        break;
                    case "20-40":
                        p = p.Where(x => x.Price >= 20000000 && x.Price <= 40000000);
                        break;
                    case "40":
                        p = p.Where(x => x.Price >= 40000000);
                        break;
                }
            }
            //sort
            if (string.IsNullOrEmpty(sort))
            {
                ViewBag.sort = "name_asc";
                sort = "name_asc";
            }
            else
            {
                ViewBag.sort = sort;
            }
            switch (sort)
            {
                case "name_asc":
                    p = p.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    p = p.OrderByDescending(x => x.Name);
                    break;
                case "price_asc":
                    p = p.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    p = p.OrderByDescending(x => x.Price);
                    break;
               
                case "bestselling":
                    var bs = db.OrderDetails.Where(x => x.Order.StatusId == 3).GroupBy(x => x.ProductId)
                   .Select(group => new
                   {
                       productid = group.Key,
                       Count = group.Count()
                   })
                   .OrderByDescending(x => x.Count);
                    p = from a in p
                        join b in bs on a.Id equals b.productid into c
                        from d in c.DefaultIfEmpty()
                        orderby d.Count descending
                        select a;

                    break;
            }
            ViewBag.resultcount = p.Count();
            return View(p.ToPagedList(pageNumber, pageSize));
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