using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        DangNhapEntities database = new DangNhapEntities();

        // GET: Login
        public ActionResult Index()
        {
            var customers = database.Customers.ToList();
            return View(customers);
        }
    }
}