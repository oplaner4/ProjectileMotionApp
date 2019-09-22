﻿using System;
using ProjectileMotionSource.WithRezistance.Func;
using ProjectileMotionSource.Point;

namespace ProjectileMotionSource.WithRezistance.PointsComputation
{
    public class ProjectileMotionWithRezistanceComputation
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

            Point = Motion.Degrade().GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Initial);
            VyComputed = Point.Vy.Val;
        }

        private ProjectileMotionWithRezistanceComputation(ProjectileMotionWithRezistanceComputation prevComputation)
        {
            Motion = prevComputation.Motion;
            IsNextReal = true;

            VyComputed = prevComputation.GetNewVy();

            Point = new ProjectileMotionPoint( prevComputation );

            if (Point.Y.GetBasicVal() == 0)
            {
                IsNextReal = false;
            }
        }

        internal double GetNewVy()
        {
            return VyComputed + GetAy() * Dt;
        }

        internal double GetNewY()
        {
            return Point.Y.GetBasicVal() + VyComputed * Dt;
        }

        private double VyComputed { get; set; }

        public ProjectileMotionWithRezistance Motion { get; private set; }

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