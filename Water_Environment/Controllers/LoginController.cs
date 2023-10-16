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
                bool IsValidUser = _db.Users
               .Any(u => u.UserName.ToLower() == user
               .Username.ToLower() && u
               .PassWord == user.Password);
                if (IsValidUser)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("Index", "Admin");
                }
            }
            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng !");
            return View();
        }
    }
}