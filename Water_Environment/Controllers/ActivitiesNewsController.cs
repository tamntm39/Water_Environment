using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Water_Environment.Models;
using Water_Environment.Models.Custom;

namespace Water_Environment.Controllers
{
    public class ActivitiesNewsController : Controller
    {
        Water_EnvironmentEntities _db = new Water_EnvironmentEntities();
        public ActionResult Content(int id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ActivitiesAndNew activities = _db.ActivitiesAndNews.Find(id);
            if (activities == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(activities);
            }
        } 
    }
}