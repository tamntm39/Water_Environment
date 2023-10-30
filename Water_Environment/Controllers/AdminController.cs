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
                //if (_db.Permissions.Any(x => x.Code == p.Code || x.Name == p.Name))
                //{
                //    rs.Success = false;
                //    rs.Message = "Tên quyền hoặc mã quyền này đã tồn tại";
                //}
                //else
                //{
                _db.Entry(p).State = EntityState.Modified;
                _db.SaveChanges();
                rs.Success = true;
                rs.Message = "Sửa thành công";
                //}
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
        public ActionResult CreateActivitiesNews()
        {
            ActivitiesAndNew activitiesAndNew = new ActivitiesAndNew();
            List<Category> lstCategories = _db.Categories.Where(x => x.IsActive).ToList();
            if (activitiesAndNew != null)
            {
                EditActitiviesNewModel model = new EditActitiviesNewModel()
                {
                    activitiesAndNew = activitiesAndNew,
                    categories = lstCategories
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("ManageActivitiesNews");
            }
        }
        public ActionResult EditActivitiesNews(int id)
        {
            ActivitiesAndNew activitiesAndNew = _db.ActivitiesAndNews.Find(id);
            List<Category> lstCategories = _db.Categories.Where(x => x.IsActive).ToList();
            if (activitiesAndNew != null)
            {
                EditActitiviesNewModel model = new EditActitiviesNewModel()
                {
                    activitiesAndNew = activitiesAndNew,
                    categories = lstCategories
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("ManageActivitiesNews");
            }
        }
        [HttpPost]
        public JsonResult AddActivitiesNews(ActivitiesAndNew actiNews)
        {
            ApiResult rs = new ApiResult();
            try
            {
                actiNews.IsActive = true;
                actiNews.CreateOn = DateTime.Now;
                actiNews.CreateBy = _db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name).id;
                _db.Entry(actiNews).State = EntityState.Added;
                _db.SaveChanges();
                rs.Success = true;
                rs.Message = "Tạo thành công";
                //}
            }
            catch (Exception)
            {
                rs.Success = false;
                rs.Message = "Tạo thất bại. Có lỗi trong khi tạo.";
            }
            return new JsonResult()
            {
                Data = rs,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        public JsonResult EditActivities(ActivitiesAndNew actiNews)
        {
            ApiResult rs = new ApiResult();
            try
            {
                //if (_db.ActivitiesAndNews.Any(x => x.Title == actiNews.Title && x.CategoryId == actiNews.CategoryId))
                //{
                //    rs.Success = false;
                //    rs.Message = "Tên bài viết này đã tồn tại";
                //}
                //else
                //{
                actiNews.IsActive = true;
                actiNews.CreateOn = DateTime.Now;
                _db.Entry(actiNews).State = EntityState.Modified;
                _db.SaveChanges();
                rs.Success = true;
                rs.Message = "Sửa thành công";
                //}
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
        [HttpDelete]
        public JsonResult DeleteActivitiesNews()
        {
            int id = 9;
            ApiResult rs = new ApiResult();
            try
            {
                ActivitiesAndNew activitiesAndNew = _db.ActivitiesAndNews.Find(id);
                if (activitiesAndNew != null)
                {
                    _db.Entry(activitiesAndNew).State = EntityState.Deleted;
                    _db.SaveChanges();
                    rs.Success = true;
                    rs.Message = "Xóa thành công";
                }
                else
                {
                    rs.Success = false;
                    rs.Message = "Không tìm thấy bài viết này";
                }
            }
            catch (Exception)
            {
                rs.Success = false;
                rs.Message = "Xóa thất bại. Có lỗi trong khi xóa.";
            }
            return new JsonResult()
            {
                Data = rs,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}