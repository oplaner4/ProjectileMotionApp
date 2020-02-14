using System;
using ProjectileMotionSource.WithRezistance.Func;
using ProjectileMotionSource.Point;
using Utilities.Quantities;
using Utilities.Units;

namespace ProjectileMotionSource.WithRezistance.PointsComputation
{
    internal class ProjectileMotionWithRezistanceComputation
    {
        public static ProjectileMotionWithRezistanceComputation Start(ProjectileMotionWithRezistance motion)
        {
            return new ProjectileMotionWithRezistanceComputation(motion);
        }

        public ProjectileMotionPoint Point { get; private set; }

        public bool IsNextReal { get; private set; }

        private ProjectileMotionWithRezistanceComputation(ProjectileMotionWithRezistance motion)
        {
            Motion = motion;
            IsNextReal = true;

            Point = new ProjectileMotionPoint(Motion, new Time(0, UnitTime.Basic));
            VyComputed = Point.Vy.GetBasicVal();
            VxComputed = Point.Vx.GetBasicVal();

            if (Point.Y.Val == 0 && Motion.Settings.Quantities.Α.Val == 0)
            {
                IsNextReal = false;
            }
        }

        private ProjectileMotionWithRezistanceComputation(ProjectileMotionWithRezistanceComputation prevComputation)
        {
            Motion = prevComputation.Motion;
            IsNextReal = true;

            VyComputed = prevComputation.GetNewVy();
            VxComputed = prevComputation.GetNewVx();

            Point = new ProjectileMotionPoint( prevComputation );

            if (Point.Y.Val == 0)
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

        internal ProjectileMotionWithRezistance Motion { get; private set; }

        public ProjectileMotionWithRezistanceComputation Continue()
        {
            return new ProjectileMotionWithRezistanceComputation(this);
        }

        public static double Dt = Math.Pow(10, -1 * 3);

        internal double GetAx()
        {
            return -1.0 * GetD() * Math.Sqrt(Math.Pow(Point.Vx.GetBasicVal(), 2.0) + Math.Pow(Point.Vy.GetBasicVal(), 2.0)) * Point.Vx.GetBasicVal() / Motion.Settings.Quantities.M.GetBasicVal();
        }

        public double GetD()
        {
            return Motion.Settings.Quantities.Ρ.GetBasicVal() * Motion.Settings.Quantities.C.Val * Motion.Settings.Quantities.A.GetBasicVal() / 2.0;
        }

        private double GetAy()
        {
            return -1.0 * Motion.Settings.Quantities.G.GetBasicVal() - GetD() * Math.Sqrt(Math.Pow(Point.Vx.GetBasicVal(), 2.0) + Math.Pow(Point.Vy.GetBasicVal(), 2.0)) * Point.Vy.GetBasicVal() / Motion.Settings.Quantities.M.GetBasicVal();
        }
    }
}