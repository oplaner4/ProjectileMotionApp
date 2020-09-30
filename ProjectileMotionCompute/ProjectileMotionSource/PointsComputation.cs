using MathNet.Numerics.RootFinding;
using ProjectileMotionSource.Exceptions;
using ProjectileMotionSource.Func;
using ProjectileMotionSource.WithResistance.Func;
using System;
using Utilities.Quantities;
using Utilities.Units;

namespace ProjectileMotionSource.PointsComputation
{
    internal class ProjectileMotionPointsComputation
    {
        public ProjectileMotionPointsComputation (ProjectileMotionSettings settings)
        {
            if (settings is ProjectileMotionWithResistanceSettings)
            {
                throw new OnlySuperClassMethodException("This constructor cannot be used for motions with resistance");
            }

            Settings = settings;
        }

        private ProjectileMotionSettings Settings { get; set; }

        public static Time GetTimeInitial ()
        {
            return new Time(0, UnitTime.Basic);
        }

        public Time GetTimeFinal()
        {
            return new Time((Settings.Quantities.V.GetBasicVal() * Math.Sin(Settings.Quantities.Α.GetBasicVal()) + Math.Sqrt(Math.Pow(Settings.Quantities.V.GetBasicVal() * Math.Sin(Settings.Quantities.Α.GetBasicVal()), 2.0) + 2 * Settings.Quantities.G.GetBasicVal() * Settings.Quantities.H.GetBasicVal())) / Settings.Quantities.G.GetBasicVal(), UnitTime.Basic);
        }

        public Time GetTimeHighest()
        {
            return new Time(Settings.Quantities.V.GetBasicVal() * Math.Sin(Settings.Quantities.Α.GetBasicVal()) / Settings.Quantities.G.GetBasicVal(), UnitTime.Basic);
        }

        public Time GetTimeFarthest()
        {
            double root = Cubic.RealRoots(
                2.0 * Settings.Quantities.H.GetBasicVal() * Settings.Quantities.V.GetBasicVal() * Math.Sin(Settings.Quantities.Α.GetBasicVal()) / Math.Pow(Settings.Quantities.G.GetBasicVal(), 2.0),
                (2.0 * Math.Pow(Settings.Quantities.V.GetBasicVal(), 2.0) - (Settings.Quantities.G.GetBasicVal() * Settings.Quantities.H.GetBasicVal())) / Math.Pow(Settings.Quantities.G.GetBasicVal(), 2.0),
                -3.0 * Settings.Quantities.V.GetBasicVal() * Math.Sin(Settings.Quantities.Α.GetBasicVal()) / Settings.Quantities.G.GetBasicVal()
            ).Item3;
            return double.IsNaN(root) ? GetTimeFinal() : new Time(root, UnitTime.Basic);
        }

        public Length GetY (Time t)
        {
            double computedLength = t >= GetTimeFinal() ? 0 : t.GetBasicVal() * Settings.Quantities.V.GetBasicVal() * Math.Sin(Settings.Quantities.Α.GetBasicVal()) - 0.5 * Settings.Quantities.G.GetBasicVal() * Math.Pow(t.GetBasicVal(), 2) + Settings.Quantities.H.GetBasicVal();
            return new Length(computedLength < 0 ? 0 : computedLength, UnitLength.Basic);
        }

        public Length GetX(Time t)
        {
            return new Length(Settings.Quantities.Α.IsRight() ? 0 : t.GetBasicVal() * GetVelocityX().Val, UnitLength.Basic);
        }

        public Velocity GetVelocityY(Time t)
        {
            double computedBasicVelocity = Settings.Quantities.V.GetBasicVal() * Math.Sin(Settings.Quantities.Α.GetBasicVal()) - Settings.Quantities.G.GetBasicVal() * t.GetBasicVal();
            return new Velocity(Math.Abs(computedBasicVelocity), UnitVelocity.Basic);
        }

        public Velocity GetVelocityX()
        {
            return new Velocity(Settings.Quantities.Α.IsRight() ? 0 : Math.Cos(Settings.Quantities.Α.GetBasicVal()) * Settings.Quantities.V.GetBasicVal(), UnitVelocity.Basic);
        }
    }
}