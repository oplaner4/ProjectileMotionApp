using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectileMotionSource.Func;
using Utilities.Units;
using Utilities.Quantities;
using System.Collections.Generic;
using System;
using ProjectileMotionSource.Point;

namespace ProjectileMotionUnitTest
{
    public class MotionUnitTestUtilities
    {
        public readonly ProjectileMotionResultsUnits UnitsOfResults = new ProjectileMotionResultsUnits()
        {
            Length = UnitLength.Kilometre,
            Time = UnitTime.Milisecond,
            Velocity = UnitVelocity.FurlongPerFortnight,
            Angle = UnitAngle.Gradian,
            Area = UnitArea.SquareInch,
            GravAcceleration = UnitGravAcceleration.MetrePerSquareSecond
        };

        public MotionUnitTestUtilities (double requiredPrecision)
        {
            RequiredPrecision = requiredPrecision;
        }

        private double RequiredPrecision { get; set; }

        public void AlmostSame(QuantityWithUnit q1, QuantityWithUnit q2, string additionalMessage = "")
        {
            double q1B = q1.GetBasicVal();
            double q2B = q2.GetBasicVal();

            Assert.IsTrue(Math.Abs(q1B - q2B) <= RequiredPrecision,
                string.Format("{0} {1} is not almost same as {2} {3}. Comparing {4} and {5} with precision {6}. ",
                    q1.Val,
                    q1.Unit.Name,
                    q2.Val,
                    q2.Unit.Name,
                    q1B, q2B,
                    RequiredPrecision
                ) + additionalMessage
            );
        }

        public Area GetAreaUnderTrajectoryIteratively (ProjectileMotion motion)
        {
            double areaUnderTrajectory = 0;
            List<ProjectileMotionPoint> points = motion.Trajectory.GetPointsList();

            for (int i = 0; i < points.Count - 1; i++)
            {

                areaUnderTrajectory += (points[i + 1].X.GetBasicVal() - points[i].X.GetBasicVal()) * (points[i].Y.GetBasicVal() + points[i + 1].Y.GetBasicVal()) / 2;
            }

            return new Area(areaUnderTrajectory, UnitArea.Basic);
        }

        public Length GetTrajectoryLengthIteratively (ProjectileMotion motion)
        {
            double trajectoryLength = 0;
            List<ProjectileMotionPoint> points = motion.Trajectory.GetPointsList();

            for (int i = 0; i < points.Count - 1; i++)
            {
                trajectoryLength += points[i].GetDistanceFromPoint(points[i + 1]).GetBasicVal();
            }

            return new Length(trajectoryLength, UnitLength.Basic);
        }

        public Length GetMaxHeightIteratively (ProjectileMotion motion)
        {
            double maxH = motion.Trajectory.GetInitialPoint().Y.GetBasicVal();

            foreach (ProjectileMotionPoint point in motion.Trajectory.GetPointsList())
            {
                double y = point.Y.GetBasicVal();

                if (y > maxH)
                {
                    maxH = y;
                }
            }

            return new Length(maxH, UnitLength.Basic);
        }

        public Length GetMaxDistanceIteratively (ProjectileMotion motion)
        {
            double maxD = 0;

            foreach (ProjectileMotionPoint point in motion.Trajectory.GetPointsList())
            {
                double d = point.GetDistance().GetBasicVal();

                if (d > maxD)
                {
                    maxD = d;
                }
            }

            return new Length(maxD, UnitLength.Basic);
        }
    }
}
