using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Water_Environment.Models;
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
            List<Permission> lstPermissions = _db.Permissions.ToList();
            return View(lstPermissions);
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
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();  
            return RedirectToAction("Index", "Home");  
        }
    }
}