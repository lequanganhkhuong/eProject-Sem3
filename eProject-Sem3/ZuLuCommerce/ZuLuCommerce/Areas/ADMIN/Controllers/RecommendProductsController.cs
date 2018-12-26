using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZuLuCommerce.Models;

namespace ZuLuCommerce.Areas.ADMIN.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RecommendProductsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        public ActionResult RemoveProduct(int id)
        {
            try
            {
                var p = db.RecommendProducts.Find(id);
                db.RecommendProducts.Remove(p);
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

                RecommendProduct rp = new RecommendProduct()
                {
                    ProductId = id
                };
                db.RecommendProducts.Add(rp);
                db.SaveChanges();
                return Content("OK");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
        // GET: ADMIN/RecommendProducts
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 6;
            var RecommendProducts = db.RecommendProducts.Include(b => b.Product).OrderBy(x => x.Id);
            ViewBag.resultcount = RecommendProducts.Count();
            return View(RecommendProducts.ToPagedList(pageNumber, pageSize));
        }


        // GET: ADMIN/RecommendProducts/AddProducts
        public ActionResult AddProducts(int? page, string kw, string sort, string supplier, string category, string price, string discount, string isactive)
        {

            int pageNumber = page ?? 1;
            int pageSize = 6;
            //select all
            var p = db.Products.OrderBy(x => x.Id).Include(c => c.Category).Include(d => d.Supplier);



            //filter
            //supplier filter
            if (string.IsNullOrEmpty(supplier))
            {
                ViewBag.supplier = "allsup";
                supplier = "allsup";
            }
            else
            {
                ViewBag.supplier = supplier;
            }
            if (supplier.Equals("allsup"))
            {
                p = db.Products.OrderBy(x => x.Id).Include(c => c.Category).Include(d => d.Supplier);

            }
            else
            {
                sort = "id_asc";
                p = db.Products.Where(x => x.Supplier.Name == supplier);

            }
            //category filter
            if (string.IsNullOrEmpty(category))
            {
                ViewBag.category = "allcat";
                category = "allcat";
            }
            else
            {
                ViewBag.category = category;
            }
            if (category.Equals("allcat"))
            {
                if (supplier.Equals("allsup"))
                {
                    p = db.Products.OrderBy(x => x.Id).Include(c => c.Category).Include(d => d.Supplier);
                }
                else
                {
                    sort = "id_asc";
                    p = db.Products.Where(x => x.Supplier.Name == supplier);
                }
            }
            else
            {
                if (supplier.Equals("allsup"))
                {
                    p = db.Products.Where(x => x.Category.Name == category);
                }
                else
                {
                    p = p.Where(x => x.Category.Name == category);

                }

            }
            //filter isactive
            if (string.IsNullOrEmpty(isactive))
            {
                ViewBag.isactive = "none";
                isactive = "none";
            }
            else
            {
                ViewBag.isactive = isactive;
            }
            if (!isactive.Equals("none"))
            {
                sort = "id_asc";
                switch (isactive)
                {
                    case "active":
                        p = p.Where(x => x.IsActive);
                        break;
                    case "notactive":
                        p = p.Where(x => !x.IsActive);
                        break;
                }
            }
            else
            {
                if (category.Equals("allcat"))
                {
                    if (supplier.Equals("allsup"))
                    {
                        p = db.Products.OrderBy(x => x.Id).Include(c => c.Category).Include(d => d.Supplier);
                    }
                    else
                    {
                        sort = "id_asc";
                        p = db.Products.Where(x => x.Supplier.Name == supplier);
                    }
                }
                else
                {
                    if (supplier.Equals("allsup"))
                    {
                        p = db.Products.Where(x => x.Category.Name == category);
                    }
                    else
                    {
                        p = p.Where(x => x.Category.Name == category);

                    }

                }
            }

            //filter price
            if (string.IsNullOrEmpty(price))
            {
                ViewBag.price = "allprice";
                price = "allprice";
            }
            else
            {
                ViewBag.price = price;
            }
            switch (price)
            {
                case "allprice":
                    p = p.Where(x => x.Price > 0);
                    break;
                case "0-1mil":
                    p = p.Where(x => x.Price > 0 && x.Price < 1000000);
                    break;
                case "1-5mil":
                    p = p.Where(x => x.Price > 1000000 && x.Price < 5000000);
                    break;
                case "5-10mil":
                    p = p.Where(x => x.Price > 5000000 && x.Price < 10000000);
                    break;
                case "10-20mil":
                    p = p.Where(x => x.Price > 10000000 && x.Price < 20000000);
                    break;
                case "20milup":
                    p = p.Where(x => x.Price > 20000000);
                    break;
            }

            //filter discount
            if (string.IsNullOrEmpty(discount))
            {
                ViewBag.discount = "none";
                discount = "none";
            }
            else
            {
                ViewBag.discount = discount;
            }
            switch (discount)
            {
                case "none":
                    p = p.Where(x => x.Discount >= 0);
                    break;
                case "0-10":
                    p = p.Where(x => x.Discount >= 0 && x.Discount <= 10);
                    break;
                case "10-20":
                    p = p.Where(x => x.Discount >= 10 && x.Discount < 20);
                    break;

                case "20up":
                    p = p.Where(x => x.Discount > 20);
                    break;
            }

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
                    break;
                case "id_desc":
                    p = p.OrderByDescending(x => x.Id);
                    break;
                case "name_asc":
                    p = p.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    p = p.OrderByDescending(x => x.Name);
                    break;
                case "category_asc":
                    p = p.OrderBy(x => x.Category.Name);
                    break;
                case "category_desc":
                    p = p.OrderByDescending(x => x.Category.Name);
                    break;
            }

            p = from c in p
                where !db.RecommendProducts.Select(x => x.ProductId).Contains(c.Id)
                select c;
            p = p.Where(x => x.IsActive);
            ViewBag.resultcount = p.Count();
            return View(p.ToPagedList(pageNumber, pageSize));
        }

        // POST: ADMIN/RecommendProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProducts([Bind(Include = "Id,ProductId")] RecommendProduct RecommendProduct)
        {
            if (ModelState.IsValid)
            {
                db.RecommendProducts.Add(RecommendProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", RecommendProduct.ProductId);
            return View(RecommendProduct);
        }
    }
}
