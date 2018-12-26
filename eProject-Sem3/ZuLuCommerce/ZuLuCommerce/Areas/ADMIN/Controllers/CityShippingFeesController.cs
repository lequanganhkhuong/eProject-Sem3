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
    public class CityShippingFeesController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: ADMIN/CityShippingFees
        public ActionResult Index()
        {
            return View(db.CityShippingFees.ToList());
        }

        // GET: ADMIN/CityShippingFees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityShippingFee cityShippingFee = db.CityShippingFees.Find(id);
            if (cityShippingFee == null)
            {
                return HttpNotFound();
            }
            return View(cityShippingFee);
        }

        // GET: ADMIN/CityShippingFees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ADMIN/CityShippingFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CityName,ShippingFee")] CityShippingFee cityShippingFee)
        {
            if (ModelState.IsValid)
            {
                db.CityShippingFees.Add(cityShippingFee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cityShippingFee);
        }

        // GET: ADMIN/CityShippingFees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityShippingFee cityShippingFee = db.CityShippingFees.Find(id);
            if (cityShippingFee == null)
            {
                return HttpNotFound();
            }
            return View(cityShippingFee);
        }

        // POST: ADMIN/CityShippingFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CityName,ShippingFee")] CityShippingFee cityShippingFee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cityShippingFee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cityShippingFee);
        }

        // GET: ADMIN/CityShippingFees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityShippingFee cityShippingFee = db.CityShippingFees.Find(id);
            if (cityShippingFee == null)
            {
                return HttpNotFound();
            }
            return View(cityShippingFee);
        }

        // POST: ADMIN/CityShippingFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CityShippingFee cityShippingFee = db.CityShippingFees.Find(id);
            db.CityShippingFees.Remove(cityShippingFee);
            db.SaveChanges();
            return RedirectToAction("Index");
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
