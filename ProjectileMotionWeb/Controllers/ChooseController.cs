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
            return View(new ChooseModel() { Layout = new LayoutModel("Choose", new LayoutMenuModel (LayoutMenuModel.ActiveNavItem.Choose) ) });
        }

        public ActionResult Choosen(bool withResistance)
        {
            SessionStore session = GetSession();

            if (session.IsSavedProjectileMotion() && withResistance)
            {
                session.SaveProjectileMotion(null);
            }
            else if (session.IsSavedProjectileMotionWithResistance() && !withResistance)
            {
                session.SaveProjectileMotionWithResistance(null);
            }
            return RedirectToAction("Properties", "Set", new { setWithResistance = withResistance });
        }
    }
}