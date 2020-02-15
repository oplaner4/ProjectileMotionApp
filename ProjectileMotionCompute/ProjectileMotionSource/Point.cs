using Utilities.Units;
using Utilities.Quantities;
using ProjectileMotionSource.Func;
using System;
using MathNet.Numerics.RootFinding;
using ProjectileMotionSource.WithRezistance.PointsComputation;
using ProjectileMotionSource.WithRezistance.Func;
using ProjectileMotionSource.Exceptions;

namespace ProjectileMotionSource.Point
{
    /// <summary>
    /// Projectile motion point of trajectory.
    /// </summary>
    public class ProjectileMotionPoint
    {
        public enum ProjectileMotionPointTypes
        {
            Highest,
            Farthest,
            Initial,
            Final
        }

        public bool IsHighest { get; private set; }

        private bool WasHighest { get; set; }

        public bool IsFarthest { get; set; }

        private void ComputeProperties()
        {
            Vx = GetVelocityX();
            Vy = GetVelocityY();


            if (T >= GetTimeFinal() && T.Val != 0)
            {
                X = Motion.GetLength();
                Y = new Length(0, UnitLength.Basic);
            }
            else
            {
                X = new Length(T.GetBasicVal() * GetVelocityX().Val, UnitLength.Basic);
                double computedLength = T.GetBasicVal() * Motion.Settings.Quantities.V.GetBasicVal() * Math.Sin(Motion.Settings.Quantities.Α.GetBasicVal()) - 0.5 * Motion.Settings.Quantities.G.GetBasicVal() * Math.Pow(T.GetBasicVal(), 2) + Motion.Settings.Quantities.H.GetBasicVal();
                Y = new Length(computedLength < 0 ? 0 : computedLength, UnitLength.Basic);
            }

            ConvertToResultUnits();
        }


        private void ConvertToResultUnits()
        {
            X = X.Convert(Motion.Settings.Quantities.Units.Length);
            Y = Y.Convert(Motion.Settings.Quantities.Units.Length);
            Vx = Vx.Convert(Motion.Settings.Quantities.Units.Velocity);
            Vy = Vy.Convert(Motion.Settings.Quantities.Units.Velocity);
            T = T.Convert(Motion.Settings.Quantities.Units.Time);
        }


        internal ProjectileMotionPoint(ProjectileMotionWithRezistanceComputation prevComputation)
        {
            Motion = prevComputation.Motion;
            X = new Length(prevComputation.Point.X.GetBasicVal() + prevComputation.Point.Vx.GetBasicVal() * ProjectileMotionWithRezistanceComputation.Dt, UnitLength.Basic);
            Y = new Length(prevComputation.GetNewY() < 0 ? 0 : prevComputation.GetNewY(), UnitLength.Basic);
            T = new Time(prevComputation.Point.T.GetBasicVal() + ProjectileMotionWithRezistanceComputation.Dt, UnitTime.Basic);
            Vx = new Velocity(Math.Abs(prevComputation.GetNewVx()), UnitVelocity.Basic);
            Vy = new Velocity(Math.Abs(prevComputation.GetNewVy()), UnitVelocity.Basic);

            WasHighest = prevComputation.Point.WasHighest;

            if (Y < prevComputation.Point.Y && !prevComputation.Point.WasHighest)
            {
                prevComputation.Point.IsHighest = true;
                WasHighest = true;
            }

            ConvertToResultUnits();
        }

        public ProjectileMotionPoint(ProjectileMotion motion, Time t)
        {
            Motion = motion;
            IsFarthest = false;
            IsHighest = false;

            T = t;

            ComputeProperties();
        }


        public ProjectileMotionPoint(ProjectileMotion motion, ProjectileMotionPointTypes type)
        {
            if (motion is ProjectileMotionWithRezistance)
            {
                throw new OnlySuperClassMethodException("This constructor cannot be used for motions with rezistance");
            }

            Motion = motion;
            IsFarthest = false;
            IsHighest = false;

            switch (type)
            {
                case ProjectileMotionPointTypes.Highest:
                    IsHighest = true;
                    if (HighestIsFarthest() && Motion.Settings.Quantities.Α.Val > 0)
                    {
                        IsFarthest = true;
                    }

                    T = GetTimeHighest();
                    break;
                case ProjectileMotionPointTypes.Farthest:
                    IsFarthest = true;
                    if (HighestIsFarthest())
                    {
                        IsHighest = true;
                        T = GetTimeHighest();
                    }
                    else {
                        T = GetTimeFarthest();
                    }
                    break;
                case ProjectileMotionPointTypes.Initial:
                    if (Motion.Settings.Quantities.Α.Val == 0)
                    {
                        IsHighest = true;
                        WasHighest = true;
                        if (Motion.Settings.Quantities.H.Val == 0)
                        {
                            IsFarthest = true;
                        }
                    }

                    T = new Time(0, UnitTime.Basic);
                    break;
                case ProjectileMotionPointTypes.Final:
                    T = GetTimeFinal();
                    if (GetTimeFarthest() >= T || Motion.Settings.Quantities.Α.Val == 0)
                    {
                        IsFarthest = true;
                    }
                    break;
                default:
                    T = new Time(0, UnitTime.Basic);
                    break;
            }


            ComputeProperties();
        }

        private bool HighestIsFarthest()
        {
            return GetTimeFarthest() == GetTimeHighest() || Motion.Settings.Quantities.Α.IsRight();
        }

        private Time GetTimeFinal()
        {
            return new Time((Motion.Settings.Quantities.V.GetBasicVal() * Math.Sin(Motion.Settings.Quantities.Α.GetBasicVal()) + Math.Sqrt(Math.Pow(Motion.Settings.Quantities.V.GetBasicVal() * Math.Sin(Motion.Settings.Quantities.Α.GetBasicVal()), 2) + 2 * Motion.Settings.Quantities.G.GetBasicVal() * Motion.Settings.Quantities.H.GetBasicVal())) / Motion.Settings.Quantities.G.GetBasicVal(), UnitTime.Basic);
        }

        private Time GetTimeHighest ()
        {
            return new Time(Motion.Settings.Quantities.V.GetBasicVal() * Math.Sin(Motion.Settings.Quantities.Α.GetBasicVal()) / Motion.Settings.Quantities.G.GetBasicVal(), UnitTime.Basic);
        }
             
        private Time GetTimeFarthest()
        {
            double root = Cubic.RealRoots(
                2.0 * Motion.Settings.Quantities.H.GetBasicVal() * Motion.Settings.Quantities.V.GetBasicVal() * Math.Sin(Motion.Settings.Quantities.Α.GetBasicVal()) / Math.Pow(Motion.Settings.Quantities.G.GetBasicVal(), 2.0),
                (2.0 * Math.Pow(Motion.Settings.Quantities.V.GetBasicVal(), 2.0) - (Motion.Settings.Quantities.G.GetBasicVal() * Motion.Settings.Quantities.H.GetBasicVal())) / Math.Pow(Motion.Settings.Quantities.G.GetBasicVal(), 2.0),
                -3.0 * Motion.Settings.Quantities.V.GetBasicVal() * Math.Sin(Motion.Settings.Quantities.Α.GetBasicVal()) / Motion.Settings.Quantities.G.GetBasicVal()
            ).Item3;
            return double.IsNaN(root) ? GetTimeFinal() : new Time(root, UnitTime.Basic);
        }

        public ProjectileMotionPoint Round ()
        {
            X = X.RoundVal(Motion.Settings.RoundDigits);
            Y = Y.RoundVal(Motion.Settings.RoundDigits);
            Vx = Vx.RoundVal(Motion.Settings.RoundDigits);
            Vy = Vy.RoundVal(Motion.Settings.RoundDigits);
            T  = T.RoundVal(Motion.Settings.RoundDigits);
            return this;
        }

        public Length X { get; private set; }
        public Length Y { get; private set; }
        public Velocity Vx { get; private set; }
        public Velocity Vy { get; private set; }
        public Time T { get; private set; }
        private ProjectileMotion Motion { get; set; }

        public double[] GetCoords(UnitLength unitLength)
        {
            return new double[] { X.Convert(unitLength).Val, Y.Convert(unitLength).Val };
        }

        private Velocity GetVelocityY()
        {
            double computedBasicVelocity = Motion.Settings.Quantities.V.GetBasicVal() * Math.Sin(Motion.Settings.Quantities.Α.GetBasicVal()) - Motion.Settings.Quantities.G.GetBasicVal() * T.GetBasicVal();
            return new Velocity(computedBasicVelocity < 0 ? 0 : computedBasicVelocity, UnitVelocity.Basic);
        }

        private Velocity GetVelocityX()
        {
            if (Motion.Settings.Quantities.Α.IsRight())
            {
                return new Velocity(0, UnitVelocity.Basic);
            }

            return new Velocity(Math.Cos(Motion.Settings.Quantities.Α.GetBasicVal()) * Motion.Settings.Quantities.V.GetBasicVal(), UnitVelocity.Basic);
        }

        public Velocity GetVelocity(UnitVelocity unitVelocity)
        {
            return new Velocity(Math.Sqrt(Math.Pow(GetVelocityX().Val, 2) + Math.Pow(GetVelocityY().Val, 2)), UnitVelocity.Basic).Convert(unitVelocity);
        }

        public Length GetDistanceFromPoint(ProjectileMotionPoint p, UnitLength unitLength)
        {
           return new Length(Math.Sqrt(Math.Pow(p.X.GetBasicVal() - X.GetBasicVal(), 2.0) + Math.Pow(p.Y.GetBasicVal() - Y.GetBasicVal(), 2.0)), UnitLength.Basic).Convert(unitLength);
        }

        public Length GetDistance(UnitLength unitLength)
        {
            return GetDistanceFromPoint(new ProjectileMotionPoint(Motion, new Time(0, UnitTime.Basic)), unitLength);
        }
    }
}