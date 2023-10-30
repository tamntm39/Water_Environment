using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            return View();
        }

        public PartialViewResult Navbar()
        {

            List<CategoryContent> lstCateContent = new List<CategoryContent>();
            List<Category> lstCate = _db.Categories.Where(x => x.IsActive).ToList();
            foreach (Category c in lstCate)
            {
                List<ContentOfCate> lstCOC = _db.ActivitiesAndNews
                    .Where(x => x.CategoryId == c.id && c.IsActive)
                    .Select(x => new ContentOfCate() { Title = x.Title, url = ("/ActivitiesNews/Content/" + x.id) })
                    .ToList();
                CategoryContent categoryContentNew = new CategoryContent()
                {
                    CateName = c.Name,
                    CateContent = lstCOC
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