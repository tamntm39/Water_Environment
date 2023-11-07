using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Water_Environment.Models;
using Water_Environment.Models.Users;

namespace Water_Environment.Controllers
{
    public class LoginController : Controller
    {
        Water_EnvironmentEntities _db = new Water_EnvironmentEntities();

        // GET: Login
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
                //    if (User.IsInRole("Admin"))
                //    {
                //        Response.Redirect("~/AdminController");

                //    }
                //    else if (User.IsInRole("User"))
                //    {
                //        Response.Redirect("~/UserController");
                //    }
            }
            else
            {
                return View();
            }
        }
        //public ActionResult Login()
        //{
        //    //var customers = database.Customers.ToList();
        //    return View();
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserModel user)
        {
            if (ModelState.IsValid)
            {
                User userDb = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == user.Username.ToLower() && u
               .PassWord == user.Password);
                if (userDb != null && userDb.UserPermission == 1)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    Session["User"] = user.Username;
                    var userRole = Session["UserRole"] as string;
                    var isUserAdmin = userDb.UserPermission == 1;
                    Session["Username"] = user.Username;

                    return RedirectToAction("Index", "Admin");
                }
                else if (userDb != null)
                {
                    Session["UserLogined"] = true;
                    Session["UserId"] = userDb.id;
                    Session["Username"] = user.Username;
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng !");
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}