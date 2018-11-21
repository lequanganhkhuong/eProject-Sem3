using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ZuLuCommerce.Models;
using System.Net;

namespace ZuLuCommerce.Controllers
{
    public class ProductsController : Controller
    {
        eCommerceEntities db = new eCommerceEntities();

        // GET: Products
        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;
            var products = db.Products.OrderBy(x => x.Id);
            return View(products.ToPagedList(pageNumber, pageSize));
        }
        //GET:  Products/Category/id
        public ActionResult Category(int id)
        {
            var product = db.Products.Where(x => x.CategoryId == id);
            return View(product.ToList());
        }
        public ActionResult Search(string kw)
        {

            return View();
        }
        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
    }
}