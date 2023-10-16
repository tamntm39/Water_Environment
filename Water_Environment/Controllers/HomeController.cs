﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Water_Environment.Models;
using Water_Environment.Models.Home;
using Water_Environment.Models.Users;
using static Water_Environment.Models.Home.CategoryContent;

namespace Water_Environment.Controllers
{
    public class HomeController : Controller
    {
        Water_EnvironmentEntities _db = new Water_EnvironmentEntities();
        public ActionResult Index()
        {
            List<CategoryContent> lstCateContent = new List<CategoryContent>();
            List<Category> lstCate = _db.Categories.Where(x=>x.IsActive).ToList();
            foreach (Category c in lstCate)
            {
                List<ContentOfCate> lstCOC = _db.ActivitiesAndNews
                    .Where(x => x.CategoryId == c.id && c.IsActive)
                    .Select(x => new ContentOfCate() { Title = x.Title, url = x.id.ToString() })
                    .ToList();
                CategoryContent categoryContentNew = new CategoryContent()
                {
                    CateName = c.Name,
                    CateContent = lstCOC
                };
                lstCateContent.Add(categoryContentNew);
            }
            return View(lstCateContent);
        }
    }
}