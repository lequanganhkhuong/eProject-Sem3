using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZuLuCommerce.Models;
namespace ZuLuCommerce.Controllers
{
    public class CartsController : Controller
    {
        eCommerceEntities db = new eCommerceEntities();
       
        public ActionResult AddToCart(int id, int qty)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            try
            {
                var p = db.Products.Where(x => x.Id == id).FirstOrDefault();
                if(p == null)
                {
                    return Content("Invalid");
                }
                var userid = int.Parse(User.Identity.Name);
                var cart = db.Carts.Where(x => x.UserId == userid).SingleOrDefault();
                if (cart.CartItems.Select(x => x.ProductId).Contains(id))
                {
                    var item = db.CartItems.Where(x => x.ProductId == id && x.CartId == cart.Id).SingleOrDefault();
                    item.Quantity += qty;
                    db.SaveChanges();
                }
                else
                {
                    CartItem ci = new CartItem()
                    {
                        CartId = cart.Id,
                        ProductId = id,
                        Quantity = qty

                    };
                    db.CartItems.Add(ci);
                    db.SaveChanges();
                }
                var count = cart.CartItems.Select(x => x.ProductId).Count();
                return Content(count.ToString());
            }
            catch (Exception)
            {

                return Content("Invalid");
            }

        }
        public ActionResult ChangeQuantityCart(int id,int qty,int price)
        {
            var item = db.CartItems.Find(id);
            if(item == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            try
            {
                item.Quantity = qty;
                db.SaveChanges();
                var reduce = qty * price;
                return Content(reduce.ToString());
            }
            catch (Exception)
            {
                return Content("There is an error in the server, please try again later!");
            }
            
        }
        public ActionResult RemoveFromCart(int id,int cartid)
        {
            var item = db.CartItems.Find(id);
            
            if (item == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            try
            {
                

                db.CartItems.Remove(item);
                db.SaveChanges();
               
                var cart = db.CartItems.Where(x => x.CartId == cartid);
                var count = cart.Count();
                decimal price = 0;
                int total = 0;
                var specdeal = db.SpecialDeals.Where(x => x.IsActive && x.EndDate > DateTime.Now && x.StartDate < DateTime.Now);
                foreach (var c in cart)
                {

                    if (specdeal.Select(x => x.ProductId).Contains(c.ProductId))
                    {
                        price = c.Product.Price * (100 - specdeal.Where(x => x.ProductId == c.ProductId).SingleOrDefault().Discount) / 100;
                    }
                    else
                    {
                        price = c.Product.Price * (100 - c.Product.Discount) / 100;
                    }
                    total += (int)price *c.Quantity;
                }
                return Content(count.ToString() + "-" + total.ToString());
            }
            catch (Exception)
            {
                return Content("There is an error in the server, please try again later!");
            }
           
        }
        // GET: Carts
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                var id = int.Parse(User.Identity.Name);
                var cart = db.CartItems.Where(x => x.Cart.UserId == id).ToList(); ;
                return View(cart);
            }
            
        }
        
        public ActionResult Checkout(int? id)
        {
            
            if(id == null)
            {
                ViewBag.CityId = new SelectList(db.CityShippingFees, "Id", "CityName");
                return View();
            }
            else
            {
                if (!User.Identity.IsAuthenticated || id != int.Parse(User.Identity.Name))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                var acc = db.Accounts.Find(id);
                var customer = db.Customers.Where(x => x.Id == acc.CustomerId).SingleOrDefault();
                ViewBag.carts = db.CartItems.Where(x => x.Cart.UserId == id).ToList();
                ViewBag.CityId = new SelectList(db.CityShippingFees, "Id", "CityName", customer.CityId);
                return View(customer);
            }
            
        }
        [HttpPost]
        public ActionResult Checkout(Customer customer,string payment,string data,string discount,string description)
        {
            // mail content
            List<OrderDetail> maillist = new List<OrderDetail>();
            var codename = "";
            var maildiscount = "";
            decimal shipfee = 0;

            //
            decimal price = 0;
            decimal subtotal = 0;
            var coupon = db.Coupons.Where(x => x.Code.Equals(discount) && x.StartDate <= DateTime.Now && x.EndDate > DateTime.Now && x.Uses > 0 && x.IsActive).SingleOrDefault();
            decimal coupdiscount = 0;
            var fee = db.CityShippingFees.Where(x => x.Id == customer.CityId).SingleOrDefault().ShippingFee;

            
            var specdeal = db.SpecialDeals.Where(x => x.IsActive && x.EndDate > DateTime.Now && x.StartDate < DateTime.Now);
            if (!User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrEmpty(data))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //create customer -> add order
                var cart_items = JsonConvert.DeserializeObject<List<LocalCartVM>>(data);
                if (cart_items == null || cart_items.Count == 0)
                {
                    return Content("Bay roi");
                }
                try
                {
                  
                    //create customer
                    Customer cus = new Customer()
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Phone = customer.Phone,
                        Address = customer.Address,
                        Email = customer.Email,
                        Birthday = customer.Birthday,
                        CityId = customer.CityId,
                        IsRegistered = false
                    };
                    db.Customers.Add(cus);
                    db.SaveChanges();
                    //add order
                    Order o = new Order()
                    {
                        Codenname = "C" + cus.Id + "-" + (DateTime.Now.ToString("ddMMyyhhmmss")).ToString(),
                        CreatedDate = DateTime.Now,
                        StatusId = 1,
                        Description = description ?? "",
                        PaymentType = payment ?? "COD",
                        CustomerId = cus.Id,
                        EmployeeId = null,
                        ShipmentId = null,
                        Discount = coupdiscount,
                        Tax = 10,
                        ShippingFee = fee,
                        ShippingAddress = customer.Address,
                        ShippingCity = customer.CityId
                    };
                    if (coupon != null)
                    {
                        coupon.Uses -= 1;
                        coupdiscount = coupon.Discount;
                    }
                    codename = o.Codenname;
                    maildiscount = o.Discount.ToString();
                    shipfee = o.ShippingFee ?? 0;
                    db.Orders.Add(o);
                    db.SaveChanges();
                    //add orderdetails
                    foreach (var c in cart_items)
                    {
                        var p = db.Products.Find(c.productid);
                        //
                        if (specdeal.Select(x => x.ProductId).Contains(c.productid))
                        {
                            price = p.Price * (100 - specdeal.Where(x => x.ProductId == c.productid).SingleOrDefault().Discount) / 100;
                        }
                        else
                        {
                            price = p.Price * (100 - p.Discount) / 100;
                        }
                        subtotal += price;
                        OrderDetail od = new OrderDetail()
                        {
                            OrderId = o.Id,
                            ProductId = c.productid,
                            Quantity = c.quantity,
                            ProductName = p.Name,
                            Description = null,
                            Price = price
                        };
                        maillist.Add(od);
                        db.OrderDetails.Add(od);
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Content(ex.ToString());
                }
                return Content("OK");
            }
            else
            {
                if (string.IsNullOrEmpty(data))
                {
                    data = "";
                }
                if (string.IsNullOrEmpty(description))
                {
                    description = "";
                }
            
                var id = int.Parse(User.Identity.Name);
               
                if (coupon != null)
                {
                    coupon.Uses -= 1;
                    coupdiscount = coupon.Discount;
                }
                // add order
                try
                {
                    
                    
                    Order o = new Order()
                    {
                        Codenname = "C" + customer.Id + "-" + (DateTime.Now.ToString("ddMMyyhhmmss")).ToString(),
                        CreatedDate = DateTime.Now,
                        StatusId = 1,
                        Description = description ?? "",
                        PaymentType = payment ?? "COD",
                        CustomerId = customer.Id,
                        EmployeeId = null,
                        ShipmentId = null,
                        Discount = coupdiscount,
                        Tax = 10,
                        ShippingFee = fee ,
                        ShippingAddress = customer.Address,
                        ShippingCity = customer.CityId
                    };
                    db.Orders.Add(o);
                    codename = o.Codenname;
                    maildiscount = o.Discount.ToString();
                    shipfee = o.ShippingFee ?? 0;
                    db.SaveChanges();
                    var cart = db.CartItems.Where(x=>x.Cart.UserId == id);
                   
                    
                    foreach (var c in cart)
                    {
                        
                        if (specdeal.Select(x => x.ProductId).Contains(c.ProductId))
                        {
                            price = c.Product.Price * (100 - specdeal.Where(x => x.ProductId == c.ProductId).SingleOrDefault().Discount) / 100 ;
                        }
                        else
                        {
                            price = c.Product.Price * (100 - c.Product.Discount) / 100 ;
                        }
                        subtotal += price ;
                        OrderDetail od = new OrderDetail()
                        {
                            OrderId = o.Id,
                            ProductId = c.ProductId,
                            Quantity = c.Quantity,
                            ProductName = c.Product.Name,
                            Description = null,
                            Price = price
                        };
                        db.OrderDetails.Add(od);
                        db.CartItems.Remove(c);
                        maillist.Add(od);
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                   
                    return Content(ex.ToString());
                }
            }
            var mailtablebody = "";
            decimal mailordertotal = 0;
            foreach(var a in maillist)
            {
                var aPrice = a.Price.Value.ToString("N0");
                mailtablebody += "<tr>"
                                + "<td> " + a.ProductName + "<strong> × " + a.Quantity + "</strong></td>"
                                + "<td><span>" + aPrice + " VND</span></td>"
                                + "</tr>";
                decimal p = a.Price ?? 0;
                mailordertotal += p * a.Quantity;
            }
          
            mailordertotal = (mailordertotal * (100 - int.Parse(maildiscount)) / 100) * 110 / 100 + shipfee;
            //send mail
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("anhkhuongsubmail@gmail.com");
            mail.To.Add(customer.Email);
            mail.Subject = "Your order detail";
            mail.Body = "Your order's codename is: " + codename + "<br>"
                + "<table>"
                + "<thead><tr><th>Product</th><th>Total</th></tr></thead>"
                + "<tbody>"
                + mailtablebody
                + "</tbody>"
                + "<tr><th>Discount </th><td><span>" + maildiscount + "</span></td></tr>"
                + "<tr><th>Total </th><td><span>" + mailordertotal.ToString("N0") + " VND</span></td></tr>";

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
        [HttpPost]
        public ActionResult CheckCoupon(string data)
        {
            var coupon = db.Coupons.Where(x => x.Code.Equals(data) && x.StartDate <= DateTime.Now && x.EndDate >DateTime.Now && x.Uses>0 && x.IsActive).SingleOrDefault();
            if(coupon == null)
            {
                return Content("Invalid");
            }
            
            return Content(coupon.Discount.ToString());
        }
        [HttpPost]
        public ActionResult getShippingFee(string data)
        {
            var id = int.Parse(data);
            var fee = db.CityShippingFees.Where(x => x.Id == id).SingleOrDefault();
            if (fee == null)
            {
                return Content("Invalid");
            }
            int rs = (int)fee.ShippingFee;
            return Content(rs.ToString());
        }
    }
}