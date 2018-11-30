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
    
    public class EmployeesController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();
        public ActionResult UserProfile(int? id)
        {
            if (id != int.Parse(User.Identity.Name))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName", employee.LevelId);

            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile([Bind(Include = "Id,FirstName,LastName,Phone,Address,Email,Gender,Birthday,Username,Password,LevelId,Avatar,LastLogin,IsActive,IsOnline")] Employee employee)
        {
            if (ModelState.IsValid)
            {

                db.Entry(employee).State = EntityState.Modified;
                if (User.Identity.IsAuthenticated && employee.Id == int.Parse(User.Identity.Name))
                {
                    employee.IsOnline = true;
                    employee.LastLogin = DateTime.Now;
                }
                if (Request.Files.Count > 0)
                {
                    UploadPictures(employee.Id);
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName", employee.LevelId);
            return View(employee);
        }
        private void UploadPictures(int id)
        {
            var e = db.Employees.Find(id);
            if (Request.Files.Count > 0)
            {
                //add picture
                string path = Server.MapPath("~/Uploads/Employees/") + "\\" + id;
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
                        if(e.Avatar != filename )
                        {
                            file.SaveAs(path + "\\" + filename);
                            e.Avatar = filename;
                        }
                       

                    }
                    catch { }
                }
                db.SaveChanges();
            }
            
        }
        [Authorize(Roles = "Admin")]

        // GET: ADMIN/Employees
        public ActionResult Index(int? page)
        {
            int pageSize = 12;
            int pageNumber = page ?? 1;
            var employees = db.Employees.Include(e => e.Level).OrderBy(x=>x.Id);
            return View(employees.ToPagedList(pageNumber,pageSize));
        }
        [Authorize(Roles = "Admin")]
        // GET: ADMIN/Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        [Authorize(Roles = "Admin")]
        // GET: ADMIN/Employees/Create
        public ActionResult Create()
        {
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName");
            return View();
        }

        // POST: ADMIN/Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Phone,Address,Email,Gender,Birthday,Username,Password,LevelId,Avatar,LastLogin,IsActive")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Password = MySecurity.EncryptPass(employee.Password);
                employee.IsActive = false;
                employee.IsOnline = false;
                
                db.Employees.Add(employee);
                db.SaveChanges();
                if (employee.LevelId == 1)
                {
                    for(int i = 1; i <= 3; i++)
                    {
                        EmployeeLevel el = new EmployeeLevel()
                        {
                            EmployeeId = employee.Id,
                            LevelId = i
                        };

                        db.EmployeeLevels.Add(el);
                    }
                    
                }
                if (employee.LevelId == 2)
                {
                    for (int i = 2; i <= 3; i++)
                    {
                        EmployeeLevel el = new EmployeeLevel()
                        {
                            EmployeeId = employee.Id,
                            LevelId = i
                        };

                        db.EmployeeLevels.Add(el);
                    }

                }
                if (employee.LevelId == 3)
                {
                    
                        EmployeeLevel el = new EmployeeLevel()
                        {
                            EmployeeId = employee.Id,
                            LevelId = 3
                        };

                        db.EmployeeLevels.Add(el);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName", employee.LevelId);
            return View(employee);
        }
        [Authorize(Roles = "Admin")]
        // GET: ADMIN/Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName", employee.LevelId);
            return View(employee);
        }

        // POST: ADMIN/Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Phone,Address,Email,Gender,Birthday,Username,Password,LevelId,Avatar,LastLogin,IsActive,IsOnline")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
         
                if (User.Identity.IsAuthenticated && employee.Id == int.Parse(User.Identity.Name))
                {
                    employee.IsOnline = true;
                    employee.LastLogin = DateTime.Now;
                }
                if (Request.Files.Count > 0)
                {
                    UploadPictures(employee.Id);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName", employee.LevelId);
            return View(employee);
        }
        [Authorize(Roles = "Admin")]
        // GET: ADMIN/Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: ADMIN/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
