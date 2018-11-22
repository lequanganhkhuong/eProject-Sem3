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
    public class OrdersController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: ADMIN/Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.OrderStatus).Include(o => o.Shipment);
            return View(orders.ToList());
        }

        // GET: ADMIN/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: ADMIN/Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName");
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName");
            ViewBag.StatusId = new SelectList(db.OrderStatuses, "Id", "Name");
            ViewBag.ShipmentId = new SelectList(db.Shipments, "Id", "ShippingAddress");
            return View();
        }

        // POST: ADMIN/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreatedDate,StatusId,Description,PaymentType,CustomerId,EmployeeId,ShipmentId,Discount,Tax")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", order.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", order.EmployeeId);
            ViewBag.StatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.StatusId);
            ViewBag.ShipmentId = new SelectList(db.Shipments, "Id", "ShippingAddress", order.ShipmentId);
            return View(order);
        }

        // GET: ADMIN/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", order.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", order.EmployeeId);
            ViewBag.StatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.StatusId);
            ViewBag.ShipmentId = new SelectList(db.Shipments, "Id", "ShippingAddress", order.ShipmentId);
            return View(order);
        }

        // POST: ADMIN/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreatedDate,StatusId,Description,PaymentType,CustomerId,EmployeeId,ShipmentId,Discount,Tax")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", order.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", order.EmployeeId);
            ViewBag.StatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.StatusId);
            ViewBag.ShipmentId = new SelectList(db.Shipments, "Id", "ShippingAddress", order.ShipmentId);
            return View(order);
        }

        // GET: ADMIN/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: ADMIN/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
