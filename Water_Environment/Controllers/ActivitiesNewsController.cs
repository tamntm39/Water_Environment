﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
                activities.ViewCount = activities.ViewCount + 1;
                _db.Entry(activities).State = EntityState.Modified;
                _db.SaveChanges();
                List<CommentItem> lstComments = _db.Comments.Include(x => x.User).Where(x => x.PostId == id).OrderByDescending(x => x.CreateOn).Select(x => new CommentItem()
                {
                    comment = x,
                    userComment = x.User.UserName
                }).Take(4).ToList();
                ViewBag.lstComments = lstComments;
                return View(activities);
            }
        }

    }

}