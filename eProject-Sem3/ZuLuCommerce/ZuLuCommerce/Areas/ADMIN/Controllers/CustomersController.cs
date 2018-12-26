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
    [Authorize]
    public class CustomersController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: ADMIN/Customers
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var customers = db.Customers.OrderBy(x=>x.Id).Include(c => c.CityShippingFee);
            return View(customers.ToPagedList(pageNumber, pageSize));
        }

        // GET: ADMIN/Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [Authorize(Roles ="Admin")]
        // GET: ADMIN/Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.CityShippingFees, "Id", "CityName", customer.CityId);
            return View(customer);
        }
        [Authorize(Roles = "Admin")]
        // POST: ADMIN/Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Phone,Address,Email,Birthday,CityId,IsRegistered")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.CityShippingFees, "Id", "CityName", customer.CityId);
            return View(customer);
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
