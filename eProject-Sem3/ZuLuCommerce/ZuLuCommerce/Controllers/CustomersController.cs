using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ZuLuCommerce.Areas.ADMIN.Models;
using ZuLuCommerce.Models;
using ZuLuCommerce.Models.RoleSecurity;
using ZuLuCommerce.Models.VM;

namespace ZuLuCommerce.Controllers
{
    public class CustomersController : Controller
    {
        eCommerceEntities db = new eCommerceEntities();
       
        public ActionResult ForgotPassword()
        {
            return View();    
        }
     
        [HttpPost]
        public ActionResult ForgotPassword(string username, string email)
        {
            var cus = db.Accounts.Where(x => x.Username.Equals(username)).SingleOrDefault();
            if (cus != null)
            {
                if (cus.Customer.Email.Equals(email))
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
                    mail.To.Add(cus.Customer.Email);
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

                    cus.Password = MySecurity.EncryptPass(finalString);
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
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                Logout();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Loginname, string Password)
        {
            
            var cus = db.Accounts.Where(x => x.Username.Equals(Loginname)).SingleOrDefault();
            if (cus != null)
            {
                if (cus.Password.Equals(MySecurity.EncryptPass(Password)))
                {
                    if (cus.IsActive)
                    {
                        FormsAuthentication.SetAuthCookie(cus.Id.ToString(), false);
                        return RedirectToAction("Index", "Products");
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
        [CustomAuthorization(LoginPage = "~/Customers/Login")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult Register()
        {
            ViewBag.CityId = new SelectList(db.CityShippingFees, "Id", "CityName");
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterVM data)
        {
            if (db.Accounts.Select(x => x.Username).Contains(data.Username))
            {
                ViewBag.Message = "Username existed";
                return View();
            }
            if (db.Customers.Select(x => x.Email).Contains(data.Email))
            {
                ViewBag.Message = "This email is registered to another account!";
                return View();
            }
            try
            {
                Customer cus = new Customer()
                {
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Phone = data.Phone,
                    Email = data.Email,
                    Address = data.Address,
                    Birthday = data.Birthday,
                    CityId = data.CityId,
                    IsRegistered = true
                };
                db.Customers.Add(cus);
                db.SaveChanges();
                var c = db.Customers.Where(x => x.FirstName == data.FirstName && x.LastName == data.LastName && x.Email == data.Email).FirstOrDefault();
                if (c != null)
                {
                    Account acc = new Account()
                    {
                        Username = data.Username,
                        Password = MySecurity.EncryptPass(data.Password),
                        CustomerId = c.Id,
                        IsActive = false,
                        RegisterToken = MySecurity.EncryptPass(data.Username + DateTime.Now.Ticks),
                        RegisterDate = DateTime.Now
                    };
                    
                    db.Accounts.Add(acc);
                    db.SaveChanges();
                    //send mail
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("anhkhuongsubmail@gmail.com");
                    mail.To.Add(cus.Email);
                    mail.Subject = "Activate Account";
                    mail.Body = "Follow this link to activate your account <a href='http://group5.aptech.cloud/Customers/Activate?token=" + acc.RegisterToken +
                        "'>Click here to activate</a>";


                    // please use this line of code instead when running in localhost
                    //mail.Body = "Follow this link to activate your account <a href='http://localhost:54358/Customers/Activate?token=" + acc.RegisterToken +
                    //    "'>Click here to activate</a>";

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

                    return RedirectToAction("ConfirmEmail","Customers",new { id = acc.Id});
                }
                else
                {
                    ViewBag.Message = "Error";
                    return View();
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View();
        }
        public ActionResult ConfirmEmail(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var acc = db.Accounts.Find(id);

            if (acc == null || acc.IsActive)
            {
                return HttpNotFound();
            }
            return View(acc);
        }

        public ActionResult ResendToken(int id)
        {
            var acc = db.Accounts.Find(id);
            acc.RegisterToken = MySecurity.EncryptPass(acc.Username + DateTime.Now.Ticks);

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("anhkhuongsubmail@gmail.com");
            mail.To.Add(acc.Customer.Email);
            mail.Subject = "Activate Account";
            mail.Body = "Follow this link to activate your account <a href='http://localhost:54358/Customers/Activate?token=" + acc.RegisterToken +
                "'>Click here to activate</a>";

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
            return Content("OK");
        }
        public ActionResult Activate(string token)
        {
            var acc = db.Accounts.Where(x => x.RegisterToken.Equals(token)).SingleOrDefault();
            if (acc != null)
            {
                DateTime date = acc.RegisterDate.HasValue ? acc.RegisterDate.Value : DateTime.Now;
                var h = (DateTime.Now - date).Hours;
                if (h <= 24)
                {
                    acc.IsActive = true;
                    acc.RegisterToken = "";
                    db.SaveChanges();
                    
                    ViewBag.Message = "Account activate successfully";

                }
                else
                {
                    ViewBag.Message = "Token has expired!";
                }
            }
            else
            {
                ViewBag.Message = "Token invalid";
            }
            return View();
        }
        [CustomAuthorization(LoginPage = "~/Customers/Login")]
        public ActionResult UpdateProfile(int? id, string kw, int? page)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cus = db.Customers.Find(id);
            if(cus == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var order = db.Orders.Where(x => x.CustomerId == id);
            //search
            if (!string.IsNullOrEmpty(kw))
            {
                order = order.Where(x => x.Codenname.ToLower().Contains(kw.ToLower()));
                ViewBag.kw = kw;
            }
            ViewBag.Orders = order.OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize);
            ViewBag.CityId = new SelectList(db.CityShippingFees, "Id", "CityName");
            return View(cus);
        }
        [CustomAuthorization(LoginPage = "~/Customers/Login")]
        [HttpPost]
        public ActionResult UpdateProfile(Customer data)
        {
            var cus = db.Customers.Find(data.Id);
            if(cus!= null)
            {
                try
                {
                    cus.FirstName = data.FirstName;
                    cus.LastName = data.LastName;
                    cus.Address = data.Address;
                    cus.Birthday = data.Birthday;
                    cus.Email = data.Email;
                    cus.Phone = data.Phone;
                    cus.CityId = data.CityId;

                    db.SaveChanges();
                    ViewBag.Message = "Update success";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                  
                }
            }
            ViewBag.CityId = new SelectList(db.CityShippingFees, "Id", "CityName", cus.CityId);
            return View();
        }
        [CustomAuthorization(LoginPage = "~/Customers/Login")]
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
            Account acc = db.Accounts.Find(id);
            if (acc == null)
            {
                return HttpNotFound();
            }
            ChangePasswordVm change = new ChangePasswordVm()
            {
                Id = acc.Id,
                Username = acc.Username
            };

            return View(change);
        }
        [CustomAuthorization(LoginPage = "~/Customers/Login")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordVm data)
        {
            var cus = db.Accounts.Find(data.Id);
            if (!cus.Password.Equals(MySecurity.EncryptPass(data.OldPassword)))
            {
                ViewBag.Message = "Wrong password!";
            }
            else
            {
                cus.Password = MySecurity.EncryptPass(data.Password);
                db.SaveChanges();
                return RedirectToAction("UpdateProfile", new { id = cus.CustomerId});
            }

            return View(data);
        }
    }
}