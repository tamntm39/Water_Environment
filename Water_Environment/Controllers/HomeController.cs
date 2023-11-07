using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Water_Environment.Models;
using Water_Environment.Models.ActivitiesNews;
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
            NewAndActivitiesHome lstNewsActi = new NewAndActivitiesHome();
            List<ActivitiesAndNew> activitiesAndNews = _db.ActivitiesAndNews.ToList();
            lstNewsActi.NewsLatest = activitiesAndNews.OrderByDescending(x=>x.CreateOn).Take(3).ToList();
            lstNewsActi.NewsHot = activitiesAndNews.OrderByDescending(x=>x.ViewCount).Take(3).ToList();
            return View(lstNewsActi);
        }

        public PartialViewResult Navbar()
        {
            List<CategoryContent> lstCateContent = new List<CategoryContent>();
            List<Category> lstCate = _db.Categories.Where(x => x.IsActive).ToList();
            foreach (Category c in lstCate)
            {
                List<ActivitiesAndNew> lstActiNews = _db.ActivitiesAndNews
                    .Where(x => x.CategoryId == c.id)
                    .ToList();
                CategoryContent categoryContentNew = new CategoryContent()
                {
                    CateName = c.Name,
                    CateContent = lstActiNews
                };
                lstCateContent.Add(categoryContentNew);
            }
            return PartialView(lstCateContent);
        }
       
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult Synthetic()
        {
            return View();
        }
    }
}