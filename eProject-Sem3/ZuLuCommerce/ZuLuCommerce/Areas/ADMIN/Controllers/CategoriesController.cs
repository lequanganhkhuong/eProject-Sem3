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
    [Authorize(Roles ="Admin")]
    public class CategoriesController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: ADMIN/Categories
        public ActionResult Index(int? page, string topic, string sort, string kw)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var p = db.Categories.OrderBy(x => x.Id).Include(c => c.Topic);

            //filter topic
            if (string.IsNullOrEmpty(topic))
            {
                ViewBag.topic = "alltopic";
                topic = "alltopic";
            }
            else
            {
                ViewBag.topic = topic;
            }
            if (!topic.Equals("alltopic"))
            {
                sort = "id_asc";
                p = db.Categories.Where(x => x.Topic.TopicName == topic);

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
               
            }


            ViewBag.resultcount = p.Count();
            return View(p.ToPagedList(pageNumber,pageSize));
        }

        // GET: ADMIN/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: ADMIN/Categories/Create
        public ActionResult Create()
        {
            ViewBag.TopicId = new SelectList(db.Topics, "Id", "TopicName");
            return View();
        }

        // POST: ADMIN/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,TopicId,IsActive")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TopicId = new SelectList(db.Topics, "Id", "TopicName", category.TopicId);
            return View(category);
        }

        // GET: ADMIN/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.TopicId = new SelectList(db.Topics, "Id", "TopicName", category.TopicId);
            return View(category);
        }

        // POST: ADMIN/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,TopicId,IsActive")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TopicId = new SelectList(db.Topics, "Id", "TopicName", category.TopicId);
            return View(category);
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
