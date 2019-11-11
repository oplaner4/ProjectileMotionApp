using ProjectileMotionSource.Exceptions;
using ProjectileMotionSource.Func;
using ProjectileMotionSource.Point;
using ProjectileMotionSource.WithRezistance.PointsComputation;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Quantities;
using Utilities.Units;

namespace ProjectileMotionSource.WithRezistance.Func
{
    /// <summary>
    /// Projectile motion with rezistance features.
    /// </summary>
    public class ProjectileMotionWithRezistance : ProjectileMotion
    {
        public ProjectileMotionWithRezistance(ProjectileMotionWithRezistanceSettings settings) : base(settings)
        {
            Settings = settings;
            _ListAllPointsOfTrajectory = new List<ProjectileMotionPoint>();
        }

        public override Length GetLength()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Final).Round().X;
        }


        public override Time GetDur()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Final).Round().T;
        }

        public override Length GetX(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().X;
        }


        public override Length GetY(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().Y;
        }


        public override Length GetMaxDistance()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest).GetDistance(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        public override Length GetMaxHeight()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest).Round().Y;
        }

        public override Time GetTimeFallen()
        {
            return GetDur();
        }

        public override double[] GetCoords(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().GetCoords(Settings.Quantities.Units.Length);
        }

        public override double[] GetCoordsFallen()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Final).Round().GetCoords(Settings.Quantities.Units.Length);
        }

        public override Time GetTimeHighest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest).Round().T;
        }

        public override double[] GetCoordsHighest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest).Round().GetCoords(Settings.Quantities.Units.Length);
        }


        public override Time GetTimeFarthest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest).Round().T;
        }


        public override double[] GetCoordsFarthest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest).Round().GetCoords(Settings.Quantities.Units.Length);
        }


        public override Velocity GetVelocityX()
        {
            throw new OnlySuperClassMethodException("This method cannot be used for motions with rezistance");
        }


        public Velocity GetVelocityX(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().Vx;
        }


        public override Velocity GetVelocityY(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().Vy;
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
                foreach (ProjectileMotionPoint point in GetListAllPointsOfTrajectory())
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
                foreach (ProjectileMotionPoint point in GetListAllPointsOfTrajectory())
                {
                    l += point.GetDistanceFromPoint(prevPoint, UnitLength.Basic).Val;
                    prevPoint = point;
                }

                _ArcLength = new Length(l, UnitLength.Basic).Convert(unitLength);
            }

            return _ArcLength;
        }

        public override ProjectileMotionPoint GetPoint(Time t)
        {
            if (t >= GetDur())
            {
                return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Final);
            }
            return GetListAllPointsOfTrajectory().Where(x => t.GetBasicVal() - x.T.GetRoundedVal(Settings.RoundDigits) <= ProjectileMotionWithRezistanceComputation.Dt).First();

        }


        
        public override ProjectileMotionPoint GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes type)
        {
            switch (type)
            {
                case ProjectileMotionPoint.ProjectileMotionPointTypes.Highest:
                    return GetListAllPointsOfTrajectory().Where(p => p.IsHighest).First();
                case ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest:
                    return GetListAllPointsOfTrajectory().Where(p => p.IsFarthest).First();
                case ProjectileMotionPoint.ProjectileMotionPointTypes.Initial:
                    return GetListAllPointsOfTrajectory().First();
                case ProjectileMotionPoint.ProjectileMotionPointTypes.Final:
                    return GetListAllPointsOfTrajectory().Last();
            }

            return null;
        }


        private List<ProjectileMotionPoint> _ListAllPointsOfTrajectory { get; set; }

        private List<ProjectileMotionPoint> GetListAllPointsOfTrajectory()
        {
            if (!_ListAllPointsOfTrajectory.Any())
            {
                ProjectileMotionWithRezistanceComputation computation = ProjectileMotionWithRezistanceComputation.Start(this);
                _ListAllPointsOfTrajectory.Add(computation.Point);

                while (computation.IsNextReal)
                {
                    ProjectileMotionWithRezistanceComputation nextComputation = computation.Continue();
                    _ListAllPointsOfTrajectory.Add(nextComputation.Point);
                    computation = nextComputation;
                }
            }

            return _ListAllPointsOfTrajectory;
        }


        public override List<ProjectileMotionPoint> GetListPointsOfTrajectory()
        {
            return GetListAllPointsOfTrajectory()
                .Where((p, i) => i % Math.Round((double)GetListAllPointsOfTrajectory().Count / Settings.PointsForTrajectory) == 0 || i == GetListAllPointsOfTrajectory().Count - 1 || p.IsFarthest || p.IsHighest)
                .ToList();
        }

        public override double[][] GetTrajectory()
        {
            return GetListPointsOfTrajectory().Select(p => p.Round().GetCoords(Settings.Quantities.Units.Length)).ToArray();
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
            ProjectileMotion DegradedMotion = new ProjectileMotion(new ProjectileMotionSettings(Settings.Quantities)
            {
                RoundDigits = Settings.RoundDigits
            });

            if (GetLength().Val > 0)
            {
                DegradedMotion.Settings.PointsForTrajectory = (int)(Settings.PointsForTrajectory * DegradedMotion.GetLength().Val / GetLength().Val);
            }

            return DegradedMotion;
        }
    }
}