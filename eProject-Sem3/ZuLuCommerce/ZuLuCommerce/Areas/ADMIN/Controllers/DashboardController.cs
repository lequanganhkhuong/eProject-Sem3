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
          
            decimal total = 0;
            foreach (var o in income)
            {
                decimal sale = 0;
                var od = db.OrderDetails.Where(x => x.OrderId == o.Id);
                foreach (var d in od)
                {
                    decimal p = d.Price ?? 0;
                    sale += p * d.Quantity;
                }
                var shipfee = o.ShippingFee ?? 0;
                var dis = o.Discount ?? 0;
                sale = (sale * (100 - dis) / 100) * (o.Tax + 100) / 100 + shipfee;
                total += sale;
            }
            
            return total;
        }
        public decimal SupplierTotal(IQueryable<Order> order,int sup)
        {
         
            decimal total = 0;
            foreach (var o in order)
            {
                decimal sale = 0;
                var od = db.OrderDetails.Where(x => x.OrderId == o.Id && x.Product.SupplierId == sup);
                foreach (var d in od)
                {
                    decimal p = d.Price ?? 0;
                    sale += p * d.Quantity;
                }
                var shipfee = o.ShippingFee ?? 0;
                var dis = o.Discount ?? 0;
                sale = (sale * (100 - dis) / 100) * (o.Tax + 100) / 100 + shipfee;
                total += sale;
            }
            return total;
        }
        public decimal ProductTotal(IQueryable<Order> order, int pid)
        {

            decimal total = 0;
            foreach (var o in order)
            {
                decimal sale = 0;
                var od = db.OrderDetails.Where(x => x.OrderId == o.Id && x.ProductId == pid);
                foreach (var d in od)
                {
                    decimal p = d.Price ?? 0;
                    sale += p * d.Quantity;
                }
                var shipfee = o.ShippingFee ?? 0;
                var dis = o.Discount ?? 0;
                sale = (sale * (100 - dis) / 100) * (o.Tax + 100) / 100 + shipfee;
                total += sale;
            }
            return total;
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
            ViewBag.newUser = db.Accounts.Where(x => x.RegisterDate.Value.Month == DateTime.Now.Month).Count();
            ViewBag.activeSpecialdiscount = db.SpecialDeals.Where(x => x.IsActive && x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).Count();

            //sale per month this year chart data
            decimal[] saleThisYear = new decimal[12];
            var orderMonth = new List<int>();
            for(int i = 0; i < 12; i++)
            {
                var salePerMonth = db.Orders.Where(x => x.StatusId == 3 && x.Shipment.StatusId == 3 && x.Shipment.ShippedDate.Value.Month == (i + 1)  && x.Shipment.ShippedDate.Value.Year == DateTime.Now.Year);
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
                var saleIYear = db.Orders.Where(x => x.StatusId == 3 && x.Shipment.StatusId == 3 && x.Shipment.ShippedDate.Value.Year == y);
                decimal incomeIYear = Income(saleIYear);
                saleEachYear.Add(incomeIYear) ;
                orderEachYear.Add(saleIYear.Count());
            }
            ViewBag.years = lstYears;
            ViewBag.salePerYear = saleEachYear;
            ViewBag.orderEachYear = orderEachYear;
            //data for labels
            var saleyear = db.Orders.Where(x => x.StatusId == 3 && x.Shipment.StatusId == 3  && x.Shipment.ShippedDate.Value.Year == DateTime.Now.Year);
            decimal incomeyear = Income(saleyear);
            //decimal incomeyear = saleThisYear.Sum();
            var salemonth = db.Orders.Where(x => x.StatusId == 3  && x.Shipment.ShippedDate.Value.Month == DateTime.Now.Month && x.Shipment.ShippedDate.Value.Year == DateTime.Now.Year);
            decimal incomemonth = Income(salemonth);
            var salelastmonth = db.Orders.Where(x => x.StatusId == 3 && (DateTime.Now.Month - x.Shipment.ShippedDate.Value.Month ) == 1 && x.Shipment.ShippedDate.Value.Year == DateTime.Now.Year);
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
        [Authorize(Roles ="Admin,Manager")]
        public ActionResult Customers()
        {
            var highorder = db.Orders.Where(x=>x.StatusId == 3).GroupBy(x => x.CustomerId)
                .Select(x => new
                {
                    cus = x.Key,
                    Count = x.Count()
                }).OrderByDescending(x=>x.Count).Take(10);
            var list = new List<Tuple<int,string, decimal, int>>();
            
            foreach (var h in highorder)
            {
                var cus = db.Customers.Where(x => x.Id == h.cus).SingleOrDefault();
                var name = cus.FirstName + " " + cus.LastName;
                var sale = db.Orders.Where(x => x.StatusId == 3 && x.CustomerId == h.cus);
                decimal incomeIYear = Income(sale);
                list.Add(new Tuple<int, string, decimal, int>(h.cus.Value,name, incomeIYear,h.Count));
            }
            ViewBag.highpaycustomers = list;
            return View();
        }
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Products()
        {
            //total sale each supplier
            var saleSupplier = new List<decimal>();
            var saleSupplierThisMonth = new List<decimal>();
            var saleSupplierLastMonth = new List<decimal>();
            var supsale = db.OrderDetails.GroupBy(x => x.Product.SupplierId)
                         .Select(x => new
                         {
                             sup = x.Key,
                             Count = x.Count()
                         }).OrderByDescending(x => x.Count);
            foreach(var ss in supsale)
            {
                var sale = db.Orders.Where(x => x.StatusId == 3);
                decimal incomeIYear = SupplierTotal(sale, ss.sup);
                saleSupplier.Add(incomeIYear);

                var saleThisMonth = db.Orders.Where(x => x.StatusId == 3 && x.Shipment.ShippedDate.Value.Month == DateTime.Now.Month && x.Shipment.ShippedDate.Value.Year == DateTime.Now.Year);
                decimal incomeThisMonth = SupplierTotal(saleThisMonth, ss.sup);
                saleSupplierThisMonth.Add(incomeThisMonth);

                var saleLastMonth = db.Orders.Where(x => x.StatusId == 3 && (DateTime.Now.Month - x.Shipment.ShippedDate.Value.Month) == 1 && x.Shipment.ShippedDate.Value.Year == DateTime.Now.Year);
                decimal incomeLastMonth = SupplierTotal(saleLastMonth, ss.sup);
                saleSupplierLastMonth.Add(incomeLastMonth);
            }
            ViewBag.supplierTotalsale = saleSupplier;
            ViewBag.saleSupplierThisMonth = saleSupplierThisMonth;
            ViewBag.saleSupplierLastMonth = saleSupplierLastMonth;


            //best product tables
            var bestproduct = db.OrderDetails.GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    pid = x.Key,
                    Count = x.Count()
                }).OrderByDescending(x => x.Count).Take(10);
            var list = new List<Tuple<int, string, decimal, int>>();

            foreach (var h in bestproduct)
            {
                var p = db.Products.Where(x => x.Id == h.pid).SingleOrDefault();
                var name = p.Name;
                var sale = db.Orders.Where(x => x.StatusId == 3);
                decimal incomeIYear = ProductTotal(sale, h.pid);
                list.Add(new Tuple<int, string, decimal, int>(h.pid, name, incomeIYear, h.Count));
            }
            ViewBag.bestProducts = list;

            //worst product table
            var worstproducts = db.OrderDetails.GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    pid = x.Key,
                    Count = x.Count()
                }).OrderBy(x => x.Count).Take(10);
            var worstlist = new List<Tuple<int, string, decimal, int>>();

            foreach (var h in worstproducts)
            {
                var p = db.Products.Where(x => x.Id == h.pid).SingleOrDefault();
                var name = p.Name;
                var sale = db.Orders.Where(x => x.StatusId == 3);
                decimal incomeIYear = ProductTotal(sale, h.pid);
                worstlist.Add(new Tuple<int, string, decimal, int>(h.pid, name, incomeIYear, h.Count));
            }
            ViewBag.bestProducts = list;
            ViewBag.worstProducts = worstlist;
            return View();
        }
    }
}