using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using ZuLuCommerce.Models;

namespace ZuLuCommerce.Areas.ADMIN.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();
        
        public ActionResult RemoveProduct(int pid, int gid)
        {
            try
            {
                var p = db.GroupItems.Where(x=>x.GroupId == gid && x.ProductId == pid).FirstOrDefault();
                db.GroupItems.Remove(p);
                db.SaveChanges();
                return Content("OK");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
        public ActionResult AddProduct(int pid,int gid)
        {
            try
            {

                GroupItem item = new GroupItem()
                {
                    ProductId = pid,
                    GroupId = gid
                };
                db.GroupItems.Add(item);
                db.SaveChanges();
                return Content("OK");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
        // GET: ADMIN/Groups
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            //select all
            var g = db.Groups.OrderBy(x => x.Id);
            return View(g.ToPagedList(pageNumber,pageSize));

        }

        

        // GET: ADMIN/Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ADMIN/Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,IsActive")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: ADMIN/Groups/Edit/5
        public ActionResult Edit(int? id, int? page, string kw, string sort, string supplier, string category,string discount,string isactive, string price)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }


            //get products list
            int pageNumber = page ?? 1;
            int pageSize = 6;
            //select all
            var p = db.Products.OrderBy(x => x.Id).Include(c => c.Category).Include(d => d.Supplier);


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
                sort = "id_asc";
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
                    sort = "id_asc";
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
                sort = "id_asc";
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
                        sort = "id_asc";
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
            }

            //get this group's products list 
            var groupitem = db.GroupItems.Where(x => x.GroupId == id);
            var productitem = from c in p
                              where groupitem.Select(x => x.ProductId).Contains(c.Id)
                              orderby c.Id
                              select c;

            //get Product list that is not in this group
            p = from d in p
                where !productitem.Select(x => x.Id).Contains(d.Id)
                orderby d.Id
                select d;
            ViewBag.resultcount = p.Count();
            ViewBag.groupitems = productitem.Count();
            return View(Tuple.Create<Group,IPagedList<Product>, IPagedList<Product>>(group, p.ToPagedList(pageNumber, pageSize), productitem.ToPagedList(pageNumber,pageSize)));
        }

        // POST: ADMIN/Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "Item1")] Group group)
        {
            
                if (ModelState.IsValid)
                {
                    db.Entry(group).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(group);
    
           
        }

        // GET: ADMIN/Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: ADMIN/Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var groupitems = db.GroupItems.Where(x => x.GroupId == id).ToList();
            foreach(var a in groupitems)
            {
                db.GroupItems.Remove(a);
            }
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);

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
