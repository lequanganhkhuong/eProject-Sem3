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
    [Authorize(Roles ="Admin,Manager")]
    public class SpecialDealsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: ADMIN/SpecialDeals
        public ActionResult Index(int? page, string isactive)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;

            IQueryable<SpecialDeal> specialDeals = db.SpecialDeals.Include(s => s.Product).OrderBy(x => x.Id);
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

                switch (isactive)
                {
                    case "active":
                        specialDeals = specialDeals.Where(x => x.IsActive);
                        break;
                    case "notactive":
                        specialDeals = specialDeals.Where(x => !x.IsActive);
                        break;
                }
            }
           
            return View(specialDeals.ToPagedList(pageNumber,pageSize));
        }

        // GET: ADMIN/SpecialDeals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialDeal specialDeal = db.SpecialDeals.Find(id);
            if (specialDeal == null)
            {
                return HttpNotFound();
            }
            return View(specialDeal);
        }

        // GET: ADMIN/SpecialDeals/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: ADMIN/SpecialDeals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,StartDate,EndDate,Discount,IsActive")] SpecialDeal specialDeal)
        {
            if (ModelState.IsValid)
            {
                db.SpecialDeals.Add(specialDeal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", specialDeal.ProductId);
            return View(specialDeal);
        }

        // GET: ADMIN/SpecialDeals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpecialDeal specialDeal = db.SpecialDeals.Find(id);
            if (specialDeal == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", specialDeal.ProductId);
            return View(specialDeal);
        }

        // POST: ADMIN/SpecialDeals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,StartDate,EndDate,Discount,IsActive")] SpecialDeal specialDeal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specialDeal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", specialDeal.ProductId);
            return View(specialDeal);
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
