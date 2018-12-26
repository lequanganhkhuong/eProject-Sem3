using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZuLuCommerce.Models;

namespace ZuLuCommerce.Areas.ADMIN.Controllers
{
    public class BannerProductsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();
       
        // GET: ADMIN/BannerProducts
        public ActionResult Index()
        {
            var bannerProducts = db.BannerProducts.Include(b => b.Product);
            return View(bannerProducts.ToList());
        }

        // GET: ADMIN/BannerProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannerProduct bannerProduct = db.BannerProducts.Find(id);
            if (bannerProduct == null)
            {
                return HttpNotFound();
            }
            return View(bannerProduct);
        }

        // GET: ADMIN/BannerProducts/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: ADMIN/BannerProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,Description1,Description2,PictureUrl")] BannerProduct bannerProduct)
        {
            if (ModelState.IsValid)
            {
                //add picture
                
                
                db.BannerProducts.Add(bannerProduct);
                db.SaveChanges();
                string path = Server.MapPath("~/Uploads/Banner") + "\\" + bannerProduct.Id;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    string filename = file.FileName.Split('\\').Last();
                    try
                    {
                        file.SaveAs(path + "\\" + filename);

                    }
                    catch { }
                    bannerProduct.PictureUrl = filename;
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", bannerProduct.ProductId);
            return View(bannerProduct);
        }

        // GET: ADMIN/BannerProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannerProduct bannerProduct = db.BannerProducts.Find(id);
            if (bannerProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", bannerProduct.ProductId);
            return View(bannerProduct);
        }

        // POST: ADMIN/BannerProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,PictureUrl")] BannerProduct bannerProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bannerProduct).State = EntityState.Modified;
                string path = Server.MapPath("~/Uploads/Banner") + "\\" + bannerProduct.Id;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    string filename = file.FileName.Split('\\').Last();
                    try
                    {
                        file.SaveAs(path + "\\" + filename);

                    }
                    catch { }
                    bannerProduct.PictureUrl = filename;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", bannerProduct.ProductId);
            return View(bannerProduct);
        }

        // GET: ADMIN/BannerProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannerProduct bannerProduct = db.BannerProducts.Find(id);
            if (bannerProduct == null)
            {
                return HttpNotFound();
            }
            return View(bannerProduct);
        }

        // POST: ADMIN/BannerProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BannerProduct bannerProduct = db.BannerProducts.Find(id);
            db.BannerProducts.Remove(bannerProduct);
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
