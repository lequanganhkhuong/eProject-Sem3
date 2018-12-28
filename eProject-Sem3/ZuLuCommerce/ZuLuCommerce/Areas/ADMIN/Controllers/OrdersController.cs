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
    [Authorize]
    public class OrdersController : Controller
    {
        private eCommerceEntities db = new eCommerceEntities();
        public ActionResult CancelOrder(int id)
        {
            var order = db.Orders.Find(id);
            if(order == null)
            {
                return Content("Invalid");
            }
            else
            {
                order.Shipment.ShippedDate = DateTime.Now;
                order.Shipment.StatusId = 4;
                order.Shipment.Shipper.StatusId = 1;
                order.StatusId = 4;
                order.EmployeeId = int.Parse(User.Identity.Name);
                db.SaveChanges();
                return Content("OK");
            }
            
        }
        public ActionResult CompleteOrder(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return Content("Invalid");
            }
            else
            {
                order.Shipment.ShippedDate = DateTime.Now;
                order.Shipment.StatusId = 3;
                order.Shipment.Shipper.StatusId = 1;
                order.StatusId = 3;
                order.EmployeeId = int.Parse(User.Identity.Name);
                db.SaveChanges();
                return Content("OK");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateShipment([Bind(Include = "Id,StatusId,ShippedDate,ShippingAddress,ShippingCity,ShippingFee,ShipperId")] Shipment shipment, string orderid)
        {
            if (ModelState.IsValid)
            {
                var shipper = db.Shippers.Where(x => x.Id == shipment.ShipperId).SingleOrDefault();
                shipper.StatusId = 2;

                var order = db.Orders.Find(int.Parse(orderid));
                order.ShipmentId = shipment.Id;
                order.StatusId = 2;
                order.EmployeeId = int.Parse(User.Identity.Name);

                shipment.ShippedDate = null;
                shipment.StatusId = 2;
                db.Shipments.Add(shipment);
                db.SaveChanges();
                ViewBag.Message = "Success";
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.ShipmentStatuses, "Id", "Name", shipment.StatusId);
            ViewBag.ShipperId = new SelectList(db.Shippers, "Id", "FirstName", shipment.ShipperId);
            return View(shipment);
        }
        public decimal Income(Order income)
        {

          
            
            
                decimal sale = 0;
                var od = db.OrderDetails.Where(x => x.OrderId == income.Id);
                foreach (var d in od)
                {
                    decimal p = d.Price ?? 0;
                    sale += p * d.Quantity;
                }
                var shipfee = income.ShippingFee ?? 0;
                var dis = income.Discount ?? 0;
                sale = (sale * (100 - dis) / 100) * (income.Tax + 100) / 100 + shipfee;
            
            

            return sale;
        }
        // GET: ADMIN/Orders
        public ActionResult Index(int? page, string kw, string sort, string status,string date,string pay, string city)
        {
            int pageNumber = page ?? 1;
            int pageSize = 20;
            var orders = db.Orders.OrderBy(x=>x.Id).Include(o => o.CityShippingFee).Include(o => o.Customer).Include(o => o.Employee).Include(o => o.OrderStatus).Include(o => o.Shipment);

            ViewBag.total = orders.Count();
            ViewBag.pending = orders.Where(x => x.StatusId == 1).Count();
            ViewBag.delivering = orders.Where(x => x.StatusId == 2).Count();
            ViewBag.completed = orders.Where(x => x.StatusId == 3).Count();
            ViewBag.canceled = orders.Where(x => x.StatusId == 4).Count();
            //filter status 
            if (string.IsNullOrEmpty(status))
            {
                ViewBag.status = "Total";
                status = "Total";
            }
            else
            {
                switch (status)
                {
                    case "Total":
                        orders = orders.Where(x => x.StatusId == 1 || x.StatusId == 2 || x.StatusId == 3 || x.StatusId == 4);
                        break;
                    case "Pending":
                        orders = orders.Where(x => x.StatusId == 1);
                        break;
                    case "Delivering":
                        orders = orders.Where(x => x.StatusId == 2);
                        break;
                    case "Completed":
                        orders = orders.Where(x => x.StatusId == 3);
                        break;
                    case "Canceled":
                        orders = orders.Where(x => x.StatusId == 4);
                        break;

                }
                ViewBag.status = status;
            }
            // city
            if (string.IsNullOrEmpty(city))
            {
                ViewBag.city = "allcity";
                city = "allcity";
            }
            else
            {
                ViewBag.city = city;
            }
            if (!city.Equals("allcity"))
            {
                int c = int.Parse(city);
                orders = orders.Where(x => x.CityShippingFee.Id == c);

            }
            //pay
            if (string.IsNullOrEmpty(pay))
            {
                ViewBag.pay = "all";
                pay = "all";
            }
            else
            {
                ViewBag.pay = pay;
            }
            if (!pay.Equals("all"))
            {
                switch (pay)
                {
                    case "cod":
                        orders = orders.Where(x => x.PaymentType.Equals("COD"));
                        break;
                    case "credit":
                        orders = orders.Where(x => x.PaymentType.Equals("Credit card"));
                        break;
                }
               

            }

            //date filter 
            ViewBag.date = "";
            if (!string.IsNullOrEmpty(date))
            {
                DateTime d = DateTime.ParseExact(date,"MM/dd/yyyy",null);
                orders = orders.Where(x => DbFunctions.DiffHours(x.CreatedDate, d) < 24);
           
                ViewBag.date = d.ToString("MM/dd/yyyy");
            }
            //search
            if (!string.IsNullOrEmpty(kw))
            {
                orders = orders.Where(x => x.ShipmentId.ToString().Equals(kw) || x.Codenname.ToLower().Contains(kw.ToLower()));
                ViewBag.kw = kw;
            }
            //sort


            if (string.IsNullOrEmpty(sort))
            {
                ViewBag.sort = "new";
                sort = "new";
            }
            else
            {
                ViewBag.sort = sort;
            }
            switch (sort)
            {
                case "id_asc":
                    orders = orders.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    orders = orders.OrderByDescending(x => x.Id);
                    break;
               
                case "old":
                    orders = orders.OrderBy(x => x.CreatedDate);
                    break;
                case "new":
                    orders = orders.OrderByDescending(x => x.CreatedDate);
                    break;
            }
            
            int start = 1;
            int end = pageSize;
            int max = orders.Count();
            if (pageNumber > 1)
            {
                start = start + pageSize * (pageNumber - 1);
                end = end * pageNumber;
            }
            if (end > max)
            {
                end = max;
            }
            if(max == 0)
            {
                start = 0;
            }
            ViewBag.resultcount = max;
            ViewBag.from = start;
            ViewBag.to = end;

            var list = new List<Tuple<int, decimal>>();
            foreach (var o in orders)
            {
                decimal sale = Income(o);
                list.Add(new Tuple<int, decimal>(o.Id, sale));
            }
            ViewBag.orderTotal = list;
            return View(orders.ToPagedList(pageNumber, pageSize));
            
        }

        // GET: ADMIN/Orders1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var orderitems = db.OrderDetails.Where(x => x.OrderId == id);
            ViewBag.StatusId = new SelectList(db.ShipmentStatuses, "Id", "Name");
            ViewBag.ShipperId = new SelectList(db.Shippers.Where(x=>x.StatusId == 1 && x.IsActive), "Id", "Id");
            return View(Tuple.Create<Order,IEnumerable<OrderDetail>>(order,orderitems.ToList()));
        }


        // GET: ADMIN/Orders1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            if(order.StatusId == 3 || order.StatusId == 4)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }
            ViewBag.ShippingCity = new SelectList(db.CityShippingFees, "Id", "CityName", order.ShippingCity);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", order.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", order.EmployeeId);
            ViewBag.StatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.StatusId);
            ViewBag.ShipmentId = new SelectList(db.Shipments, "Id", "ShippingAddress", order.ShipmentId);
            return View(order);
        }

        // POST: ADMIN/Orders1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codenname,CreatedDate,StatusId,Description,PaymentType,CustomerId,EmployeeId,ShipmentId,Discount,Tax,ShippingFee,ShippingAddress,ShippingCity")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShippingCity = new SelectList(db.CityShippingFees, "Id", "CityName", order.ShippingCity);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", order.CustomerId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", order.EmployeeId);
            ViewBag.StatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.StatusId);
            ViewBag.ShipmentId = new SelectList(db.Shipments, "Id", "ShippingAddress", order.ShipmentId);
            return View(order);
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
