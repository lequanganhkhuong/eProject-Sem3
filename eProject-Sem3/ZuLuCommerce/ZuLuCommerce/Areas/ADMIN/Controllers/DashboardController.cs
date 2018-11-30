using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZuLuCommerce.Areas.ADMIN.Controllers
{
    //[Authorize(Roles = "Admin,Manager")]
    public class DashboardController : Controller
    {
        // GET: ADMIN/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}