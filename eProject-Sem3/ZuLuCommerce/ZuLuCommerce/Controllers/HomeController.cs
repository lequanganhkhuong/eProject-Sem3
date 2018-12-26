using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZuLuCommerce.Models;

namespace ZuLuCommerce.Controllers
{
    public class HomeController : Controller
    {
        eCommerceEntities db = new eCommerceEntities();
        public ActionResult Index()
        {
            
            ViewBag.Title = "Home Page";
            ViewBag.banner = db.BannerProducts.Where(x=>x.PictureUrl != null && x.Product.IsActive).ToList();
            ViewBag.bestselling = db.BestSellings.Where(x => x.Product.IsActive).ToList();
            ViewBag.recommend = db.RecommendProducts.Where(x => x.Product.IsActive).ToList();
            ViewBag.newarrival = db.NewProducts.Where(x => x.Product.IsActive && x.IsActive).ToList();
            ViewBag.specdeal = db.SpecialDeals.Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now && x.IsActive && x.Product.IsActive);
            return View();
        }
    }
}
