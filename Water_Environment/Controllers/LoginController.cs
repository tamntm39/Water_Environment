using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Water_Environment.Models;

namespace Water_Environment.Controllers
{
    public class LoginController : Controller
    {
        Water_EnvironmentEntities database = new Water_EnvironmentEntities();

        // GET: Login
        public ActionResult Index()
        {
            //var customers = database.Customers.ToList();
            return View();
        }
    }
}