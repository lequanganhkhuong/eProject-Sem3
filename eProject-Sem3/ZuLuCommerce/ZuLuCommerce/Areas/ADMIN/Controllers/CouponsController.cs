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
    [Authorize(Roles = "Admin,Manager")]
    public class CouponsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: ADMIN/Coupons
        public ActionResult Index(int? page,string isactive)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            IQueryable<Coupon> coupons = db.Coupons;
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
                        coupons =  coupons.Where(x=>x.IsActive);
                        break;
                    case "notactive":
                        coupons = coupons.Where(x => !x.IsActive);
                        break;
                }
            }
            
            return View(coupons.OrderBy(x => x.Id).ToPagedList(pageNumber,pageSize));
        }

        // GET: ADMIN/Coupons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // GET: ADMIN/Coupons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ADMIN/Coupons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Code,Discount,Uses,StartDate,EndDate,IsActive")] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                db.Coupons.Add(coupon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(coupon);
        }

        // GET: ADMIN/Coupons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: ADMIN/Coupons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Code,Discount,Uses,StartDate,EndDate,IsActive")] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coupon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coupon);
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
