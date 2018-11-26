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
    public class ShipmentsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: ADMIN/Shipments
        public ActionResult Index()
        {
            var shipments = db.Shipments.Include(s => s.ShipmentStatus).Include(s => s.Shipper);
            return View(shipments.ToList());
        }

        // GET: ADMIN/Shipments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // GET: ADMIN/Shipments/Create
        public ActionResult Create()
        {
            ViewBag.StatusId = new SelectList(db.ShipmentStatuses, "Id", "Name");
            ViewBag.ShipperId = new SelectList(db.Shippers, "Id", "FirstName");
            return View();
        }

        // POST: ADMIN/Shipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StatusId,ShippedDate,ShippingAddress,ShippingCity,ShippingFee,ShipperId")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.Shipments.Add(shipment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.ShipmentStatuses, "Id", "Name", shipment.StatusId);
            ViewBag.ShipperId = new SelectList(db.Shippers, "Id", "FirstName", shipment.ShipperId);
            return View(shipment);
        }

        // GET: ADMIN/Shipments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusId = new SelectList(db.ShipmentStatuses, "Id", "Name", shipment.StatusId);
            ViewBag.ShipperId = new SelectList(db.Shippers, "Id", "FirstName", shipment.ShipperId);
            return View(shipment);
        }

        // POST: ADMIN/Shipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StatusId,ShippedDate,ShippingAddress,ShippingCity,ShippingFee,ShipperId")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusId = new SelectList(db.ShipmentStatuses, "Id", "Name", shipment.StatusId);
            ViewBag.ShipperId = new SelectList(db.Shippers, "Id", "FirstName", shipment.ShipperId);
            return View(shipment);
        }

        // GET: ADMIN/Shipments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // POST: ADMIN/Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shipment shipment = db.Shipments.Find(id);
            db.Shipments.Remove(shipment);
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
