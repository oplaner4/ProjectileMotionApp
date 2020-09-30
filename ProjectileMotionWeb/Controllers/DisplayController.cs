using System.Web.Mvc;
using ProjectileMotionSource.Func;
using ProjectileMotionSource.WithResistance.Func;
using ProjectileMotionWeb.Models;
using Utilities.Quantities;
using Utilities.Units;

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

            return View(new DisplayMotionModel(motion, ShowLargerMotionChart)
            {
                Layout = new LayoutModel("Projectile motion")
                {
                    FluidContainer = true,
                    Menu = new LayoutMenuModel()
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
                Layout = new LayoutModel("Projectile motion with resistance")
                {
                    FluidContainer = true,
                    Menu = new LayoutMenuModel()
                    {
                        ActiveMenuItem = LayoutMenuModel.ActiveNavItem.MotionDropdown,
                        SetWithResistance = true
                    }
                }
            });
        }

        [HttpGet]
        public ActionResult TennisBall ()
        {
            ProjectileMotionWithResistance motion = new ProjectileMotionWithResistance(
              new ProjectileMotionWithResistanceSettings(
                 new ProjectileMotionWithResistanceQuantities(
                     new InitialVelocity(73.06, UnitVelocity.MetrePerSecond),
                     new ElevationAngle(20.0, UnitAngle.Degree),
                     new InitialHeight(130, UnitLength.Centimetre),
                     new GravAcceleration(GravAcceleration.GravAccelerations.Earth),
                     new Mass(56.7, UnitMass.Gram),
                     new Density(Density.Densities.Air),
                     new FrontalArea(4.9062, UnitArea.SquareInch),
                     new DragCoefficient(0.55),
                     new ProjectileMotionResultsUnits()
                     {
                         Angle = UnitAngle.Degree
                     }
                 )
              )
              { 
                 HexColorOfTrajectory = "#dcfd50",
                 RoundDigits = 4,
                 PointsForTrajectory = 100
              }
            );

            GetSession().SaveProjectileMotionWithResistance(motion).SaveProjectileMotion(null);

            return RedirectToAction(nameof(MotionWithResistance));
        }
    }
}