using Newtonsoft.Json;
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
        [HttpPost]
        public ActionResult ProductFeatures(string data)
        {
            var features = JsonConvert.DeserializeObject<ProductFeature>(data);
            var f = db.ProductFeatures.Find(features.Id);
            if (f != null)
            {
                f.OS = features.OS;
                f.Processor = features.Processor;
                f.ScreenSize = features.ScreenSize;
                f.StorageCap = features.StorageCap;
                f.StorageType = features.StorageType;
                f.BatteryLife = features.BatteryLife;
                f.Graphic = features.Graphic;
                db.SaveChanges();
                return Content("OK");
            }
            else
            {
                return Content("Bay roi`");
            }
            
        }
        //}
        // GET: ADMIN/Products
        public ActionResult Index(int? page, string kw, string sort, string supplier,string category, string price,string discount,string isactive)
        {
            int pageNumber = page ?? 1;
            int pageSize = 6;
            //select all
            var p = db.Products.OrderBy(x => x.Id).Include(c=>c.Category).Include(d=>d.Supplier);


            //filter
            //supplier filter
            if (string.IsNullOrEmpty(supplier))
            {
                ViewBag.supplier = "allsup";
                supplier = "allsup";
            }
            else
            {
                ViewBag.supplier = supplier;
            }
            if (supplier.Equals("allsup"))
            {
                p = db.Products.OrderBy(x => x.Id).Include(c => c.Category).Include(d => d.Supplier);

            }
            else
            {
             
                p = db.Products.Where(x => x.Supplier.Name == supplier);

            }
            //category filter
            if (string.IsNullOrEmpty(category))
            {
                ViewBag.category = "allcat";
                category = "allcat";
            }
            else
            {
                ViewBag.category = category;
            }
            if (category.Equals("allcat"))
            {
                if (supplier.Equals("allsup"))
                {
                    p = db.Products.OrderBy(x => x.Id).Include(c => c.Category).Include(d => d.Supplier);
                }
                else
                {
               
                    p = db.Products.Where(x => x.Supplier.Name == supplier);
                }
            }
            else
            {
                if (supplier.Equals("allsup"))
                {
                    p = db.Products.Where(x => x.Category.Name == category);
                }
                else
                {
                    p = p.Where(x => x.Category.Name == category);

                }

            }
            //filter isactive
            if (string.IsNullOrEmpty(isactive))
            {
                ViewBag.isactive = "none";
                isactive = "none";
            }
            else
            {
                ViewBag.isactive = isactive;
            }
            if (!isactive.Equals("none"))
            {
             
                switch (isactive)
                {
                    case "active":
                        p = p.Where(x => x.IsActive);
                        break;
                    case "notactive":
                        p = p.Where(x => !x.IsActive);
                        break;
                }
            }
            else
            {
                if (category.Equals("allcat"))
                {
                    if (supplier.Equals("allsup"))
                    {
                        p = db.Products.OrderBy(x => x.Id).Include(c => c.Category).Include(d => d.Supplier);
                    }
                    else
                    {
                    
                        p = db.Products.Where(x => x.Supplier.Name == supplier);
                    }
                }
                else
                {
                    if (supplier.Equals("allsup"))
                    {
                        p = db.Products.Where(x => x.Category.Name == category);
                    }
                    else
                    {
                        p = p.Where(x => x.Category.Name == category);

                    }

                }
            }

            //filter price
            if (string.IsNullOrEmpty(price))
            {
                ViewBag.price = "allprice";
                price = "allprice";
            }
            else
            {
                ViewBag.price = price;
            }
            switch (price)
            {
                case "allprice":
                    p = p.Where(x => x.Price > 0);
                    break;
                case "0-1mil":
                    p = p.Where(x => x.Price > 0 && x.Price < 1000000);
                    break;
                case "1-5mil":
                    p = p.Where(x => x.Price > 1000000 && x.Price < 5000000);
                    break;
                case "5-10mil":
                    p = p.Where(x => x.Price > 5000000 && x.Price < 10000000);
                    break;
                case "10-20mil":
                    p = p.Where(x => x.Price > 10000000 && x.Price < 20000000);
                    break;
                case "20milup":
                    p = p.Where(x => x.Price > 20000000);
                    break;
            }

            //filter discount
            if (string.IsNullOrEmpty(discount))
            {
                ViewBag.discount = "none";
                discount = "none";
            }
            else
            {
                ViewBag.discount = discount;
            }
            switch (discount)
            {
                case "none":
                    p = p.Where(x => x.Discount >= 0);
                    break;
                case "0-10":
                    p = p.Where(x => x.Discount >= 0 && x.Discount <= 10);
                    break;
                case "10-20":
                    p = p.Where(x => x.Discount >= 10 && x.Discount < 20);
                    break;

                case "20up":
                    p = p.Where(x => x.Discount > 20);
                    break;
            }

            //search
            if (!string.IsNullOrEmpty(kw))
            {
                p = p.Where(x => x.Id.ToString().Equals(kw) || x.Name.ToLower().Contains(kw.ToLower()));
                ViewBag.kw = kw;
            }
            //sort

            
            if (string.IsNullOrEmpty(sort))
            {
                ViewBag.sort = "id_asc";
                sort = "id_asc";
            }
            else
            {
                ViewBag.sort = sort;
            }
            switch (sort)
            {
                case "id_asc":
                    p = p.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    p = p.OrderByDescending(x => x.Id);
                    break;
                case "name_asc":
                    p = p.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    p = p.OrderByDescending(x => x.Name);
                    break;
                case "category_asc":
                    p = p.OrderBy(x => x.Category.Name);
                    break;
                case "category_desc":
                    p = p.OrderByDescending(x => x.Category.Name);
                    break;
                case "least_stock":
                    p = p.OrderBy(x => x.Stock);
                    break;
                case "bestselling":
                    var bs = db.OrderDetails.Where(x=>x.Order.StatusId == 3).GroupBy(x => x.ProductId)
                   .Select(group => new
                   {
                       productid = group.Key,
                       Count = group.Count()
                   })
                   .OrderByDescending(x => x.Count);
                    p = from a in p
                        join b in bs on a.Id equals b.productid into c
                        from d in c.DefaultIfEmpty()
                        orderby d.Count descending
                        select a;
                     
                    break;
            }

            ViewBag.resultcount = p.Count();
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
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Discount,Stock,CategoryId,SupplierId,Description,Thumbnail,IsActive")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                UploadPictures(product.Id);
                NewProduct np = new NewProduct()
                {
                    ProductId = product.Id,
                    AddDate = DateTime.Now,
                    IsActive = true
                };
                db.NewProducts.Add(np);
                db.SaveChanges();
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
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Discount,Stock,CategoryId,SupplierId,Description,Thumbnail,IsActive")] Product product)
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
