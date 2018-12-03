using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ZuLuCommerce.Models;

namespace ZuLuCommerce.Areas.ADMIN.Controllers
{
    public class AccountsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();

        // GET: Admin/Account/Login
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                int cur = int.Parse(User.Identity.Name);
                var emp = db.Employees.Where(x => x.Id == cur).SingleOrDefault();
                emp.IsOnline = false;
                emp.LastLogin = DateTime.Now;
                db.SaveChanges();
                FormsAuthentication.SignOut();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Loginname, string Password)
        {
            
            

            var emp = db.Employees.Where(x => x.Username.Equals(Loginname)).SingleOrDefault();
            if (emp != null)
            {
                if (emp.Password.Equals(MySecurity.EncryptPass(Password)))
                {
                    if (emp.IsActive)
                    {
                        FormsAuthentication.SetAuthCookie(emp.Id.ToString(), false);
                        emp.IsOnline = true;

                        db.SaveChanges();
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ViewBag.Message = "This user is blocked";
                    }
                   

                }
                else
                {
                    ViewBag.Message = "Password invalid";
                }
            }
            else
            {
                ViewBag.Message = "Login name not exist";
            }
            return View();
        }
        public ActionResult Logout()
        {
            int cur = int.Parse(User.Identity.Name);
            var emp = db.Employees.Where(x => x.Id == cur).SingleOrDefault();
            emp.IsOnline = false;
            emp.LastLogin = DateTime.Now;
            db.SaveChanges();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        // GET: ADMIN/Accounts
        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(a => a.Customer);
            return View(accounts.ToList());
        }

        // GET: ADMIN/Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: ADMIN/Accounts/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName");
            return View();
        }

        // POST: ADMIN/Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,CustomerId,IsActive")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", account.CustomerId);
            return View(account);
        }

        // GET: ADMIN/Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", account.CustomerId);
            return View(account);
        }

        // POST: ADMIN/Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,CustomerId,IsActive")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", account.CustomerId);
            return View(account);
        }

        // GET: ADMIN/Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: ADMIN/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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
