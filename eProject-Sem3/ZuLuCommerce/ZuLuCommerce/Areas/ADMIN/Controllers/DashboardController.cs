using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZuLuCommerce.Models;
namespace ZuLuCommerce.Areas.ADMIN.Controllers
{
    [Authorize(Roles = "Admin,Manager,Sale")]
    public class DashboardController : Controller
    {
        eCommerceEntities db = new eCommerceEntities();
        // GET: ADMIN/Dashboard
        public decimal Income(IQueryable<Order> income)
        {
            decimal sale = 0;
            foreach (var o in income)
            {
                var od = db.OrderDetails.Where(x => x.OrderId == o.Id);
                foreach (var d in od)
                {
                    decimal p = d.Price ?? 0;
                    sale += p * d.Quantity;
                }
                sale = (sale * (100 - o.Discount)/100) * (o.Tax + 100) / 100 + o.ShippingFee ?? 0;
            }
            return sale;
        }
        public ActionResult Index()
        {
            var checkcoupon = db.Coupons.Where(x => x.IsActive && x.EndDate < DateTime.Now || x.Uses == 0);
            foreach(var c in checkcoupon)
            {
                c.IsActive = false;
                
            }
            db.SaveChanges();

            ViewBag.orders = db.Orders.Count();
            ViewBag.comments = db.Comments.Count();
            ViewBag.users = db.Accounts.Count();
            ViewBag.pending = db.Orders.Where(x => x.StatusId == 1).Count();
            ViewBag.orderthismonth = db.Orders.Where(x => x.CreatedDate.Month == DateTime.Now.Month && x.CreatedDate.Year == DateTime.Now.Year).Count();
            ViewBag.stock = db.Products.Where(x => x.Stock <= 20).Count();
            ViewBag.coupon = db.Coupons.Where(x => x.IsActive && x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).Count();

            //sale per month this year chart data
            decimal[] saleThisYear = new decimal[12];
            var orderMonth = new List<int>();
            for(int i = 0; i < 12; i++)
            {
                var salePerMonth = db.Orders.Where(x => x.StatusId == 3 && (x.Shipment.ShippedDate ?? new DateTime()).Month == (i + 1)  && (x.Shipment.ShippedDate ?? new DateTime()).Year == DateTime.Now.Year);
                decimal saleThisMonth = Income(salePerMonth);
                saleThisYear[i] = saleThisMonth;
                orderMonth.Add(salePerMonth.Count());
            }
            ViewBag.salePerMonthThisYear = saleThisYear;
            ViewBag.orderMonth = orderMonth;

            //sale every year chart data
            var saleEachYear = new List<decimal>();
            var orderEachYear = new List<int>();
            var lstYears = db.Orders
            .Where(p => p.StatusId == 3).OrderBy(x=>x.Shipment.ShippedDate)
            .Select(p => p.Shipment.ShippedDate.Value.Year)
            .Distinct()
            .AsEnumerable()
            .ToList();
          
            foreach(var y in lstYears)
            {
                var saleIYear = db.Orders.Where(x => x.StatusId == 3 && (x.Shipment.ShippedDate ?? new DateTime()).Year == y);
                decimal incomeIYear = Income(saleIYear);
                saleEachYear.Add(incomeIYear) ;
                orderEachYear.Add(saleIYear.Count());
            }
            ViewBag.years = lstYears;
            ViewBag.salePerYear = saleEachYear;
            ViewBag.orderEachYear = orderEachYear;
            //data for labels
            var saleyear = db.Orders.Where(x => x.StatusId == 3 && x.Shipment.StatusId == 3  && (x.Shipment.ShippedDate ?? new DateTime()).Year == DateTime.Now.Year);
            //decimal incomeyear = Income(saleyear);
            decimal incomeyear = saleThisYear.Sum();
            var salemonth = db.Orders.Where(x => x.StatusId == 3  && (x.Shipment.ShippedDate ?? new DateTime()).Month == DateTime.Now.Month && (x.Shipment.ShippedDate ?? new DateTime()).Year == DateTime.Now.Year);
            decimal incomemonth = Income(salemonth);
            var salelastmonth = db.Orders.Where(x => x.StatusId == 3 && (DateTime.Now.Month - (x.Shipment.ShippedDate ?? new DateTime()).Month ) == 1 && (x.Shipment.ShippedDate ?? new DateTime()).Year == DateTime.Now.Year);
            decimal incomelastmonth = Income(salelastmonth);
            var totalsale = db.Orders.Where(x => x.StatusId == 3 && x.Shipment.StatusId == 3);
            decimal totalincome = Income(totalsale);
            //decimal totalincome = saleEachYear.Sum();
            ViewBag.saleyear = incomeyear.ToString("N0");
            ViewBag.salemonth = incomemonth.ToString("N0");
            ViewBag.salelastmonth = incomelastmonth.ToString("N0");
            ViewBag.totalsale = totalincome.ToString("N0");

            
            return View();
        }
    }
}