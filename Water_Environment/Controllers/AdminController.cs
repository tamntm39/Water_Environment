using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Water_Environment.Core;
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
                List<ChartItem> lstChartItem = _db.ActivitiesAndNews.Select(x=>new ChartItem()
                {
                    label = x.Title,
                    value = x.ViewCount
                }).ToList();
                return View(lstChartItem);
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
                _db.Entry(p).State = EntityState.Modified;
                _db.SaveChanges();
                rs.Success = true;
                rs.Message = "Sửa thành công";
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
            ViewBag.ListPermission = _db.Permissions.Where(x => x.IsActive == true).ToList();
            return View(lstUsers);
        }

        [HttpPost]
        public JsonResult EditUser(User p)
        {
            ApiResult rs = new ApiResult();
            try
            {
                User u = _db.Users.Find(p.id);
                if (u == null)
                {
                    rs.Success = false;
                    rs.Message = "Không tìm thấy tài khoản";
                }
                else
                {
                    if (_db.Users.Any(x => x.Email == p.Email) && u.Email != p.Email)
                    {
                        rs.Success = false;
                        rs.Message = "Email này đã tồn tại";
                    }
                    else
                    {
                        u.Email = p.Email;
                        u.UserPermission = p.UserPermission;
                        u.CreatedOn = DateTime.Now;
                        u.IsActive = p.IsActive;
                        _db.Entry(u).State = EntityState.Modified;
                        _db.SaveChanges();
                        rs.Success = true;
                        rs.Message = "Sửa thành công";
                    }
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
        [HttpPost]
        public JsonResult CreateUser(User p)
        {
            ApiResult rs = new ApiResult();
            try
            {
                if (_db.Users.Any(x => x.UserName == p.UserName || x.Email == p.Email))
                {
                    rs.Success = false;
                    rs.Message = "Username hoặc email này đã tồn tại";
                }
                else
                {
                    p.CreatedOn = DateTime.Now;
                    p.IsActive = true;
                    p.PassWord = Extension.GetMd5Hash("12345678");
                    _db.Entry(p).State = EntityState.Added;
                    _db.SaveChanges();
                    rs.Success = true;
                    rs.Message = "Thêm tài khoản thành công. Mật khẩu mặc định sẽ là 12345678";
                }
            }
            catch (Exception)
            {
                rs.Success = false;
                rs.Message = "Thêm thất bại. Có lỗi trong khi thêm tài khoản.";
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
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ListDonate()
        {
            List<DonateItem> lstDonates = _db.Donates.Include(x => x.User)
                .Select(x => new DonateItem()
                {
                    donate = x,
                    userNameDonate = x.User.UserName
                }).ToList();
            return View(lstDonates);
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
            activitiesAndNew.Img = "~/Content/img/user.png";
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
                if (actiNews.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(actiNews.UploadImage.FileName);
                    string extent = Path.GetExtension(actiNews.UploadImage.FileName);
                    filename = filename + extent;
                    actiNews.Img = "~/Content/img/" + filename;
                    actiNews.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), filename));
                }
                _db.Entry(actiNews).State = EntityState.Added;
                _db.SaveChanges();
                rs.Success = true;
                rs.Message = "Tạo thành công";

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
            // Initialize the actiNews object
            try
            {
                if (actiNews.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(actiNews.UploadImage.FileName);
                    string extent = Path.GetExtension(actiNews.UploadImage.FileName);
                    filename = filename + extent;
                    actiNews.Img = "~/Content/img/" + filename;
                    actiNews.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), filename));
                }
                actiNews.IsActive = true;
                actiNews.CreateOn = DateTime.Now;
                _db.Entry(actiNews).State = EntityState.Modified;
                _db.SaveChanges();
                rs.Success = true;
                rs.Message = "Sửa thành công";
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
        [HttpPost]
        public JsonResult DeleteActivitiesNews(int id)
        {
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

        [HttpPost]
        public JsonResult ResetPassword(int id)
        {
            ApiResult rs = new ApiResult();
            try
            {
                User u = _db.Users.Find(id);
                if (u == null)
                {
                    rs.Success = false;
                    rs.Message = "Không tìm thấy tài khoản này";
                } 
                else
                {
                    u.PassWord = Extension.GetMd5Hash("12345678");
                    _db.Entry(u).State = EntityState.Modified;
                    _db.SaveChanges();
                    rs.Success = true;
                    rs.Message = "Reset mật khẩu thành công. Mật khẩu mặc định sau reset là 12345678";
                }    
            }
            catch (Exception)
            {
                rs.Success = false;
                rs.Message = "Reset mật khẩu  thất bại. Có lỗi trong khi reset mật khẩu .";
            }
            return new JsonResult()
            {
                Data = rs,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}