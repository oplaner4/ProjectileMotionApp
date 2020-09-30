using Utilities.Quantities;
using Utilities.Units;
using System.Collections.Generic;
using System.Linq;
using ProjectileMotionSource.Point;
using MathNet.Numerics;
using System;
using ProjectileMotionSource.PointsComputation;

namespace ProjectileMotionSource.Func
{
    public class ProjectileMotionTrajectory
    {
        private ProjectileMotionSettings Settings { get; set; }

        internal ProjectileMotionTrajectory(ProjectileMotionSettings settings)
        {
            Settings = settings;
        }

        public virtual ProjectileMotionPoint GetPoint(Time t)
        {
            return new ProjectileMotionPoint(Settings, t);
        }

        public virtual ProjectileMotionPoint GetInitialPoint()
        {
            return GetPoint(ProjectileMotionPointsComputation.GetTimeInitial());
        }

        public virtual ProjectileMotionPoint GetFarthestPoint()
        {
            return GetPoint(Settings.Quantities.Α.IsRight() ? new ProjectileMotionPointsComputation(Settings).GetTimeHighest() : new ProjectileMotionPointsComputation(Settings).GetTimeFarthest());
        }

        public virtual ProjectileMotionPoint GetHighestPoint()
        {
            return GetPoint(new ProjectileMotionPointsComputation(Settings).GetTimeHighest());
        }

        public virtual ProjectileMotionPoint GetFinalPoint ()
        {
            return GetPoint(new ProjectileMotionPointsComputation(Settings).GetTimeFinal());
        }

        protected List<ProjectileMotionPoint> GetDifferentSpecialPoints()
        {
            List<ProjectileMotionPoint> result = new List<ProjectileMotionPoint>();

            if (GetInitialPoint().T == GetHighestPoint().T)
            {
                result.Add(GetInitialPoint());
                result.Add(GetFarthestPoint());
            }
            else if (GetHighestPoint().T == GetFarthestPoint().T)
            {
                result.Add(GetInitialPoint());
                result.Add(GetHighestPoint());
                result.Add(GetFinalPoint());
            }
            else if (GetFarthestPoint().T == GetFinalPoint().T)
            {
                result.Add(GetInitialPoint());
                result.Add(GetHighestPoint());
                result.Add(GetFarthestPoint());
            }
            else
            {
                result.Add(GetInitialPoint());
                result.Add(GetHighestPoint());
                result.Add(GetFarthestPoint());
                result.Add(GetFinalPoint());
            }
            return result;
        }

        public delegate ProjectileMotionPoint OrdinaryPointGenerator (int currentIndex, int ordinaryPointsCount, List<ProjectileMotionPoint> specialPoints);

        protected virtual List<ProjectileMotionPoint> GetPointsList (OrdinaryPointGenerator generator)
        {
            List<ProjectileMotionPoint> result = new List<ProjectileMotionPoint>();
            List<ProjectileMotionPoint> specialPoints = GetDifferentSpecialPoints();
            int ordinaryPointsCount = Settings.PointsForTrajectory - specialPoints.Count;

            for (int i = 1; i <= ordinaryPointsCount; i++)
            {
                result.Add(generator(i, ordinaryPointsCount, specialPoints));
            };

            result.AddRange(specialPoints);
            return result.OrderBy(p => p.T.Val).ToList();
        }

        public virtual List<ProjectileMotionPoint> GetPointsList()
        {
            double finalTimeBasicVal = GetFinalPoint().T.GetBasicVal();

            return GetPointsList((currentIndex, ordinaryPointsCount, specialPoints) =>
            {
                Time now = new Time(finalTimeBasicVal * currentIndex / ordinaryPointsCount, UnitTime.Basic);
                if (specialPoints.Where(p => p.T == now).Any())
                {
                    return GetPoint(new Time((finalTimeBasicVal * (currentIndex - 1) / ordinaryPointsCount + now.Val) / 2, UnitTime.Basic));
                }

                return GetPoint(now);
            });
        }

        public virtual Length GetArcLength()
        {
            if (Settings.Quantities.Α.IsRight())
            {
                return new Length(2.0 * GetHighestPoint().Y.GetBasicVal(), UnitLength.Basic);
            }

            return new Length(Integrate.OnClosedInterval(x => Math.Sqrt(1 + Math.Pow(Math.Tan(Settings.Quantities.Α.GetBasicVal()) - (Settings.Quantities.G.GetBasicVal() / Math.Pow(Settings.Quantities.V.GetBasicVal() * Math.Cos(Settings.Quantities.Α.GetBasicVal()), 2.0)) * x, 2.0)), 0, GetFarthestPoint().X.GetBasicVal()), UnitLength.Basic);
        }

        public virtual Area GetAreaUnderArc ()
        {
            if (Settings.Quantities.Α.IsRight())
            {
                return new Area(0, Settings.Quantities.Units.Area);
            }

            double length = GetFinalPoint().X.GetBasicVal();

            return new Area(Math.Pow(length, 2.0) * Math.Tan(Settings.Quantities.Α.GetBasicVal()) / 2.0 - Math.Pow(length, 3.0) * Settings.Quantities.G.GetBasicVal() / (6.0 * Math.Pow(Settings.Quantities.V.GetBasicVal() * Math.Cos(Settings.Quantities.Α.GetBasicVal()), 2.0)) + length * Settings.Quantities.H.GetBasicVal(), UnitArea.Basic);
        }
    }
}