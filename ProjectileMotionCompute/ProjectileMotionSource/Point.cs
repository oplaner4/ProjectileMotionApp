 using Utilities.Units;
using Utilities.Quantities;
using ProjectileMotionSource.Func;
using System;
using ProjectileMotionSource.WithResistance.PointsComputation;
using ProjectileMotionSource.PointsComputation;
using ProjectileMotionSource.WithResistance.Func;

namespace ProjectileMotionSource.Point
{
    /// <summary>
    /// Projectile motion point of the trajectory.
    /// Properties are converted into results units.
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

        internal bool IsHighest { get; private set; }

        /// <summary>
        /// For motions with resistance.
        /// </summary>
        private bool WasHighest { get; set; }

        private void InResultUnits()
        {
            X = X.Convert(Settings.Quantities.Units.Length);
            Y = Y.Convert(Settings.Quantities.Units.Length);
            Vx = Vx.Convert(Settings.Quantities.Units.Velocity);
            Vy = Vy.Convert(Settings.Quantities.Units.Velocity);
            T = T.Convert(Settings.Quantities.Units.Time);
        }

        internal ProjectileMotionPoint(ProjectileMotionWithResistanceComputation prevComputation)
        {
            Settings = new ProjectileMotionSettings(prevComputation.Settings.Quantities);

            X = new Length(prevComputation.Point.X.GetBasicVal() + prevComputation.Point.Vx.GetBasicVal() * ProjectileMotionWithResistanceComputation.Dt, UnitLength.Basic);
            Y = new Length(prevComputation.GetNewY() < 0 ? 0 : prevComputation.GetNewY(), UnitLength.Basic);
            T = new Time(prevComputation.Point.T.GetBasicVal() + ProjectileMotionWithResistanceComputation.Dt, UnitTime.Basic);
            Vx = new Velocity(Math.Abs(prevComputation.GetNewVx()), UnitVelocity.Basic);
            Vy = new Velocity(Math.Abs(prevComputation.GetNewVy()), UnitVelocity.Basic);

            WasHighest = prevComputation.Point.WasHighest;

            if (Y < prevComputation.Point.Y && !prevComputation.Point.WasHighest)
            {
                prevComputation.Point.IsHighest = true;
                prevComputation.Point.Vy = new Velocity(0, UnitVelocity.Basic);
                WasHighest = true;
            }

            InResultUnits();
        }

        internal ProjectileMotionPoint(ProjectileMotionSettings settings, Time t)
        {
            Settings = settings;
            IsHighest = false;

            T = t;

            if (T == ProjectileMotionPointsComputation.GetTimeInitial() && Settings.Quantities.Α.Val == 0)
            {
                IsHighest = true;
                WasHighest = true;
            }

            ProjectileMotionPointsComputation projectileMotionPointsComputation = new ProjectileMotionPointsComputation(Settings);

            Y = projectileMotionPointsComputation.GetY(T);
            X = projectileMotionPointsComputation.GetX(T);
            Vx = projectileMotionPointsComputation.GetVelocityX();
            Vy = projectileMotionPointsComputation.GetVelocityY(T);

            InResultUnits();
        }

        public Length X { get; private set; }
        public Length Y { get; private set; }
        public Velocity Vx { get; private set; }
        public Velocity Vy { get; private set; }

        public Time T { get; private set; }
        private ProjectileMotionSettings Settings { get; set; }

        public Velocity GetVelocity()
        {
            return new Velocity(Math.Sqrt(Math.Pow(Vx.GetBasicVal(), 2) + Math.Pow(Vy.GetBasicVal(), 2)), UnitVelocity.Basic).Convert(Settings.Quantities.Units.Velocity);
        }

        public Length GetDistanceFromPoint(ProjectileMotionPoint p)
        {
           return new Length(Math.Sqrt(Math.Pow(p.X.GetBasicVal() - X.GetBasicVal(), 2.0) + Math.Pow(p.Y.GetBasicVal() - Y.GetBasicVal(), 2.0)), UnitLength.Basic).Convert(Settings.Quantities.Units.Length);
        }

        public Length GetDistance()
        {
            return GetDistanceFromPoint(new ProjectileMotionPoint(Settings, ProjectileMotionPointsComputation.GetTimeInitial()));
        }
    }
}