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
    public class RecommendProductsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: ADMIN/RecommendProducts
        public ActionResult Index()
        {
            var recommendProducts = db.RecommendProducts.Include(r => r.Product);
            return View(recommendProducts.ToList());
        }

        // GET: ADMIN/RecommendProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecommendProduct recommendProduct = db.RecommendProducts.Find(id);
            if (recommendProduct == null)
            {
                return HttpNotFound();
            }
            return View(recommendProduct);
        }

        // GET: ADMIN/RecommendProducts/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: ADMIN/RecommendProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId")] RecommendProduct recommendProduct)
        {
            if (ModelState.IsValid)
            {
                db.RecommendProducts.Add(recommendProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", recommendProduct.ProductId);
            return View(recommendProduct);
        }

        // GET: ADMIN/RecommendProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecommendProduct recommendProduct = db.RecommendProducts.Find(id);
            if (recommendProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", recommendProduct.ProductId);
            return View(recommendProduct);
        }

        // POST: ADMIN/RecommendProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId")] RecommendProduct recommendProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recommendProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", recommendProduct.ProductId);
            return View(recommendProduct);
        }

        // GET: ADMIN/RecommendProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecommendProduct recommendProduct = db.RecommendProducts.Find(id);
            if (recommendProduct == null)
            {
                return HttpNotFound();
            }
            return View(recommendProduct);
        }

        // POST: ADMIN/RecommendProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecommendProduct recommendProduct = db.RecommendProducts.Find(id);
            db.RecommendProducts.Remove(recommendProduct);
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
