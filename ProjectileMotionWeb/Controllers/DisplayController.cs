using System.Web.Mvc;
using ProjectileMotionSource.Func;
using ProjectileMotionSource.WithRezistance.Func;
using ProjectileMotionWeb.Models;

namespace ProjectileMotionWeb.Controllers
{
    public class DisplayController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction(nameof(SetController.Properties), "Set", new { withRezistance = false });
        }

        [HttpGet]
        public ActionResult Motion(bool ShowLargerMotionChart = false)
        {
            ProjectileMotion motion = GetSession().GetSavedProjectileMotion();

            if (motion == null)
            {
                return DefaultRedirect();
            }

            return View(new DisplayMotionModel(motion, ShowLargerMotionChart) {
                Layout = new LayoutModel("Projectile motion") {
                    FluidContainer = true,
                    Menu = new LayoutMenuModel ()
                    {
                        ActiveMenuItem = LayoutMenuModel.ActiveNavItem.MotionDropdown,
                        SetWithRezistance = false
                    }
                },
            });
        }


        [HttpGet]
        public ActionResult MotionWithRezistance(bool ShowLargerMotionChart = false)
        {
            ProjectileMotionWithRezistance motionWithRezistance = GetSession().GetSavedProjectileMotionWithRezistance();

            if (motionWithRezistance == null)
            {
                return DefaultRedirect();
            }

            return View(new DisplayMotionWithRezistanceModel(motionWithRezistance, ShowLargerMotionChart)
            {
                Layout = new LayoutModel("Projectile motion with rezistance") {
                    FluidContainer = true,
                    Menu = new LayoutMenuModel()
                    {
                        ActiveMenuItem = LayoutMenuModel.ActiveNavItem.MotionDropdown,
                        SetWithRezistance = true
                    }
                }
            });
        }
    }
}