using System.Web.Mvc;
using ProjectileMotionSource.Func;
using ProjectileMotionSource.WithResistance.Func;
using ProjectileMotionWeb.Models;
using Utilities.Quantities;
using Utilities.Units;

namespace ProjectileMotionWeb.Controllers
{
    public class ExamplesController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Motions));
        }

        public ActionResult Motions ()
        {
            BaseModel viewModel = new BaseModel(new LayoutModel("Examples"));
            viewModel.Layout.Menu.ActiveMenuItem = LayoutMenuModel.ActiveNavItem.Examples;
            return View(viewModel);
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
                 PointsForTrajectory = 100,
                 ChartFileName = "Throwing tennis ball chart",
                 CsvDataFileName = "Throwing tennis ball data",
                 PdfInfoFileName = "Throwing tennis ball info",
                 TxtInfoFileName = "Throwing tennis ball info"
              }
            );

            GetSession().SaveProjectileMotionWithResistance(motion).SaveProjectileMotion(null);

            return RedirectToAction("MotionWithResistance", "Display");
        }

        [HttpGet]
        public ActionResult Boeing747()
        {
            ProjectileMotionWithResistance motion = new ProjectileMotionWithResistance(
              new ProjectileMotionWithResistanceSettings(
                 new ProjectileMotionWithResistanceQuantities(
                     new InitialVelocity(540, UnitVelocity.MilePerHour),
                     new ElevationAngle(ElevationAngle.ElevationAngleTypes.Horizontal),
                     new InitialHeight(7, UnitLength.Kilometre),
                     new GravAcceleration(GravAcceleration.GravAccelerations.Earth),
                     new Mass(363, UnitMass.Tonne),
                     new Density(Density.Densities.Air),
                     new FrontalArea(180, UnitArea.SquareMetre),
                     new DragCoefficient(0.03),
                     new ProjectileMotionResultsUnits()
                     {
                         Angle = UnitAngle.Degree
                     }
                 )
              )
              {
                  HexColorOfTrajectory = "#0039a6",
                  RoundDigits = 4,
                  PointsForTrajectory = 100,
                  ChartFileName = "Falling Boeing 747 chart",
                  CsvDataFileName = "Falling Boeing 747 data",
                  PdfInfoFileName = "Falling Boeing 747 info",
                  TxtInfoFileName = "Falling Boeing 747 info"
              }
            );

            GetSession().SaveProjectileMotionWithResistance(motion).SaveProjectileMotion(null);

            return RedirectToAction("MotionWithResistance", "Display");
        }
    }
}