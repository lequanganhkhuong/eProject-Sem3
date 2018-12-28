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
using ZuLuCommerce.Areas.ADMIN.Models;
using ZuLuCommerce.Models;

namespace ZuLuCommerce.Areas.ADMIN.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();
        public ActionResult UserProfile(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != int.Parse(User.Identity.Name))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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
                return RedirectToAction("UserProfile");
            }
            ViewBag.LevelId = new SelectList(db.Levels, "Id", "LevelName", employee.LevelId);
            return View(employee);
        }
        public ActionResult ChangePassword(int? id)
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
            ChangePasswordVm change = new ChangePasswordVm()
            {
                Id = employee.Id,
                Username = employee.Username
            };

            return View(change);
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordVm data)
        {
            var emp = db.Employees.Find(data.Id);
            if (!emp.Password.Equals(MySecurity.EncryptPass(data.OldPassword)))
            {
                ViewBag.Message = "Wrong password!";
            }
            else
            {
                emp.Password = MySecurity.EncryptPass(data.Password);
                db.SaveChanges();
                return RedirectToAction("UserProfile",new { id=data.Id});
            }

            return View(data);
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
        public ActionResult Index(int? page,string kw, string level,string isactive, string sort, string isonline,string gender)
        {
            int pageSize = 12;
            int pageNumber = page ?? 1;
            var employees = db.Employees.OrderBy(x=>x.Id).Include(e => e.Level);

            //filter
            //level filter
            if (string.IsNullOrEmpty(level))
            {
                ViewBag.level = "alllevel";
                level = "alllevel";
            }
            else
            {
                ViewBag.level = level;
            }
            if (level.Equals("alllevel"))
            {
                employees = db.Employees.OrderBy(x => x.Id).Include(e => e.Level);

            }
            else
            {
             
                employees = db.Employees.Where(x => x.Level.LevelName == level);

            }
            //gender filter
            if (string.IsNullOrEmpty(gender))
            {
                ViewBag.gender = "allgender";
                gender = "allgender";
            }
            else
            {
                ViewBag.gender = gender;
            }
            if (gender.Equals("allgender"))
            {
                if (level.Equals("alllevel"))
                {
                    employees = db.Employees.OrderBy(x => x.Id).Include(e => e.Level);
                }
                else
                {
               
                    employees = db.Employees.Where(x => x.Level.LevelName == level);
                }
            }
            else
            {
                if (level.Equals("alllevel"))
                {
                    employees = db.Employees.Where(x => x.Gender == gender);
                }
                else
                {
                    employees = employees.Where(x => x.Gender == gender);

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
                        employees = employees.Where(x => x.IsActive);
                        break;
                    case "notactive":
                        employees = employees.Where(x => !x.IsActive);
                        break;
                }
            }
            else
            {
                if (level.Equals("alllevel"))
                {
                    if (gender.Equals("allgender"))
                    {
                        employees = db.Employees.OrderBy(x => x.Id).Include(e => e.Level);
                    }
                    else
                    {
                     
                        employees = db.Employees.Where(x => x.Gender == gender);
                    }
                }
                else
                {
                    if (gender.Equals("allgender"))
                    {
                        employees = db.Employees.Where(x => x.Level.LevelName == level);
                    }
                    else
                    {
                        employees = employees.Where(x => x.Level.LevelName == level);

                    }

                }
            }

            //filter isonline
            if (string.IsNullOrEmpty(isonline))
            {
                ViewBag.isonline = "none";
                isonline = "none";
            }
            else
            {
                ViewBag.isonline = isonline;
            }
            if (!isonline.Equals("none"))
            {
            
                switch (isonline)
                {
                    case "online":
                        employees = employees.Where(x => x.IsOnline);
                        break;
                    case "notonline":
                        employees = employees.Where(x => !x.IsOnline);
                        break;
                }
            }
            else
            {
                if (level.Equals("alllevel"))
                {
                    if (gender.Equals("allgender"))
                    {
                        if (isactive.Equals("none"))
                        {
                            employees = db.Employees.OrderBy(x => x.Id).Include(e => e.Level);
                        }
                        else
                        {
                            switch (isactive)
                            {
                                case "active":
                                    employees = employees.Where(x => x.IsActive);
                                    break;
                                case "notactive":
                                    employees = employees.Where(x => !x.IsActive);
                                    break;
                            }
                        }
                        
                    }
                    else
                    {
                        if (isactive.Equals("none"))
                        {
                            employees = db.Employees.Where(x => x.Gender == gender);
                        }
                        else
                        {
                            switch (isactive)
                            {
                                case "active":
                                    employees = employees.Where(x => x.IsActive);
                                    break;
                                case "notactive":
                                    employees = employees.Where(x => !x.IsActive);
                                    break;
                            }
                        }
                       
                    }
                }
                else
                {
                    if (gender.Equals("allgender"))
                    {
                        if (isactive.Equals("none"))
                        {
                            employees = db.Employees.Where(x => x.Level.LevelName == level);
                        }
                        else
                        {
                            switch (isactive)
                            {
                                case "active":
                                    employees = employees.Where(x => x.IsActive);
                                    break;
                                case "notactive":
                                    employees = employees.Where(x => !x.IsActive);
                                    break;
                            }
                        }
                        
                    }
                    else
                    {
                        if (isactive.Equals("none"))
                        {
                            employees = employees.Where(x => x.Level.LevelName == level);
                        }
                        else
                        {
                            switch (isactive)
                            {
                                case "active":
                                    employees = employees.Where(x => x.IsActive);
                                    break;
                                case "notactive":
                                    employees = employees.Where(x => !x.IsActive);
                                    break;
                            }
                        }
                        employees = employees.Where(x => x.Level.LevelName == level);

                    }

                }
            }
            //search
            if (!string.IsNullOrEmpty(kw))
            {
                employees = employees.Where(x => x.Id.ToString().Equals(kw) || x.Username.ToLower().Contains(kw.ToLower()) || x.FirstName.ToLower().Contains(kw.ToLower()) || x.LastName.ToLower().Contains(kw.ToLower()));
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
                    employees = employees.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    employees = employees.OrderByDescending(x => x.Id);
                    break;
                case "name_asc":
                    employees = employees.OrderBy(x => x.FirstName);
                    break;
                case "name_desc":
                    employees = employees.OrderByDescending(x => x.FirstName);
                    break;

            }


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
        [Authorize(Roles = "Admin")]
        // POST: ADMIN/Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Phone,Address,Email,Gender,Birthday,Username,Password,LevelId,Avatar,LastLogin,IsActive")] Employee employee)
        {
            if(!employee.Gender.ToLower().Equals("male") && !employee.Gender.ToLower().Equals("female"))
            {
                return Content("Gender cant be anything other than male and female");
            }
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
        [Authorize(Roles = "Admin")]
        // POST: ADMIN/Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Phone,Address,Email,Gender,Birthday,Username,Password,LevelId,Avatar,LastLogin,IsActive,IsOnline")] Employee employee)
        {
            if (!employee.Gender.ToLower().Equals("male") && !employee.Gender.ToLower().Equals("female"))
            {
                return Content("Gender cant be anything other than male and female");
            }
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                var emp = db.Employees.Where(x=>x.Username == employee.Username).FirstOrDefault();
                if(emp.Password != MySecurity.EncryptPass(employee.Password))
                {
                    employee.Password = MySecurity.EncryptPass(employee.Password);
                }
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
