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
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
    }
}