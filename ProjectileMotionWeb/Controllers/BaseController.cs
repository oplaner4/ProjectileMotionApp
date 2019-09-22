using ProjectileMotionData;
using System.Web.Mvc;

namespace ProjectileMotionWeb.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult DefaultRedirect()
        {
            return RedirectToAction("Index", "Choose");
        }


        public SessionStore GetSession ()
        {
            return new SessionStore(HttpContext);
        }


        public bool CheckForExistingMotion(out ActionResult redirect)
        {
            redirect = DefaultRedirect();
            return CheckForExistingMotion();
        }


        public bool CheckForExistingMotion()
        {
            return GetSession().IsSavedAnyMotion();
        }
    }
}