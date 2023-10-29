using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Water_Environment.Models;
using Water_Environment.Models.ActivitiesNews;
using Water_Environment.Models.Custom;
using Water_Environment.Models.Users;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        Water_EnvironmentEntities _db = new Water_EnvironmentEntities();
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
        public ActionResult ManagePermission()
        {
            //.Where(x => x.IsActive)
            List<Permission> lstPermissions = _db.Permissions.ToList();
            return View(lstPermissions);
        }
        [HttpPost]
        public JsonResult AddPermission(Permission p)
        {
            ApiResult rs = new ApiResult();
            try
            {
                if (_db.Permissions.Any(x => x.Code == p.Code && x.Name == p.Name))
                {
                    rs.Success = false;
                    rs.Message = "Tên quyền hoặc mã quyền này đã tồn tại";
                }
                else
                {
                    p.IsActive = true;
                    _db.Entry(p).State = EntityState.Added;
                    _db.SaveChanges();
                    rs.Success = true;
                    rs.Message = "Thêm thành công";
                }
            }
            catch (Exception)
            {
                rs.Success = false;
                rs.Message = "Thêm thất bại. Có lỗi trong khi thêm.";
            }
            return new JsonResult()
            {
                Data = rs,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        public JsonResult EditPermission(Permission p)
        {
            ApiResult rs = new ApiResult();
            try
            {
                if (_db.Permissions.Any(x => x.Code == p.Code || x.Name == p.Name))
                {
                    rs.Success = false;
                    rs.Message = "Tên quyền hoặc mã quyền này đã tồn tại";
                }
                else
                {
                    _db.Entry(p).State = EntityState.Modified;
                    _db.SaveChanges();
                    rs.Success = true;
                    rs.Message = "Sửa thành công";
                }
            }
            catch (Exception)
            {
                rs.Success = false;
                rs.Message = "Sửa thất bại. Có lỗi trong khi sửa.";
            }


            return new JsonResult()
            {
                Data = rs,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult ManageUsers()
        {

            List<UserPermission> lstUsers = _db.Users
            .Include(z => z.Permission)
            .Select(x => new UserPermission
            {
                User = x,
                PermissionName = x.Permission.Name
            })
            .ToList();
            return View(lstUsers);
        }

        [HttpPost]
        public JsonResult EditUser(User p)
        {
            ApiResult rs = new ApiResult();
            try
            {
                if (_db.Users.Any(x => x.UserName == p.UserName && x.Email == p.Email && x.UserPermission == p.UserPermission && x.IsActive == p.IsActive && x.CreatedOn == p.CreatedOn))
                {
                    rs.Success = false;
                    rs.Message = "Tên quyền hoặc mã quyền này đã tồn tại";
                }
                else
                {
                    _db.Entry(p).State = EntityState.Modified;
                    _db.SaveChanges();
                    rs.Success = true;
                    rs.Message = "Sửa thành công";
                }
            }
            catch (Exception)
            {
                rs.Success = false;
                rs.Message = "Sửa thất bại. Có lỗi trong khi sửa.";
            }


            return new JsonResult()
            {
                Data = rs,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ManageCategories()
        {
            List<Category> lstCategories = _db.Categories.ToList();
            return View(lstCategories);
        }
        public ActionResult ManageActivitiesNews()
        {
            List<ActivitiesNewsCate> lstActiNews = _db.ActivitiesAndNews
          .Include(z => z.Category)
          .Select(x => new ActivitiesNewsCate
          {
              ActivitiesAndNew = x,
              CategoryName = x.Category.Name
          })
          .ToList();
            return View(lstActiNews);
        }
        public ActionResult EditActivitiesNews(int id)
        {
            ActivitiesAndNew activitiesAndNew = _db.ActivitiesAndNews.Find(id);
            if (activitiesAndNew != null)
            {
                return View(activitiesAndNew);
            }
            else
            {
                return RedirectToAction("ManageActivitiesNews");
            }
        }
        [HttpPost]
        public ActionResult EditActivitiesNews(ActivitiesAndNew actiNews)
        {
            var a = 2;
            return View();

            //ActivitiesAndNew activitiesAndNew = _db.ActivitiesAndNews.Find(id);
            //if (activitiesAndNew != null)
            //{
            //    return View(activitiesAndNew);
            //}
            //else
            //{
            //    return RedirectToAction("ManageActivitiesNews");
            //}
        }
    }
}