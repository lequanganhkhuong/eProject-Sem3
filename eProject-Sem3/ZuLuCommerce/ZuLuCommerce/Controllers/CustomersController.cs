using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZuLuCommerce.Controllers
{
    public class CustomersController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }
    }
}