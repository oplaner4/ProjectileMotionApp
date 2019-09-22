using System;
using Utilities.Quantities;
using Utilities.Units;
using System.Collections.Generic;
using System.Linq;
using ProjectileMotionSource.Func;
using ProjectileMotionSource.Exceptions;
using ProjectileMotionSource.Saving;
using ProjectileMotionSource.Point;
using ProjectileMotionSource.WithRezistance.PointsComputation;

namespace ProjectileMotionSource.WithRezistance.Func
{
    public class ProjectileMotionWithRezistanceQuantities : ProjectileMotionQuantities
    {
        public ProjectileMotionWithRezistanceQuantities(InitialVelocity v, ElevationAngle α, InitialHeight h, GravAcceleration g, Mass m, Density ρ, FrontalArea a, DragCoefficient c, ProjectileMotionResultsUnits units = null) : base(v, α, h, g, units)
        {
            M = m;
            Ρ = ρ;
            C = c;
            A = a;
        }

        public Mass M { get; private set; }

        public Density Ρ { get; private set; }

        public DragCoefficient C { get; private set; }

        public FrontalArea A { get; private set; }
    }

    public class ProjectileMotionWithRezistanceSettings : ProjectileMotionSettings
    {
        private new string GetDefaultFileName(string extension)
        {
            return string.Format(
                "ProjectileMotionWithRezistance-{0} {1}-{2} {3}-{4} {5}-{6} {7}-{8} {9} {10} {11}.{12}",
                Quantities.Α.GetRoundedVal(RoundDigits).ToString(),
                Quantities.Α.Unit.Name,
                Quantities.V.GetRoundedVal(RoundDigits).ToString(),
                Quantities.V.Unit.Name,
                Quantities.H.GetRoundedVal(RoundDigits).ToString(),
                Quantities.H.Unit.Name,
                Quantities.A.GetRoundedVal(RoundDigits).ToString(),
                Quantities.A.Unit.Name,
                Quantities.M.GetRoundedVal(RoundDigits).ToString(),
                Quantities.M.Unit.Name,
                Quantities.Ρ.GetRoundedVal(RoundDigits).ToString(),
                Quantities.Ρ.Unit.Name,
                extension
            );
        }


        /// <summary>
        /// Gets the name for chart to be exported with the specified extension.
        /// </summary>
        public override string GetChartFileNameForExport(string extension)
        {
            return GetDefaultFileName(extension);
        }


        public ProjectileMotionWithRezistanceSettings(ProjectileMotionWithRezistanceQuantities quantities) : base(quantities)
        {
            Quantities = quantities;

            TxtInfoFileName = GetDefaultFileName("txt");
            CsvDataFileName = GetDefaultFileName("csv");
        }

        public new ProjectileMotionWithRezistanceQuantities Quantities { get; private set; }
    }

    public class ProjectileMotionWithRezistance : ProjectileMotion
    {
        public ProjectileMotionWithRezistance(ProjectileMotionWithRezistanceSettings settings) : base(settings)
        {
            Settings = settings;
            Saving = new ProjectileMotionFilesSaving(this);
            _ListFunctionCourseAllPoints = new List<ProjectileMotionPoint>();
        }

        public override Length GetLength()
        {
            return GetLastPoint().Round().X.Convert(Settings.Quantities.Units.Length);
        }


        public override Time GetDur()
        {
            return GetLastPoint().Round().T.Convert(Settings.Quantities.Units.Time);
        }

        public override Time GetTimeHighest()
        {
            return GetHighestPoint().Round().T.Convert(Settings.Quantities.Units.Time);
        }

        public override double[] GetCoordsHighest()
        {
            return GetHighestPoint().Round().GetCoords(Settings.Quantities.Units.Length);
        }

        public override double[] GetCoords(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().GetCoords(Settings.Quantities.Units.Length);
        }

        public override Length GetX(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().X.Convert(Settings.Quantities.Units.Length);
        }


        public override Length GetY(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().Y.Convert(Settings.Quantities.Units.Length);
        }


        public override double[] GetCoordsFallen()
        {
            return GetLastPoint().Round().GetCoords(Settings.Quantities.Units.Length);
        }


        public override Velocity GetVelocityX()
        {
            throw new OnlySuperClassMethodException("This method cannot be used for motions with rezistance");
        }


        public Velocity GetVelocityX(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().Vx.Convert(Settings.Quantities.Units.Velocity);
        }


        public override Velocity GetVelocityY(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().Vy.Convert(Settings.Quantities.Units.Velocity);
        }


        public override Velocity GetVelocity(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).GetVelocity(Settings.Quantities.Units.Velocity).RoundVal(Settings.RoundDigits);
        }


        public override Length GetDistance(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).GetDistance(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        public override Length GetDistanceBetweenTwoPoints(double t1, double t2)
        {
            return GetPoint(new Time(t1, Settings.Quantities.Units.Time)).GetDistanceFromPoint(GetPoint(new Time(t2, Settings.Quantities.Units.Time)), Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }

        public override Time GetTimeFallen()
        {
            return GetDur();
        }


        public override Length GetMaxHeight()
        {
            return GetHighestPoint().Round().Y.Convert(Settings.Quantities.Units.Length);
        }


        public override Time GetTimeFarthest()
        {
            return GetFarthestPoint().Round().T.Convert(Settings.Quantities.Units.Time);
        }


        public override Length GetMaxDistance()
        {
            return GetFarthestPoint().GetDistance(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        public override double[] GetCoordsFarthest()
        {
            return GetFarthestPoint().Round().GetCoords(Settings.Quantities.Units.Length);
        }


        public override Area GetAreaUnderArc()
        {
            return GetAreaUnderArc(Settings.Quantities.Units.Area).RoundVal(Settings.RoundDigits);
        }


        private Area _AreaUnderArc { get; set; }

        private new Area GetAreaUnderArc(UnitArea unitArea)
        {
            if (_AreaUnderArc == null)
            {
                ProjectileMotionPoint prevPoint = ProjectileMotionWithRezistanceComputation.Start(this).Point;
                double a = 0;
                foreach (ProjectileMotionPoint point in GetListFunctionCourseAllPoints())
                {
                    a += (point.Y.GetBasicVal() + prevPoint.Y.GetBasicVal()) * (point.X.GetBasicVal() - prevPoint.X.GetBasicVal()) / 2.0;
                    prevPoint = point;
                }

                _AreaUnderArc = new Area(a, UnitArea.Basic).Convert(unitArea);
            }

            return _AreaUnderArc.Convert(unitArea);
        }


        public override Length GetArcLength()
        {
            return GetArcLength(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        private Length _ArcLength { get; set; }

        private new Length GetArcLength(UnitLength unitLength)
        {
            if (_ArcLength == null)
            {
                ProjectileMotionPoint prevPoint = ProjectileMotionWithRezistanceComputation.Start(this).Point;
                double l = 0;
                foreach (ProjectileMotionPoint point in GetListFunctionCourseAllPoints())
                {
                    l += point.GetDistanceFromPoint(prevPoint, UnitLength.Basic).Val;
                    prevPoint = point;
                }

                _ArcLength = new Length(l, UnitLength.Basic).Convert(unitLength);
            }

            return _ArcLength;
        }


        private ProjectileMotionPoint _HighestPoint { get; set; }

        private ProjectileMotionPoint GetHighestPoint()
        {
            if (_HighestPoint == null)
            {
                _HighestPoint = ProjectileMotionWithRezistanceComputation.Start(this).Point;
                Length y = _HighestPoint.Y;
                foreach (ProjectileMotionPoint point in GetListFunctionCourseAllPoints())
                {
                    if (point.Y >= y) {
                        y = point.Y;
                        _HighestPoint = point;
                    }
                    else return _HighestPoint;
                }
            }

            return _HighestPoint;
        }


        private ProjectileMotionPoint _FarthestPoint { get; set; }

        private ProjectileMotionPoint GetFarthestPoint()
        {
            if (_FarthestPoint == null)
            {
                _FarthestPoint = ProjectileMotionWithRezistanceComputation.Start(this).Point;
                double d = _FarthestPoint.GetDistance(UnitLength.Basic).Val;
                foreach (ProjectileMotionPoint point in GetListFunctionCourseAllPoints())
                {
                    if (point.GetDistance(UnitLength.Basic).Val >= d) {
                        d = point.GetDistance(UnitLength.Basic).Val;
                        _FarthestPoint = point;
                    }
                    else return _FarthestPoint;
                }
            }

            return _FarthestPoint;
        }

        private ProjectileMotionPoint GetLastPoint()
        {
            return GetListFunctionCourseAllPoints().Last();
        }

        public override ProjectileMotionPoint GetPoint(Time t)
        {
            if (t >= GetDur())
            {
                return GetLastPoint();
            }
            return GetListFunctionCourseAllPoints().Where(x => t.GetBasicVal() - x.T.GetRoundedVal(Settings.RoundDigits) <= ProjectileMotionWithRezistanceComputation.Dt).First();

        }

        public override ProjectileMotionPoint GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes type)
        {
            throw new OnlySuperClassMethodException("This method cannot be used for motions with rezistance");
        }

        private List<ProjectileMotionPoint> _ListFunctionCourseAllPoints { get; set; }

        private List<ProjectileMotionPoint> GetListFunctionCourseAllPoints()
        {
            if (!_ListFunctionCourseAllPoints.Any())
            {
                ProjectileMotionWithRezistanceComputation computation = ProjectileMotionWithRezistanceComputation.Start(this);
                _ListFunctionCourseAllPoints.Add(computation.Point);

                while (computation.IsNextReal)
                {
                    ProjectileMotionWithRezistanceComputation nextComputation = computation.Continue();
                    _ListFunctionCourseAllPoints.Add(nextComputation.Point);
                    computation = nextComputation;
                }
            }

            return _ListFunctionCourseAllPoints;
        }


        public override List<ProjectileMotionPoint> GetListFunctionCourse()
        {
            return GetListFunctionCourseAllPoints()
                .Where((p, i) => i % Math.Round((double)GetListFunctionCourseAllPoints().Count / Settings.PointsForFunctionCourse) == 0 || i == GetListFunctionCourseAllPoints().Count - 1 || p.IsFarthest || p.IsHighest)
                .ToList();
        }

        public override double[][] GetFunctionCourse()
        {
            return GetListFunctionCourse().Select(p => p.Round().GetCoords(Settings.Quantities.Units.Length)).ToArray();
        }


        public new ProjectileMotionWithRezistanceSettings Settings { get; private set; }


        public override ProjectileMotion SaveInfoToTxt()
        {
            base.SaveInfoToTxt();
            return this;
        }


        public override ProjectileMotion SaveDataToCsv()
        {
            base.SaveDataToCsv();
            return this;
        }

        public ProjectileMotion Degrade()
        {
            return new ProjectileMotion(Settings as ProjectileMotionSettings);
        }
    }
}