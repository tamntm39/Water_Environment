using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Water_Environment.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize]
        public ActionResult Index()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}