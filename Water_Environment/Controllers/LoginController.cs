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
                    Session["Username"] = user.Username;
                    Session["UserId"] = userDb.id;
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