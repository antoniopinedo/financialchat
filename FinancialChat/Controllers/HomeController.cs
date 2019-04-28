using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            bool authenticated = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

            if (!authenticated)
            {
                return Redirect("/Account/Login");
            }

            ViewBag.UserName = System.Web.HttpContext.Current.User.Identity.Name;

            return View();
        }
    }
}