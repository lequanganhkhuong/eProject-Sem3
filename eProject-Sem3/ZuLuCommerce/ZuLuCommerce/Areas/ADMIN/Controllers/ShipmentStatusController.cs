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
    public class ShipmentStatusController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: ADMIN/ShipmentStatus
        public ActionResult Index()
        {
            return View(db.ShipmentStatuses.ToList());
        }

        // GET: ADMIN/ShipmentStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentStatus shipmentStatus = db.ShipmentStatuses.Find(id);
            if (shipmentStatus == null)
            {
                return HttpNotFound();
            }
            return View(shipmentStatus);
        }

        // GET: ADMIN/ShipmentStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ADMIN/ShipmentStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ShipmentStatus shipmentStatus)
        {
            if (ModelState.IsValid)
            {
                db.ShipmentStatuses.Add(shipmentStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shipmentStatus);
        }

        // GET: ADMIN/ShipmentStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentStatus shipmentStatus = db.ShipmentStatuses.Find(id);
            if (shipmentStatus == null)
            {
                return HttpNotFound();
            }
            return View(shipmentStatus);
        }

        // POST: ADMIN/ShipmentStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ShipmentStatus shipmentStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipmentStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shipmentStatus);
        }

        // GET: ADMIN/ShipmentStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentStatus shipmentStatus = db.ShipmentStatuses.Find(id);
            if (shipmentStatus == null)
            {
                return HttpNotFound();
            }
            return View(shipmentStatus);
        }

        // POST: ADMIN/ShipmentStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShipmentStatus shipmentStatus = db.ShipmentStatuses.Find(id);
            db.ShipmentStatuses.Remove(shipmentStatus);
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
