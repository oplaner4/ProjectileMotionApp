using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectileMotionSource.Func;
using Utilities.Units;
using Utilities.Quantities;
using System.Collections.Generic;
using System;
using ProjectileMotionSource.Exceptions;
using Utilities.Exceptions;

namespace ProjectileMotionUnitTest
{
    [TestClass]
    public class MotionUnitTest
    {
        private readonly MaximalHeight MaxH = new MaximalHeight(10.077991469230138, UnitLength.Metre);
        private readonly Length Len = new Length(41.835561907661194, UnitLength.Metre);
        private readonly Duration Dur = new Duration(2.7716331533676772, UnitTime.Second);
        private readonly InitialVelocity V = new InitialVelocity(72, UnitVelocity.KilometrePerHour);
        private readonly ElevationAngle Α = new ElevationAngle(41, UnitAngle.Degree);
        private readonly InitialHeight H = new InitialHeight(130, UnitLength.Centimetre);
        private readonly GravAcceleration G = new GravAcceleration(GravAcceleration.GravAccelerations.Earth);

        public int UsePoints = 6000;

        public MotionUnitTestUtilities Utilities { get; set; }

        public MotionUnitTest()
        {
            Utilities = new MotionUnitTestUtilities(0.09);
        }

        [TestMethod]
        public void TestAssignments()
        {
            List<ProjectileMotionQuantities> setQuantities = new List<ProjectileMotionQuantities> {
                new ProjectileMotionQuantities(Dur, Α, V, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(Len, V, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(Len, Α, V, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(Len, Α, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(MaxH, V, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(MaxH, Α, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(MaxH, Α, V, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(Dur, Α, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(Dur, V, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(Α, Len, Dur, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(V, Len, Dur, G, Utilities.UnitsOfResults),
            };

            foreach (ProjectileMotionQuantities quantities in setQuantities)
            {
                Utilities.AlmostSame(quantities.Α, Α);
                Utilities.AlmostSame(quantities.V, V);
                Utilities.AlmostSame(quantities.G, G);
                Utilities.AlmostSame(quantities.H, H);
            }

            Assert.IsTrue(
                new ProjectileMotion(
                    new ProjectileMotionSettings(new ProjectileMotionQuantities(V, new InitialHeight(0, UnitLength.Basic), G))
                ).Settings.Quantities.Α == new ElevationAngle(45, UnitAngle.Degree)
            );

            Assert.IsTrue(
                new ProjectileMotion(
                    new ProjectileMotionSettings(new ProjectileMotionQuantities(V, H, G))
                ).Settings.Quantities.Α < new ElevationAngle(45, UnitAngle.Degree)
            );
        }

        [TestMethod]
        public void TestInvalidAssignments()
        {
            foreach (Func<ProjectileMotionQuantities> quantitiesF in new List<Func<ProjectileMotionQuantities>>() {
                () => new ProjectileMotionQuantities(Dur, Α, new InitialVelocity(2000, UnitVelocity.MetrePerSecond),
                G, Utilities.UnitsOfResults),
            })
            {
                Assert.ThrowsException<UnableToComputeQuantityException>(
                    () => quantitiesF(), "should have thrown"
                );
            }

            Assert.ThrowsException<InvalidElevationAngleException>(
                () => new ElevationAngle(91, UnitAngle.Degree), "should have thrown"
            );
        }

        [TestMethod]
        public void TestCorrectResultsMotionRight()
        {
            ProjectileMotion motionRight = new ProjectileMotion(
                new ProjectileMotionSettings(
                    new ProjectileMotionQuantities(V, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right), H, G, Utilities.UnitsOfResults)
                )
                {
                    PointsForTrajectory = UsePoints
                }
            );

            Utilities.AlmostSame(motionRight.GetLength(), new Length(0, UnitLength.Basic));
            Utilities.AlmostSame(motionRight.GetTimeHighest(), new Time(motionRight.GetDur().GetBasicVal() / 2, UnitTime.Basic));
            Utilities.AlmostSame(motionRight.GetTimeFarthest(), motionRight.GetTimeHighest());
            Utilities.AlmostSame(motionRight.GetMaxDistance(), new Length(motionRight.GetMaxHeight().GetBasicVal() - H.GetBasicVal(), UnitLength.Basic));
            Utilities.AlmostSame(motionRight.GetMaxDistance(), motionRight.Trajectory.GetInitialPoint().GetDistanceFromPoint(motionRight.Trajectory.GetHighestPoint()));
            Utilities.AlmostSame(motionRight.Trajectory.GetArcLength(), new Length(2 * motionRight.GetMaxHeight().GetBasicVal() - H.GetBasicVal(), UnitLength.Basic));
            Utilities.AlmostSame(motionRight.Trajectory.GetAreaUnderArc(), new Area(0, UnitArea.Basic));
            Utilities.AlmostSame(motionRight.Trajectory.GetHighestPoint().Y, motionRight.GetMaxHeight());
            Utilities.AlmostSame(motionRight.Trajectory.GetFarthestPoint().Y, motionRight.Trajectory.GetHighestPoint().Y);
            Utilities.AlmostSame(motionRight.Trajectory.GetFarthestPoint().X, motionRight.Trajectory.GetHighestPoint().X);
            Utilities.AlmostSame(motionRight.Trajectory.GetInitialPoint().X, motionRight.Trajectory.GetFinalPoint().X);
            Utilities.AlmostSame(motionRight.Trajectory.GetInitialPoint().Y, new Length(motionRight.Trajectory.GetFinalPoint().Y.GetBasicVal() + H.GetBasicVal(), UnitLength.Basic));

            Utilities.AlmostSame(Utilities.GetTrajectoryLengthIteratively(motionRight), motionRight.Trajectory.GetArcLength());
            Utilities.AlmostSame(Utilities.GetAreaUnderTrajectoryIteratively(motionRight), motionRight.Trajectory.GetAreaUnderArc());
            Utilities.AlmostSame(Utilities.GetMaxHeightIteratively(motionRight), motionRight.GetMaxHeight());
            Utilities.AlmostSame(Utilities.GetMaxDistanceIteratively(motionRight), motionRight.GetMaxDistance());
        }

        [TestMethod]
        public void TestCorrectResultsMotionHorizontal()
        {
            ProjectileMotion motionHorizontal = new ProjectileMotion(
                new ProjectileMotionSettings(
                    new ProjectileMotionQuantities(V, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Horizontal),
                    new InitialHeight(20, UnitLength.Metre), G, Utilities.UnitsOfResults)
                )
                {
                    PointsForTrajectory = UsePoints
                }
            );

            Utilities.AlmostSame(motionHorizontal.GetTimeHighest(), new Time(0, UnitTime.Basic));
            Utilities.AlmostSame(motionHorizontal.GetTimeHighest(), motionHorizontal.Trajectory.GetInitialPoint().T);
            Utilities.AlmostSame(motionHorizontal.GetMaxDistance(), motionHorizontal.Trajectory.GetInitialPoint().GetDistanceFromPoint(motionHorizontal.Trajectory.GetFinalPoint()));
            Utilities.AlmostSame(motionHorizontal.Trajectory.GetHighestPoint().Y, motionHorizontal.GetMaxHeight());
            Utilities.AlmostSame(motionHorizontal.Trajectory.GetFarthestPoint().Y, motionHorizontal.Trajectory.GetFinalPoint().Y);
            Utilities.AlmostSame(motionHorizontal.Trajectory.GetFarthestPoint().X, motionHorizontal.Trajectory.GetFinalPoint().X);
            Utilities.AlmostSame(motionHorizontal.Trajectory.GetInitialPoint().X, motionHorizontal.Trajectory.GetHighestPoint().X);
            Utilities.AlmostSame(motionHorizontal.Trajectory.GetInitialPoint().Y, motionHorizontal.Trajectory.GetHighestPoint().Y);

            Utilities.AlmostSame(Utilities.GetTrajectoryLengthIteratively(motionHorizontal), motionHorizontal.Trajectory.GetArcLength());
            Utilities.AlmostSame(Utilities.GetAreaUnderTrajectoryIteratively(motionHorizontal), motionHorizontal.Trajectory.GetAreaUnderArc());
            Utilities.AlmostSame(Utilities.GetMaxHeightIteratively(motionHorizontal), motionHorizontal.GetMaxHeight());
            Utilities.AlmostSame(Utilities.GetMaxDistanceIteratively(motionHorizontal), motionHorizontal.GetMaxDistance());
        }

        [TestMethod]
        public void TestCorrectResultsMotionLow()
        {
            ProjectileMotion motion = new ProjectileMotion(
                new ProjectileMotionSettings(
                    new ProjectileMotionQuantities(V, Α, H, G, Utilities.UnitsOfResults)
                )
                {
                    PointsForTrajectory = UsePoints
                }
            );

            Utilities.AlmostSame(motion.GetDur(), Dur);
            Utilities.AlmostSame(motion.GetLength(), Len);
            Utilities.AlmostSame(motion.GetMaxHeight(), MaxH);

            Utilities.AlmostSame(Utilities.GetTrajectoryLengthIteratively(motion), motion.Trajectory.GetArcLength());
            Utilities.AlmostSame(Utilities.GetAreaUnderTrajectoryIteratively(motion), motion.Trajectory.GetAreaUnderArc());
            Utilities.AlmostSame(Utilities.GetMaxHeightIteratively(motion), motion.GetMaxHeight());
            Utilities.AlmostSame(Utilities.GetMaxDistanceIteratively(motion), motion.GetMaxDistance());
        }

        [TestMethod]
        public void TestCorrectResultsMotionHigh()
        {
            ProjectileMotion motion = new ProjectileMotion(
                new ProjectileMotionSettings(
                    new ProjectileMotionQuantities(V, new ElevationAngle(82, UnitAngle.Degree), H, G, Utilities.UnitsOfResults)
                )
                {
                    PointsForTrajectory = UsePoints
                }
            );

            Utilities.AlmostSame(Utilities.GetTrajectoryLengthIteratively(motion), motion.Trajectory.GetArcLength());
            Utilities.AlmostSame(Utilities.GetAreaUnderTrajectoryIteratively(motion), motion.Trajectory.GetAreaUnderArc());
            Utilities.AlmostSame(Utilities.GetMaxHeightIteratively(motion), motion.GetMaxHeight());
            Utilities.AlmostSame(Utilities.GetMaxDistanceIteratively(motion), motion.GetMaxDistance());

            Assert.IsTrue(motion.GetMaxDistance() > motion.Trajectory.GetFinalPoint().GetDistanceFromPoint(motion.Trajectory.GetInitialPoint()));
            Assert.IsTrue(motion.GetTimeFarthest() > motion.GetTimeHighest());
            Assert.IsTrue(motion.Trajectory.GetFarthestPoint().Y < motion.Trajectory.GetHighestPoint().Y);
        }

        [TestMethod]
        public void TestCorrectResultsMotionsDetailed()
        {
            double[] aValues = new double[] { 85, 0, 22, 30, 45, 67, 78, 90 };
            double[] vValues = new double[] { 54, 0, 12, 31, 60, 120, 400 };
            double[] hValues = new double[] { 0, 5, 14, 20, 60, 131, 225 };

            foreach (double a in aValues)
            {
                foreach (double h in hValues)
                {
                    foreach (double v in vValues)
                    {
                        ProjectileMotion motion = new ProjectileMotion(
                            new ProjectileMotionSettings(
                                new ProjectileMotionQuantities(
                                    new InitialVelocity(v, UnitVelocity.KilometrePerHour),
                                    new ElevationAngle(a, UnitAngle.Degree),
                                    new InitialHeight(h, UnitLength.Metre),
                                    G, Utilities.UnitsOfResults
                                )
                            )
                            {
                                PointsForTrajectory = UsePoints
                            }
                        );

                        string additionalMessage = string.Format(
                            "Used values: {0} °, {1} km/h, {2} m.",
                            a, v, h
                        );

                        Utilities.AlmostSame(Utilities.GetAreaUnderTrajectoryIteratively(motion), motion.Trajectory.GetAreaUnderArc(), additionalMessage);
                        Utilities.AlmostSame(Utilities.GetMaxHeightIteratively(motion), motion.GetMaxHeight(), additionalMessage);
                        Utilities.AlmostSame(Utilities.GetTrajectoryLengthIteratively(motion), motion.Trajectory.GetArcLength(), additionalMessage);
                        Utilities.AlmostSame(Utilities.GetMaxDistanceIteratively(motion), motion.GetMaxDistance(), additionalMessage);
                    }
                }
            }
        }
    }
}
