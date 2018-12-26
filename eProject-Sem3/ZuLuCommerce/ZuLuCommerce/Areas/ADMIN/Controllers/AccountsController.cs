using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ZuLuCommerce.Areas.ADMIN.Models;
using ZuLuCommerce.Models;

namespace ZuLuCommerce.Areas.ADMIN.Controllers
{
    
    public class AccountsController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string username, string email)
        {
            var emp = db.Employees.Where(x => x.Username.Equals(username)).SingleOrDefault();
            if(emp != null)
            {
                if (emp.Email.Equals(email))
                {
                    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    var stringChars = new char[8];
                    var random = new Random();

                    for (int i = 0; i < stringChars.Length; i++)
                    {
                        stringChars[i] = chars[random.Next(chars.Length)];
                    }

                    var finalString = new String(stringChars);

                    //send mail
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("anhkhuongsubmail@gmail.com");
                    mail.To.Add(emp.Email);
                    mail.Subject = "Your new password";
                    mail.Body = "Your new password is " + finalString;

                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.IsBodyHtml = true;

                    //Create SMTP for send mail
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("anhkhuongsubmail", "AnhKhuong1");
                    smtp.EnableSsl = true;

                    //Call Send mail -> Check all Spam
                    smtp.Send(mail);

                    emp.Password = MySecurity.EncryptPass(finalString);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Message = "Invalid email address";
                    return View();
                }
            }
            else
            {
                ViewBag.Message = "Invalid username";
                return View();
            }
            
        }
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
                        var checknewproduct = db.NewProducts.Where(x=>x.IsActive).ToList();
                        foreach(var c in checknewproduct)
                        {
                            if((DateTime.Now - c.AddDate).Days >= 60)
                            {
                                c.IsActive = false;
                                db.SaveChanges();
                            }
                        }
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
        [Authorize(Roles = "Admin")]
        // GET: ADMIN/Accounts
        public ActionResult Index(int? page, string kw)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            var accounts = db.Accounts.OrderBy(x => x.Id).Include(a => a.Customer);
            //search
            if (!string.IsNullOrEmpty(kw))
            {
                accounts = accounts.Where(x => x.Id.ToString().Equals(kw) || x.Username.ToLower().Contains(kw.ToLower()));
                ViewBag.kw = kw;
            }

            return View(accounts.ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
        //// GET: ADMIN/Accounts/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName");
        //    return View();
        //}
        //[Authorize(Roles = "Admin")]
        //// POST: ADMIN/Accounts/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Username,Password,CustomerId,IsActive")] Account account)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Accounts.Add(account);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", account.CustomerId);
        //    return View(account);
        //}
        [Authorize(Roles = "Admin")]
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
            CustomerVM acc = new CustomerVM()
            {
                Id = account.Id,
                Username = account.Username,
                IsActive = account.IsActive,
                FirstName = account.Customer.FirstName,
                
                LastName = account.Customer.LastName,
                Phone = account.Customer.Phone,
                Address = account.Customer.Address,
                Email = account.Customer.Email,
                Birthday = account.Customer.Birthday
            };
            
            return View(acc);
        }
        [Authorize(Roles = "Admin")]
        // POST: ADMIN/Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IsActive,FirstName,LastName,Phone,Address,Email,Birthday")] CustomerVM cus)
        {
            try
            {
                Account account = db.Accounts.Find(cus.Id);
                account.IsActive = cus.IsActive;
                account.Customer.FirstName = cus.FirstName;
                account.Customer.LastName = cus.LastName;
                account.Customer.Phone = cus.Phone;
                account.Customer.Address = cus.Address;
                account.Customer.Email = cus.Email;
                account.Customer.Birthday = cus.Birthday;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
                
            
            
            return View(cus);
        }

        [Authorize(Roles = "Admin")]
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
