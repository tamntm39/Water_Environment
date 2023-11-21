using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Water_Environment.Core;
using Water_Environment.Models;
using Water_Environment.Models.ActivitiesNews;
using Water_Environment.Models.Custom;
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
            lstNewsActi.NewsLatest = activitiesAndNews.OrderByDescending(x => x.CreateOn).Take(3).ToList();
            lstNewsActi.NewsHot = activitiesAndNews.OrderByDescending(x => x.ViewCount).Take(3).ToList();
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
        public ActionResult Search(string keySearch)
        {
            if (string.IsNullOrEmpty(keySearch))
            {
                return RedirectToAction("Index", "Home");
            }
            keySearch = keySearch.ToLower().NonUnicode();
            IEnumerable<ActivitiesAndNew> lstNews = _db.ActivitiesAndNews.ToList();
            List<ActivitiesAndNew> activitiesAndNews = lstNews.Where(x => x.Title.NonUnicode().ToLower().Contains(keySearch)).ToList();
            return View(activitiesAndNews);
        }
        public ActionResult InfoUser()
        {
            if (Session["UserLogined"] != null && (bool)Session["UserLogined"])
            {
                User u = _db.Users.Find(Session["UserId"]);
                if (u == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.lstDonate = _db.Donates.Where(x => x.IsDonateSuccess && x.UserId == u.id).ToList();
                    return View(u);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InfoUser(User user)
        {
            if (ModelState.IsValid)
            {
                User userDb = _db.Users.Find(user.id);
                if (userDb == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    try
                    {
                        if (_db.Users.Any(x => x.Email == user.Email) && userDb.Email != user.Email)
                        {
                            ModelState.AddModelError("", "Email này đã tồn tại!");
                        }
                        else
                        {
                            userDb.Email = user.Email;
                            if (Extension.GetMd5Hash(user.PassWord ?? "") != userDb.PassWord && !string.IsNullOrEmpty(user.PassWord))
                            {
                                userDb.PassWord = Extension.GetMd5Hash(user.PassWord);
                            }
                            _db.Entry(userDb).State = EntityState.Modified;
                            _db.SaveChanges();
                            ModelState.AddModelError("", "Cập nhật thành công!");
                            return View(userDb);
                        }
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View(userDb);
            }
            return View();
        }
        [HttpPost]
        public JsonResult Comment(Comment c)
        {
            ApiResult rs = new ApiResult();
            if (Session["UserLogined"] != null && (bool)Session["UserLogined"])
            {
                try
                {
                    c.CreateOn = DateTime.Now;
                    c.UserComment = (int)Session["UserId"];
                    _db.Entry(c).State = EntityState.Added;
                    _db.SaveChanges();
                    rs.Success = true;
                    rs.Message = "Thêm thành công";
                }
                catch (Exception)
                {
                    rs.Success = false;
                    rs.Message = "Thêm thất bại. Có lỗi trong khi thêm.";
                }
            }
            else
            {
                rs.Success = false;
                rs.Message = "Thất bại";
            }
            return new JsonResult()
            {
                Data = rs,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult Synthetic()
        {
            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }
    }
}