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
            ProjectileMotionPoint final = GetFinalPoint();
            ProjectileMotionPoint initial = GetInitialPoint();
            ProjectileMotionPoint above = GetPoint(new ProjectileMotionPointsComputation(Settings).GetTimeFarthestAbove());

            if (final.GetDistanceFromPoint(initial) > above.GetDistanceFromPoint(initial)) {
                return final;
            }

            return above;
        }

        public virtual ProjectileMotionPoint GetHighestPoint()
        {
            return GetPoint(new ProjectileMotionPointsComputation(Settings).GetTimeHighest());
        }

        public virtual ProjectileMotionPoint GetFinalPoint()
        {
            return GetPoint(new ProjectileMotionPointsComputation(Settings).GetTimeFinal());
        }

        protected List<ProjectileMotionPoint> GetDifferentSpecialPoints()
        {
            ProjectileMotionPoint initial = GetInitialPoint();
            List<ProjectileMotionPoint> result = new List<ProjectileMotionPoint>()
            {
                initial
            };

            ProjectileMotionPoint highest = GetHighestPoint();
            ProjectileMotionPoint farthest = GetFarthestPoint();

            if (initial.T == highest.T)
            {
                result.Add(farthest);
            }
            else
            {
                result.Add(highest);

                ProjectileMotionPoint final = GetFinalPoint();

                if (highest.T == farthest.T)
                {
                    result.Add(final);
                }
                else
                {
                    result.Add(farthest);

                    if (farthest.T != final.T)
                    {
                        result.Add(final);
                    }
                }
            }
            return result;
        }

        protected delegate ProjectileMotionPoint OrdinaryPointGenerator (int currentIndex, int ordinaryPointsCount, List<ProjectileMotionPoint> specialPoints);

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
            double length = GetFinalPoint().X.GetBasicVal();

            if (Settings.Quantities.Α.IsRight() || length == 0)
            {
                return new Length(2.0 * GetHighestPoint().Y.GetBasicVal() - Settings.Quantities.H.GetBasicVal(), UnitLength.Basic);
            }

            return new Length(
                Integrate.OnClosedInterval(
                    x => Math.Sqrt(
                        1 + Math.Pow(
                            Math.Tan(Settings.Quantities.Α.GetBasicVal()) -
                            Settings.Quantities.G.GetBasicVal() /
                            Math.Pow(GetInitialPoint().Vx.GetBasicVal(), 2.0) * x, 2.0)
                    ), 0, length
                ), UnitLength.Basic
            );
        }

        public virtual Area GetAreaUnderArc ()
        {
            double length = GetFinalPoint().X.GetBasicVal();

            if (Settings.Quantities.Α.IsRight() || length == 0)
            {
                return new Area(0, Settings.Quantities.Units.Area);
            }

            return new Area(
                Math.Pow(length, 2.0) * Math.Tan(Settings.Quantities.Α.GetBasicVal()) / 2.0 -
                Math.Pow(length, 3.0) * Settings.Quantities.G.GetBasicVal() /
                (6.0 * Math.Pow(GetInitialPoint().Vx.GetBasicVal(), 2.0)) +
                length * Settings.Quantities.H.GetBasicVal(),
                UnitArea.Basic
            );
        }
    }
}