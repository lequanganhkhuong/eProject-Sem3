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
    public class BestSellingsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: ADMIN/BestSellings
        public ActionResult Index()
        {
            var bestSellings = db.BestSellings.Include(b => b.Product);
            return View(bestSellings.ToList());
        }

        // GET: ADMIN/BestSellings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BestSelling bestSelling = db.BestSellings.Find(id);
            if (bestSelling == null)
            {
                return HttpNotFound();
            }
            return View(bestSelling);
        }

        // GET: ADMIN/BestSellings/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: ADMIN/BestSellings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId")] BestSelling bestSelling)
        {
            if (ModelState.IsValid)
            {
                db.BestSellings.Add(bestSelling);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", bestSelling.ProductId);
            return View(bestSelling);
        }

        // GET: ADMIN/BestSellings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BestSelling bestSelling = db.BestSellings.Find(id);
            if (bestSelling == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", bestSelling.ProductId);
            return View(bestSelling);
        }

        // POST: ADMIN/BestSellings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId")] BestSelling bestSelling)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bestSelling).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", bestSelling.ProductId);
            return View(bestSelling);
        }

        // GET: ADMIN/BestSellings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BestSelling bestSelling = db.BestSellings.Find(id);
            if (bestSelling == null)
            {
                return HttpNotFound();
            }
            return View(bestSelling);
        }

        // POST: ADMIN/BestSellings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BestSelling bestSelling = db.BestSellings.Find(id);
            db.BestSellings.Remove(bestSelling);
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
