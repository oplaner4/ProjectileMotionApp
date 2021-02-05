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
            FarthestPointIndex = -1;
            HighestPointIndex = -1;
            AreaUnderArcVal = -1;
            ArcLengthVal = -1;
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
            if (FarthestPointIndex < 0)
            {
                FarthestPointIndex = 0;

                int count = GetListAllPointsOfTrajectory().Count();

                for (int i = 1; i < count; i++)
                {
                    if (GetListAllPointsOfTrajectory()[i].GetDistance() < GetListAllPointsOfTrajectory()[FarthestPointIndex].GetDistance())
                    {
                        break;
                    }

                    FarthestPointIndex = i;
                }

                ProjectileMotionPoint final = GetFinalPoint();
                ProjectileMotionPoint farthest = GetListAllPointsOfTrajectory()[FarthestPointIndex];

                if (final.GetDistance() > farthest.GetDistance())
                {
                    FarthestPointIndex = count - 1;
                    return final;
                }

                return farthest;
            }

            return GetListAllPointsOfTrajectory()[FarthestPointIndex];
        }

        public override ProjectileMotionPoint GetHighestPoint()
        {
            if (HighestPointIndex < 0)
            {
                HighestPointIndex = GetListAllPointsOfTrajectory().FindIndex(p => p.IsHighest);
            }

            return GetListAllPointsOfTrajectory()[HighestPointIndex];
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
        private int HighestPointIndex { get; set; }

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

        private double AreaUnderArcVal { get; set; }

        public override Area GetAreaUnderArc()
        {
            if (AreaUnderArcVal < 0)
            {
                AreaUnderArcVal = Settings.Quantities.Α.IsRight() ? 0 :
                    GetSumParallelPointsOfTrajectory(i => {
                        ProjectileMotionPoint current = GetListAllPointsOfTrajectory().ElementAt(i);
                        ProjectileMotionPoint prev = GetListAllPointsOfTrajectory().ElementAt(i - 1);
                        return (current.Y.GetBasicVal() + prev.Y.GetBasicVal()) *
                        (current.X.GetBasicVal() - prev.X.GetBasicVal()) / 2.0;
                    });
            }

            return new Area(AreaUnderArcVal, UnitArea.Basic);
        }

        private double ArcLengthVal { get; set; }

        public override Length GetArcLength()
        {
            if (ArcLengthVal < 0)
            {
                ArcLengthVal = GetSumParallelPointsOfTrajectory(i => {
                    return GetListAllPointsOfTrajectory().ElementAt(i).GetDistanceFromPoint(
                        GetListAllPointsOfTrajectory().ElementAt(i - 1)
                    ).GetBasicVal();
                });
            }

            return new Length(ArcLengthVal, UnitLength.Basic);
        }
    }
}