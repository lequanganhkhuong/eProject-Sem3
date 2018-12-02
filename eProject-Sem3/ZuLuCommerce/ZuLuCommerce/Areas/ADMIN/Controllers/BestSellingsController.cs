using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZuLuCommerce.Models;
using PagedList;
namespace ZuLuCommerce.Areas.ADMIN.Controllers
{
    public class BestSellingsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();
        public ActionResult RemoveProduct(int id)
        {
            try
            {
                var p = db.BestSellings.Find(id);
                db.BestSellings.Remove(p);
                db.SaveChanges();
                return Content("OK");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }
        public ActionResult AddProduct(int id)
        {
            try
            {
       
                BestSelling bs = new BestSelling()
                {
                    ProductId = id
                };
                db.BestSellings.Add(bs);
                db.SaveChanges();
                return Content("OK");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
        // GET: ADMIN/BestSellings
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 6;
            var bestSellings = db.BestSellings.Include(b => b.Product).OrderBy(x => x.Id);

            return View(bestSellings.ToPagedList(pageNumber, pageSize));
        }


        // GET: ADMIN/BestSellings/AddProducts
        public ActionResult AddProducts(int? page, string kw, string sort)
        {
            int pn = page ?? 1;
            int ps = 6;
            //select all
            var p = db.Products.OrderBy(x => x.Id).Include(c => c.Category).Include(d => d.Supplier);
            p = from c in p
                    where !(from o in db.BestSellings select o.ProductId).Contains(c.Id) select c ;
            //search
            if (!string.IsNullOrEmpty(kw))
            {
                p = p.Where(x => x.Id.ToString().Equals(kw) || x.Name.ToLower().Contains(kw.ToLower()));
                ViewBag.kw = kw;
            }
            //sort

            //ViewBag.sortTopic = "topic_asc";
            if (string.IsNullOrEmpty(sort))
            {
                ViewBag.sort = "id_asc";

            }
            else
            {
                ViewBag.sort = sort;
            }
            switch (sort)
            {
                case "id_asc":
                    p = p.OrderBy(x => x.Id);
                    ViewBag.sortId = "id_desc";
                    break;
                case "id_desc":
                    p = p.OrderByDescending(x => x.Id);
                    ViewBag.sortId = "id_asc";
                    break;
                case "name_asc":
                    p = p.OrderBy(x => x.Name);
                    ViewBag.sortName = "name_desc";
                    break;
                case "name_desc":
                    p = p.OrderByDescending(x => x.Name);
                    ViewBag.sortName = "name_asc";
                    break;
                case "category_asc":
                    p = p.OrderBy(x => x.Category.Name);
                    ViewBag.sortTopic = "category_desc";
                    break;
                case "category_desc":
                    p = p.OrderByDescending(x => x.Category.Name);
                    ViewBag.sortTopic = "category_asc";
                    break;
            }
            ViewBag.sortId = sort ?? "id_desc";
            ViewBag.sortName = sort ?? "name_desc";
            ViewBag.sortTopic = sort ?? "topic_desc";

            return View(p.ToPagedList(pn, ps));

            //return View();
        }

        // POST: ADMIN/BestSellings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProducts([Bind(Include = "Id,ProductId")] BestSelling bestSelling)
        {
            if (ModelState.IsValid)
            {
                db.BestSellings.Add(bestSelling);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", bestSelling.ProductId);
            return View(bestSelling);
        }
        
    }
}
