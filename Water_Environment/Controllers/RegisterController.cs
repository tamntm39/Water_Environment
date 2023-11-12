using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Water_Environment.Models;
using Water_Environment.Models.Users;

namespace Water_Environment.Controllers
{
    public class RegisterController : Controller
    {
        Water_EnvironmentEntities _db = new Water_EnvironmentEntities();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (Session["UserLogined"] != null && (bool)Session["UserLogined"])
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserModel user)
        {
            if (ModelState.IsValid)
            {
                User userDb = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == user.Username.ToLower() || u.Email.ToLower() == user.Email.ToLower());
                if (userDb != null)
                {
                    ModelState.AddModelError("", "Tên tài khoản hoặc email này đã tồn tại!");
                }
                else 
                {
                    try
                    {
                        User userNew = new User()
                        {
                            UserName = user.Username,
                            PassWord = user.Password,
                            Email = user.Email,
                            IsActive = true,
                            UserPermission = 4,
                            CreatedOn = DateTime.Now
                        };
                        _db.Users.Add(userNew);
                        _db.SaveChanges();
                        return RedirectToAction("Index", "Login");

                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Đăng kí tài khoản thất bại!");
                    }
                }
            }
            return View();
        }
    }
}