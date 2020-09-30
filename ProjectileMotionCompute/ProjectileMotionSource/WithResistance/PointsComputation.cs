using System;
using ProjectileMotionSource.WithResistance.Func;
using ProjectileMotionSource.Point;
using ProjectileMotionSource.Func;
using ProjectileMotionSource.PointsComputation;

namespace ProjectileMotionSource.WithResistance.PointsComputation
{
    internal class ProjectileMotionWithResistanceComputation
    {
        public static ProjectileMotionWithResistanceComputation Start(ProjectileMotionWithResistanceSettings settings)
        {
            return new ProjectileMotionWithResistanceComputation(settings);
        }

        public ProjectileMotionWithResistanceComputation Continue()
        {
            IsNextReal = true;

            VyComputed = GetNewVy();
            VxComputed = GetNewVx();

            Point = new ProjectileMotionPoint(this);

            if (Point.Y.Val == 0)
            {
                IsNextReal = false;
            }

            return this;
        }

        public ProjectileMotionPoint Point { get; private set; }

        public bool IsNextReal { get; private set; }

        private ProjectileMotionWithResistanceComputation (ProjectileMotionWithResistanceSettings settings)
        {
            Settings = settings;
            IsNextReal = true;

            Point = new ProjectileMotionPoint(new ProjectileMotionSettings(Settings.Quantities), ProjectileMotionPointsComputation.GetTimeInitial());

            VyComputed = Point.Vy.GetBasicVal();
            VxComputed = Point.Vx.GetBasicVal();

            if (Point.Y.Val == 0 && Settings.Quantities.Α.Val == 0)
            {
                IsNextReal = false;
            }
        }

        internal double GetNewVy()
        {
            return VyComputed + GetAy() * Dt;
        }

        internal double GetNewVx()
        {
            return VxComputed + GetAx() * Dt;
        }

        internal double GetNewY()
        {
            return Point.Y.GetBasicVal() + VyComputed * Dt;
        }

        private double VyComputed { get; set; }

        private double VxComputed { get; set; }

        internal ProjectileMotionWithResistanceSettings Settings { get; private set; }

        public static double Dt = Math.Pow(10, -1 * 3);

        private double GetAx()
        {
            return -1.0 * GetD() * Math.Sqrt(Math.Pow(Point.Vx.GetBasicVal(), 2.0) + Math.Pow(Point.Vy.GetBasicVal(), 2.0)) * Point.Vx.GetBasicVal() / Settings.Quantities.M.GetBasicVal();
        }

        private double GetD()
        {
            return Settings.Quantities.Ρ.GetBasicVal() * Settings.Quantities.C.Val * Settings.Quantities.A.GetBasicVal() / 2.0;
        }

        private double GetAy()
        {
            return -1.0 * Settings.Quantities.G.GetBasicVal() - GetD() * Math.Sqrt(Math.Pow(Point.Vx.GetBasicVal(), 2.0) + Math.Pow(Point.Vy.GetBasicVal(), 2.0)) * Point.Vy.GetBasicVal() / Settings.Quantities.M.GetBasicVal();
        }
    }
}