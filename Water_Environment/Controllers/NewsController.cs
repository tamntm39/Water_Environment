using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Water_Environment.Models;

namespace Water_Environment.Controllers
{
    public class NewsController : Controller
    {
        Water_EnvironmentEntities db = new Water_EnvironmentEntities();
        public ActionResult Index()
        {
            return View();
        }
        // GET: UpdateNew
        public ActionResult Update()
        {
            return View();
        }
        public ActionResult BaiBao()
        {
            return View();
        }
        public ActionResult Story()
        {
            return View();
        }
    }
}