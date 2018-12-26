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
    public class ShippersController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        [Authorize(Roles ="Admin,Manager")]
        // GET: ADMIN/Shippers
        public ActionResult Index()
        {
            var shippers = db.Shippers.Include(s => s.ShipmentStatus);
            return View(shippers.ToList());
        }
        [Authorize(Roles = "Admin,Manager")]
        // GET: ADMIN/Shippers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipper shipper = db.Shippers.Find(id);
            if (shipper == null)
            {
                return HttpNotFound();
            }
            return View(shipper);
        }
        [Authorize(Roles = "Admin,Manager")]
        // GET: ADMIN/Shippers/Create
        public ActionResult Create()
        {
            ViewBag.StatusId = new SelectList(db.ShipmentStatuses, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "Admin,Manager")]
        // POST: ADMIN/Shippers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Phone,Address,Email,Birthday,StatusId,IsActive")] Shipper shipper)
        {
            if (ModelState.IsValid)
            {
                db.Shippers.Add(shipper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.ShipmentStatuses, "Id", "Name", shipper.StatusId);
            return View(shipper);
        }
        [Authorize(Roles = "Admin,Manager")]
        // GET: ADMIN/Shippers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipper shipper = db.Shippers.Find(id);
            if (shipper == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusId = new SelectList(db.ShipmentStatuses, "Id", "Name", shipper.StatusId);
            return View(shipper);
        }
        [Authorize(Roles = "Admin,Manager")]
        // POST: ADMIN/Shippers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Phone,Address,Email,Birthday,StatusId,IsActive")] Shipper shipper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusId = new SelectList(db.ShipmentStatuses, "Id", "Name", shipper.StatusId);
            return View(shipper);
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
