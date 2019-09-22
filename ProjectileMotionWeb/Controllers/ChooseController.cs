using System.Web.Mvc;
using ProjectileMotionWeb.Models;
using ProjectileMotionData;

namespace ProjectileMotionWeb.Controllers
{
    public class ChooseController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Neglection));
        }

        public ActionResult Neglection()
        {
            return View(new ChooseModel() { Layout = new LayoutModel("Choose") { ActiveMenuItem = LayoutModel.ActiveNavItem.Choose } });
        }

        public ActionResult Choosen(bool withRezistance)
        {
            SessionStore session = GetSession();

            if (session.IsSavedProjectileMotion() && withRezistance)
            {
                session.SaveProjectileMotion(null);
            }
            else if (session.IsSavedProjectileMotionWithRezistance() && !withRezistance)
            {
                session.SaveProjectileMotionWithRezistance(null);
            }
            return RedirectToAction("Properties", "Set", new { setWithRezistance = withRezistance });
        }
    }
}