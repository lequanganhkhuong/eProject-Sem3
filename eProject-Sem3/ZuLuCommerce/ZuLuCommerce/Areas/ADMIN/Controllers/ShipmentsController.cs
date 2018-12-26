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
    [Authorize(Roles ="Admin,Manager,Sale")]
    public class ShipmentsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: ADMIN/Shipments
        public ActionResult Index(int? page, string kw)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            var shipments = db.Shipments.OrderBy(x=>x.Id).Include(s => s.ShipmentStatus).Include(s => s.Shipper);
            if (!string.IsNullOrEmpty(kw))
            {
                int id = int.Parse(kw);
                shipments = shipments.Where(x => x.Id == id);
            }
            return View(shipments.ToPagedList(pageNumber, pageSize));
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
            var o = db.Orders.Where(x => x.ShipmentId == shipment.Id).SingleOrDefault();
            ViewBag.order = o.Codenname;
            ViewBag.orderid = o.Id;
            return View(shipment);
        }

        [Authorize(Roles ="Admin,Manager")]
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
            if(shipment.StatusId == 3 || shipment.StatusId == 4)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
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
