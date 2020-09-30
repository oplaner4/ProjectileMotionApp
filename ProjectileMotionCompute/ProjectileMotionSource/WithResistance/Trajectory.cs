using Utilities.Quantities;
using Utilities.Units;
using System.Collections.Generic;
using System.Linq;
using ProjectileMotionSource.Point;
using ProjectileMotionSource.WithResistance.PointsComputation;
using System;
using System.Threading.Tasks;
using ProjectileMotionSource.Func;

namespace ProjectileMotionSource.WithResistance.Func
{
    public class ProjectileMotionWithResistanceTrajectory : ProjectileMotionTrajectory
    {
        private ProjectileMotionWithResistanceSettings Settings { get; set; }

        internal ProjectileMotionWithResistanceTrajectory(ProjectileMotionWithResistanceSettings settings) : base (settings)
        {
            Settings = settings;
            ListAllPointsOfTrajectory = new List<ProjectileMotionPoint>();
            FarthestPointIndex = 0;
            AreaUnderArc = new Area(0, UnitArea.Basic);
            ArcLength = new Length(0, UnitLength.Basic);
        }

        public override ProjectileMotionPoint GetPoint(Time t)
        {
            ProjectileMotionPoint finalPoint = GetFinalPoint();

            if (t >= finalPoint.T)
            {
                return finalPoint;
            }

            return GetListAllPointsOfTrajectory().Where(x => t == x.T).First();
        }

        public override ProjectileMotionPoint GetInitialPoint()
        {
            return GetListAllPointsOfTrajectory().First();
        }

        public override ProjectileMotionPoint GetFarthestPoint()
        {
            if (FarthestPointIndex == 0)
            {
                for (int i = 0; i < GetListAllPointsOfTrajectory().Count(); i++)
                {
                    if (GetListAllPointsOfTrajectory()[i].GetDistance() > GetListAllPointsOfTrajectory()[FarthestPointIndex].GetDistance())
                    {
                        FarthestPointIndex = i;
                    }
                }
            }

            return GetListAllPointsOfTrajectory()[FarthestPointIndex];
        }

        public override ProjectileMotionPoint GetHighestPoint()
        {
            return GetListAllPointsOfTrajectory().Where(p => p.IsHighest).First();
        }

        public override ProjectileMotionPoint GetFinalPoint()
        {
            return GetListAllPointsOfTrajectory().Last();
        }

        private List<ProjectileMotionPoint> ListAllPointsOfTrajectory { get; set; }

        private List<ProjectileMotionPoint> GetListAllPointsOfTrajectory()
        {
            if (!ListAllPointsOfTrajectory.Any())
            {
                ProjectileMotionWithResistanceComputation computation = ProjectileMotionWithResistanceComputation.Start(Settings);
                ListAllPointsOfTrajectory.Add(computation.Point);

                while (computation.IsNextReal)
                {
                    ListAllPointsOfTrajectory.Add(computation.Continue().Point);
                }
            }

            return ListAllPointsOfTrajectory;
        }


        public override List<ProjectileMotionPoint> GetPointsList()
        {
            return GetPointsList((currentIndex, ordinaryPointsCount, specialPoints) =>
            {
                ProjectileMotionPoint p = GetListAllPointsOfTrajectory().ElementAt(
                    (int)Math.Floor((GetListAllPointsOfTrajectory().Count - 1) * currentIndex / (double)ordinaryPointsCount)
                );

                if (specialPoints.Where(sp => sp.T == p.T).Any())
                {
                    return GetListAllPointsOfTrajectory().ElementAt(
                        (int)Math.Floor((2.0 * currentIndex - 1) * (GetListAllPointsOfTrajectory().Count - 1) / (2.0 * ordinaryPointsCount))
                    );
                }

                return p;
            });
        }

        private int FarthestPointIndex { get; set; }

        private double GetSumParallelPointsOfTrajectory(Func<int, double> increaseFunc)
        {
            double sum = 0.0;

            Parallel.For(1, GetListAllPointsOfTrajectory().Count,
                () => 0,
                (int i, ParallelLoopState loop, double subSum) =>
                {
                    subSum += increaseFunc(i);
                    return subSum;
                },
                (double subSum) =>
                {
                    lock (GetListAllPointsOfTrajectory())
                    {
                        sum += subSum;
                    }
                }
            );

            return sum;
        }

        private Area AreaUnderArc { get; set; }

        public override Area GetAreaUnderArc()
        {
            if (AreaUnderArc.Val == 0)
            {
                double a = 0;

                if (!Settings.Quantities.Α.IsRight())
                {
                    a = GetSumParallelPointsOfTrajectory(i => {
                        return (GetListAllPointsOfTrajectory().ElementAt(i).Y.GetBasicVal() + GetListAllPointsOfTrajectory().ElementAt(i - 1).Y.GetBasicVal()) * (GetListAllPointsOfTrajectory().ElementAt(i).X.GetBasicVal() - GetListAllPointsOfTrajectory().ElementAt(i - 1).X.GetBasicVal()) / 2.0;
                    });
                }

                AreaUnderArc = new Area(a, UnitArea.Basic);
            }

            return AreaUnderArc;
        }

        private Length ArcLength { get; set; }

        public override Length GetArcLength()
        {
            if (ArcLength.Val == 0)
            {
                ArcLength = new Length(
                    GetSumParallelPointsOfTrajectory(i => {
                        return GetListAllPointsOfTrajectory().ElementAt(i).GetDistanceFromPoint(GetListAllPointsOfTrajectory().ElementAt(i - 1)).GetBasicVal();
                    }
                 ), UnitLength.Basic);
            }

            return ArcLength;
        }
    }
}