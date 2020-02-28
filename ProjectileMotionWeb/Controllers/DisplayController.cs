using System.Web.Mvc;
using ProjectileMotionSource.Func;
using ProjectileMotionSource.WithResistance.Func;
using ProjectileMotionWeb.Models;

namespace ProjectileMotionWeb.Controllers
{
    public class DisplayController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction(nameof(SetController.Properties), "Set", new { withResistance = false });
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
                        SetWithResistance = false
                    }
                },
            });
        }


        [HttpGet]
        public ActionResult MotionWithResistance(bool ShowLargerMotionChart = false)
        {
            ProjectileMotionWithResistance motionWithResistance = GetSession().GetSavedProjectileMotionWithResistance();

            if (motionWithResistance == null)
            {
                return DefaultRedirect();
            }

            return View(new DisplayMotionWithResistanceModel(motionWithResistance, ShowLargerMotionChart)
            {
                Layout = new LayoutModel("Projectile motion with resistance") {
                    FluidContainer = true,
                    Menu = new LayoutMenuModel()
                    {
                        ActiveMenuItem = LayoutMenuModel.ActiveNavItem.MotionDropdown,
                        SetWithResistance = true
                    }
                }
            });
        }
    }
}