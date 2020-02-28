using System.Web.Mvc;
using ProjectileMotionWeb.Models;
using System;
using ProjectileMotionWeb.Helpers;
using ProjectileMotionData;
using System.Collections.Generic;
using ProjectileMotionSource.WithResistance.Func;
using Utilities.Quantities;
using Utilities.Units;
using ProjectileMotionSource.Func;
using ProjectileMotionSource.Exceptions;

namespace ProjectileMotionWeb.Controllers
{
    public class SetController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Properties), new { setWithResistance = false });
        }


        [HttpGet]
        public ActionResult Properties(bool? setWithResistance =  null)
        {
            SetPropertiesModel viewModel = new SetPropertiesModel()
            {
                Layout = new LayoutModel("Set properties"),
                Quantities = new SetPropertiesQuantitiesModel()
                {
                    InitialVelocity = 15.0,
                    InitialHeight = 0,
                    ElevationAngle = Math.PI / 4.0,
                    GravAcceleration = GravAcceleration.GetGravAccelerationValue(GravAcceleration.GravAccelerations.Earth),
                    FrontalArea = Math.Pow(new Length(12.9, UnitLength.Centimetre).GetBasicVal(), 2.0) * Math.PI,
                    DragCoefficient = DragCoefficient.GetDragCoefficientValue(DragCoefficient.DragCoefficients.Sphere),
                    Mass = 0.5,
                    Density = Density.GetDensityValue(Density.Densities.Air),
                    WithResistance = setWithResistance ?? false,
                },
                RoundDigits = 6,
                PointsForTrajectory = 80,
                ShowMotionWithoutResistanceTrajectoryToo = false
            };

            viewModel.HexColorOfTrajectory = viewModel.Quantities.WithResistance ? "#007bff" : "#6c757d";

            SessionStore session = GetSession();

            if (session.IsSavedProjectileMotionWithResistance())
            {
                ProjectileMotionWithResistance savedMotion = session.GetSavedProjectileMotionWithResistance();

                viewModel.Quantities.InitialVelocity = savedMotion.Settings.Quantities.V.Val;
                viewModel.Quantities.InitialHeight = savedMotion.Settings.Quantities.H.Val;
                viewModel.Quantities.ElevationAngle = savedMotion.Settings.Quantities.Α.Val;
                viewModel.Quantities.GravAcceleration = savedMotion.Settings.Quantities.G.Val;
                viewModel.Quantities.FrontalArea = savedMotion.Settings.Quantities.A.Val;
                viewModel.Quantities.DragCoefficient = savedMotion.Settings.Quantities.C.Val;
                viewModel.Quantities.Mass = savedMotion.Settings.Quantities.M.Val;
                viewModel.Quantities.Density = savedMotion.Settings.Quantities.Ρ.Val;
                viewModel.RoundDigits = savedMotion.Settings.RoundDigits;
                viewModel.Quantities.WithResistance = true;

                viewModel.PointsForTrajectory = savedMotion.Settings.PointsForTrajectory;

                viewModel.Quantities.InitialVelocityUnit = savedMotion.Settings.Quantities.V.Unit.Name;
                viewModel.Quantities.InitialHeightUnit = savedMotion.Settings.Quantities.H.Unit.Name;
                viewModel.Quantities.ElevationAngleUnit = savedMotion.Settings.Quantities.Α.Unit.Name;
                viewModel.Quantities.GravAccelerationUnit = savedMotion.Settings.Quantities.G.Unit.Name;
                viewModel.Quantities.FrontalAreaUnit = savedMotion.Settings.Quantities.A.Unit.Name;
                viewModel.Quantities.MassUnit = savedMotion.Settings.Quantities.M.Unit.Name;
                viewModel.Quantities.DensityUnit = savedMotion.Settings.Quantities.Ρ.Unit.Name;

                viewModel.ResultUnitAngle = savedMotion.Settings.Quantities.Units.Angle.Name;
                viewModel.ResultUnitArea = savedMotion.Settings.Quantities.Units.Area.Name;
                viewModel.ResultUnitGravAcceleration = savedMotion.Settings.Quantities.Units.GravAcceleration.Name;
                viewModel.ResultUnitLength = savedMotion.Settings.Quantities.Units.Length.Name;
                viewModel.ResultUnitTime = savedMotion.Settings.Quantities.Units.Time.Name;
                viewModel.ResultUnitVelocity = savedMotion.Settings.Quantities.Units.Velocity.Name;

                viewModel.TxtInfoFileName = savedMotion.Settings.TxtInfoFileName;
                viewModel.CsvDataFileName = savedMotion.Settings.CsvDataFileName;
                viewModel.PdfInfoFileName = savedMotion.Settings.PdfInfoFileName;
                viewModel.HexColorOfTrajectory = savedMotion.Settings.HexColorOfTrajectory;
                viewModel.ShowMotionWithoutResistanceTrajectoryToo = savedMotion.Settings.ShowMotionWithoutResistanceTrajectoryToo;

                viewModel.Layout.Title = "Edit properties";
            }
            else if (session.IsSavedProjectileMotion())
            {
                ProjectileMotion savedMotion = session.GetSavedProjectileMotion();

                viewModel.Quantities.InitialVelocity = savedMotion.Settings.Quantities.V.Val;
                viewModel.Quantities.InitialHeight = savedMotion.Settings.Quantities.H.Val;
                viewModel.Quantities.ElevationAngle = savedMotion.Settings.Quantities.Α.Val;
                viewModel.Quantities.GravAcceleration = savedMotion.Settings.Quantities.G.Val;
                viewModel.Quantities.WithResistance = false;


                viewModel.RoundDigits = savedMotion.Settings.RoundDigits;

                viewModel.PointsForTrajectory = savedMotion.Settings.PointsForTrajectory;

                viewModel.Quantities.InitialVelocityUnit = savedMotion.Settings.Quantities.V.Unit.Name;
                viewModel.Quantities.InitialHeightUnit = savedMotion.Settings.Quantities.H.Unit.Name;
                viewModel.Quantities.ElevationAngleUnit = savedMotion.Settings.Quantities.Α.Unit.Name;
                viewModel.Quantities.GravAccelerationUnit = savedMotion.Settings.Quantities.G.Unit.Name;

                viewModel.Quantities.Length = savedMotion.GetLength().Val;
                viewModel.Quantities.LengthUnit = savedMotion.GetLength().Unit.Name;
                viewModel.Quantities.MaxHeight = savedMotion.GetMaxHeight().Val;
                viewModel.Quantities.MaxHeightUnit = savedMotion.GetMaxHeight().Unit.Name;
                viewModel.Quantities.Duration = savedMotion.GetDur().Val;
                viewModel.Quantities.DurationUnit = savedMotion.GetDur().Unit.Name;

                viewModel.ResultUnitAngle = savedMotion.Settings.Quantities.Units.Angle.Name;
                viewModel.ResultUnitArea = savedMotion.Settings.Quantities.Units.Area.Name;
                viewModel.ResultUnitGravAcceleration = savedMotion.Settings.Quantities.Units.GravAcceleration.Name;
                viewModel.ResultUnitLength = savedMotion.Settings.Quantities.Units.Length.Name;
                viewModel.ResultUnitTime = savedMotion.Settings.Quantities.Units.Time.Name;
                viewModel.ResultUnitVelocity = savedMotion.Settings.Quantities.Units.Velocity.Name;

                viewModel.TxtInfoFileName = savedMotion.Settings.TxtInfoFileName;
                viewModel.CsvDataFileName = savedMotion.Settings.CsvDataFileName;
                viewModel.PdfInfoFileName = savedMotion.Settings.PdfInfoFileName;
                viewModel.HexColorOfTrajectory = savedMotion.Settings.HexColorOfTrajectory;
                viewModel.ShowMotionWithoutResistanceTrajectoryToo = false;

                viewModel.Quantities.SelectedAssignmentType = savedMotion.Settings.Quantities.UsedAssignmentType;

                viewModel.Layout.Title = "Edit properties";
            }
            else if (setWithResistance == null)
            {
                return RedirectToAction(nameof(ChooseController.Neglection), "Choose");
            }

            viewModel.Layout.Menu.SetWithResistance = viewModel.Quantities.WithResistance;
            viewModel.Layout.Menu.ActiveMenuItem = LayoutMenuModel.ActiveNavItem.Set;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Properties(SetPropertiesModel postModel)
        {
            if (ModelState.IsValid)
            {
                SessionStore session = GetSession();

                InitialVelocity v = new InitialVelocity(postModel.Quantities.InitialVelocity, new ReflectionHelper(typeof(UnitVelocity)).GetValueOfStaticProperty(postModel.Quantities.InitialVelocityUnit) as UnitVelocity);
                InitialHeight h = new InitialHeight(postModel.Quantities.InitialHeight, new ReflectionHelper(typeof(UnitLength)).GetValueOfStaticProperty(postModel.Quantities.InitialHeightUnit) as UnitLength);
                GravAcceleration g = new GravAcceleration(postModel.Quantities.GravAcceleration, new ReflectionHelper(typeof(UnitGravAcceleration)).GetValueOfStaticProperty(postModel.Quantities.GravAccelerationUnit) as UnitGravAcceleration);
                ElevationAngle α = new ElevationAngle(postModel.Quantities.ElevationAngle, new UnitsReflectionHelper(typeof(UnitAngle)).GetValueOfStaticProperty(postModel.Quantities.ElevationAngleUnit) as UnitAngle);

                ProjectileMotionResultsUnits units = new ProjectileMotionResultsUnits()
                {
                    Length = new ReflectionHelper(typeof(UnitLength)).GetValueOfStaticProperty(postModel.ResultUnitLength) as UnitLength,
                    Time = new ReflectionHelper(typeof(UnitTime)).GetValueOfStaticProperty(postModel.ResultUnitTime) as UnitTime,
                    Velocity = new ReflectionHelper(typeof(UnitVelocity)).GetValueOfStaticProperty(postModel.ResultUnitVelocity) as UnitVelocity,
                    Angle = new ReflectionHelper(typeof(UnitAngle)).GetValueOfStaticProperty(postModel.ResultUnitAngle) as UnitAngle,
                    Area = new ReflectionHelper(typeof(UnitArea)).GetValueOfStaticProperty(postModel.ResultUnitArea) as UnitArea,
                    GravAcceleration = new ReflectionHelper(typeof(UnitGravAcceleration)).GetValueOfStaticProperty(postModel.ResultUnitGravAcceleration) as UnitGravAcceleration
                };

                if (postModel.Quantities.WithResistance)
                {
                    session.SaveProjectileMotionWithResistance(new ProjectileMotionWithResistance(
                    new ProjectileMotionWithResistanceSettings(
                       new ProjectileMotionWithResistanceQuantities(
                           v, α, h, g,
                           new Mass(postModel.Quantities.Mass.Value, new ReflectionHelper(typeof(UnitMass)).GetValueOfStaticProperty(postModel.Quantities.MassUnit) as UnitMass),
                           new Density(postModel.Quantities.Density.Value, new ReflectionHelper(typeof(UnitDensity)).GetValueOfStaticProperty(postModel.Quantities.DensityUnit) as UnitDensity),
                           new FrontalArea(postModel.Quantities.FrontalArea.Value, new ReflectionHelper(typeof(UnitArea)).GetValueOfStaticProperty(postModel.Quantities.FrontalAreaUnit) as UnitArea),
                           new DragCoefficient(postModel.Quantities.DragCoefficient.Value),
                           units
                       )
                   )
                    {
                        RoundDigits = postModel.RoundDigits,
                        PointsForTrajectory = postModel.PointsForTrajectory,
                        TxtInfoFileName = postModel.TxtInfoFileName,
                        CsvDataFileName = postModel.CsvDataFileName,
                        PdfInfoFileName = postModel.PdfInfoFileName,
                        HexColorOfTrajectory = postModel.HexColorOfTrajectory,
                        ShowMotionWithoutResistanceTrajectoryToo = postModel.ShowMotionWithoutResistanceTrajectoryToo

                })).SaveProjectileMotion(null);

                    return RedirectToAction(nameof(DisplayController.MotionWithResistance), "Display");
                }


                ProjectileMotionQuantities quantitiesWithoutResistance = null;

                try
                {
                    if (new List<ProjectileMotionQuantities.AssignmentsTypes>() {
                        ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByDuration,
                        ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByDuration,
                        ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByDuration
                    }.Contains(postModel.Quantities.SelectedAssignmentType))
                    {
                        Duration d = new Duration(postModel.Quantities.Duration.Value, new ReflectionHelper(typeof(UnitTime)).GetValueOfStaticProperty(postModel.Quantities.DurationUnit) as UnitTime);

                        switch (postModel.Quantities.SelectedAssignmentType)
                        {
                            case ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByDuration:
                                quantitiesWithoutResistance = new ProjectileMotionQuantities(d, v, h, g, units);
                                break;
                            case ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByDuration:
                                quantitiesWithoutResistance = new ProjectileMotionQuantities(d, α, v, g, units);
                                break;
                            case ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByDuration:
                                quantitiesWithoutResistance = new ProjectileMotionQuantities(d, α, h, g, units);
                                break;
                        }
                    }
                    else if (new List<ProjectileMotionQuantities.AssignmentsTypes>() {
                        ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByMaxHeight,
                        ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByMaxHeight,
                        ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByMaxHeight
                    }.Contains(postModel.Quantities.SelectedAssignmentType))
                    {
                        MaximalHeight maxH = new MaximalHeight(postModel.Quantities.MaxHeight.Value, new ReflectionHelper(typeof(UnitLength)).GetValueOfStaticProperty(postModel.Quantities.MaxHeightUnit) as UnitLength);

                        switch (postModel.Quantities.SelectedAssignmentType)
                        {
                            case ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByMaxHeight:
                                quantitiesWithoutResistance = new ProjectileMotionQuantities(maxH, v, h, g, units);
                                break;
                            case ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByMaxHeight:
                                quantitiesWithoutResistance = new ProjectileMotionQuantities(maxH, α, v, g, units);
                                break;
                            case ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByMaxHeight:
                                quantitiesWithoutResistance = new ProjectileMotionQuantities(maxH, α, h, g, units);
                                break;
                        }
                    }
                    else if (new List<ProjectileMotionQuantities.AssignmentsTypes>() {
                        ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByLength,
                        ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByLength,
                        ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByLength
                    }.Contains(postModel.Quantities.SelectedAssignmentType))
                    {
                        Length l = new Length(postModel.Quantities.Length.Value, new ReflectionHelper(typeof(UnitLength)).GetValueOfStaticProperty(postModel.Quantities.MaxHeightUnit) as UnitLength);

                        switch (postModel.Quantities.SelectedAssignmentType)
                        {
                            case ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleByLength:
                                quantitiesWithoutResistance = new ProjectileMotionQuantities(l, v, h, g, units);
                                break;
                            case ProjectileMotionQuantities.AssignmentsTypes.InitialHeightByLength:
                                quantitiesWithoutResistance = new ProjectileMotionQuantities(l, α, v, g, units);
                                break;
                            case ProjectileMotionQuantities.AssignmentsTypes.InitialVelocityByLength:
                                quantitiesWithoutResistance = new ProjectileMotionQuantities(l, α, h, g, units);
                                break;
                        }
                    }
                    else if (postModel.Quantities.SelectedAssignmentType == ProjectileMotionQuantities.AssignmentsTypes.ElevationAngleGetMaxRange)
                    {
                        quantitiesWithoutResistance = new ProjectileMotionQuantities(v, h, g, units);
                    }
                    else
                        quantitiesWithoutResistance = new ProjectileMotionQuantities(v, α, h, g, units);

                    session.SaveProjectileMotion(new ProjectileMotion(
                            new ProjectileMotionSettings(quantitiesWithoutResistance)
                            {
                                RoundDigits = postModel.RoundDigits,
                                PointsForTrajectory = postModel.PointsForTrajectory,
                                TxtInfoFileName = postModel.TxtInfoFileName,
                                CsvDataFileName = postModel.CsvDataFileName,
                                PdfInfoFileName = postModel.PdfInfoFileName,
                                HexColorOfTrajectory = postModel.HexColorOfTrajectory
                            })).SaveProjectileMotionWithResistance(null);

                    return RedirectToAction(nameof(DisplayController.Motion), "Display");
                }
                catch (UnableToComputeQuantityException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            postModel.Layout = new LayoutModel("Repair properties");
            postModel.Layout.Menu.SetWithResistance = postModel.Quantities.WithResistance;
            postModel.Layout.Menu.ActiveMenuItem = LayoutMenuModel.ActiveNavItem.Set;
            return View(postModel);
        }
    }
}