using ProjectileMotionSource.Exceptions;
using ProjectileMotionSource.Func;
using ProjectileMotionSource.Point;
using ProjectileMotionSource.WithResistance.PointsComputation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Quantities;
using Utilities.Units;

namespace ProjectileMotionSource.WithResistance.Func
{
    /// <summary>
    /// Projectile motion with resistance features.
    /// </summary>
    public class ProjectileMotionWithResistance : ProjectileMotion
    {
        public ProjectileMotionWithResistance(ProjectileMotionWithResistanceSettings settings) : base(settings)
        {
            Settings = settings;
            _ListAllPointsOfTrajectory = new List<ProjectileMotionPoint>();
            _FarthestPointIndex = 0;
            _AreaUnderArc = new Area(0, UnitArea.Basic);
            _ArcLength = new Length(0, UnitLength.Basic);
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
            return GetFarthestPoint().GetDistance(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
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
            return GetFarthestPoint().Round().T;
        }


        public override double[] GetCoordsFarthest()
        {
            return GetFarthestPoint().Round().GetCoords(Settings.Quantities.Units.Length);
        }


        public override Velocity GetVelocityX()
        {
            throw new OnlySuperClassMethodException("This method cannot be used for motions with resistance");
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
            if (_AreaUnderArc.Val == 0)
            {
                double a = 0;

                if (!Settings.Quantities.Α.IsRight())
                {
                    a = GetSumParallelPointsOfTrajectory(i => {
                        return (GetListAllPointsOfTrajectory().ElementAt(i).Y.GetBasicVal() + GetListAllPointsOfTrajectory().ElementAt(i - 1).Y.GetBasicVal()) * (GetListAllPointsOfTrajectory().ElementAt(i).X.GetBasicVal() - GetListAllPointsOfTrajectory().ElementAt(i - 1).X.GetBasicVal()) / 2.0;
                    });
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
            if (_ArcLength.Val == 0)
            {
                _ArcLength = new Length(
                    GetSumParallelPointsOfTrajectory(i => {
                        return GetListAllPointsOfTrajectory().ElementAt(i).GetDistanceFromPoint(GetListAllPointsOfTrajectory().ElementAt(i - 1), UnitLength.Basic).Val;
                    }
                 ), UnitLength.Basic).Convert(unitLength);
            }

            return _ArcLength.Convert(unitLength);
        }


        private double GetSumParallelPointsOfTrajectory (Func<int, double> increaseFunc)
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


        private int _FarthestPointIndex { get; set; }

        private ProjectileMotionPoint GetFarthestPoint()
        {
            if (_FarthestPointIndex == 0)
            {
                for (int i = 0; i < GetListAllPointsOfTrajectory().Count(); i++)
                {
                    if (GetListAllPointsOfTrajectory()[i].GetDistance(UnitLength.Basic) > GetListAllPointsOfTrajectory()[_FarthestPointIndex].GetDistance(UnitLength.Basic))
                    {
                        _FarthestPointIndex = i;
                    }
                }
            }

            GetListAllPointsOfTrajectory()[_FarthestPointIndex].IsFarthest = true;
            return GetListAllPointsOfTrajectory()[_FarthestPointIndex];
        }

        public override ProjectileMotionPoint GetPoint(Time t)
        {
            if (t >= GetDur())
            {
                return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Final);
            }
            return GetListAllPointsOfTrajectory().Where(x => t.GetBasicVal() - x.T.GetRoundedVal(Settings.RoundDigits) <= ProjectileMotionWithResistanceComputation.Dt).First();

        }


        
        public override ProjectileMotionPoint GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes type)
        {
            switch (type)
            {
                case ProjectileMotionPoint.ProjectileMotionPointTypes.Highest:
                    return GetListAllPointsOfTrajectory().Where(p => p.IsHighest).First();
                case ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest:
                    return GetFarthestPoint();
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
                ProjectileMotionWithResistanceComputation computation = ProjectileMotionWithResistanceComputation.Start(this);
                _ListAllPointsOfTrajectory.Add(computation.Point);

                while (computation.IsNextReal)
                {
                    ProjectileMotionWithResistanceComputation nextComputation = computation.Continue();
                    _ListAllPointsOfTrajectory.Add(nextComputation.Point);
                    computation = nextComputation;
                }
            }

            return _ListAllPointsOfTrajectory;
        }


        public override List<ProjectileMotionPoint> GetListPointsOfTrajectory()
        {
            GetListAllPointsOfTrajectory()[_FarthestPointIndex].IsFarthest = true;

            return GetListAllPointsOfTrajectory()
                .Where((p, i) => {
                    return i % Math.Round((double)GetListAllPointsOfTrajectory().Count / Settings.PointsForTrajectory) == 0 || i == GetListAllPointsOfTrajectory().Count - 1 || p.IsHighest || p.IsFarthest;
                 })
                .ToList();
        }

        public override double[][] GetTrajectory()
        {
            return GetListPointsOfTrajectory().Select(p => p.Round().GetCoords(Settings.Quantities.Units.Length)).ToArray();
        }


        public new ProjectileMotionWithResistanceSettings Settings { get; private set; }


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