using PagedList;
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
    [Authorize(Roles = "Admin,Manager")]
    public class ProductsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        private void UploadPictures(int product_id)
        {
            //add picture
            string path = Server.MapPath("~/Uploads/Products") + "\\" + product_id;
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
                    Picture p = new Picture()
                    {
                        ProductId = product_id,
                        URL = filename
                    };
                    db.Pictures.Add(p);
                }
                catch { }
            }
            db.SaveChanges();
        }
        public ActionResult DeletePictures(int id)
        {
            try
            {
                var picture = db.Pictures.Find(id);
                string path = Server.MapPath("~/Uploads/Products") + "\\" + picture.ProductId;
                string f = path + "\\" + picture.URL;
                if (System.IO.File.Exists(f))
                {
                    System.IO.File.Delete(f);
                }
                db.Pictures.Remove(picture);
                db.SaveChanges();
                return Content("OK");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult SetThumbnail(int id)
        {
            try
            {

                var pic = db.Pictures.Find(id);
                var product = db.Products.Where(x => x.Id == pic.ProductId).FirstOrDefault();
                product.Thumbnail = pic.URL;
                db.SaveChanges();
                return Content("OK");
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        // GET: ADMIN/Products
        public ActionResult Index(int? page, string kw, string sort)
        {
            int pageNumber = page ?? 1;
            int pageSize = 6;
            //select all
            var p = db.Products.OrderBy(x => x.Id).Include(c=>c.Category).Include(d=>d.Supplier);
            //search
            if (!string.IsNullOrEmpty(kw))
            {
                p = p.Where(x => x.Id.ToString().Equals(kw) || x.Name.ToLower().Contains(kw.ToLower()));
                ViewBag.kw = kw;
            }
            //sort

            //ViewBag.sortTopic = "topic_asc";
            if (string.IsNullOrEmpty(sort))
            {
                ViewBag.sort = "id_asc";

            }
            else
            {
                ViewBag.sort = sort;
            }
            switch (sort)
            {
                case "id_asc":
                    p = p.OrderBy(x => x.Id);
                    ViewBag.sortId = "id_desc";
                    break;
                case "id_desc":
                    p = p.OrderByDescending(x => x.Id);
                    ViewBag.sortId = "id_asc";
                    break;
                case "name_asc":
                    p = p.OrderBy(x => x.Name);
                    ViewBag.sortName = "name_desc";
                    break;
                case "name_desc":
                    p = p.OrderByDescending(x => x.Name);
                    ViewBag.sortName = "name_asc";
                    break;
                case "category_asc":
                    p = p.OrderBy(x => x.Category.Name);
                    ViewBag.sortTopic = "category_desc";
                    break;
                case "category_desc":
                    p = p.OrderByDescending(x => x.Category.Name);
                    ViewBag.sortTopic = "category_asc";
                    break;
            }
            ViewBag.sortId = sort ?? "id_desc";
            ViewBag.sortName = sort ?? "name_desc";
            ViewBag.sortTopic = sort ?? "topic_desc";

            return View(p.ToPagedList(pageNumber, pageSize));
        }

        // GET: ADMIN/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ADMIN/Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
            return View();
        }

        // POST: ADMIN/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Discount,Stock,CategoryId,SupplierId,Description,Thumbnail")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                UploadPictures(product.Id);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            return View(product);
        }

        // GET: ADMIN/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            return View(product);
        }

        // POST: ADMIN/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Discount,Stock,CategoryId,SupplierId,Description,Thumbnail")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                UploadPictures(product.Id);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            return View(product);
        }

        // GET: ADMIN/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ADMIN/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
