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
        public int UsePoints = 6000;

        private readonly MaximalHeight MaxH = new MaximalHeight(10.077991469230138, UnitLength.Metre);
        private readonly Length Len = new Length(41.835561907661194, UnitLength.Metre);
        private readonly Duration Dur = new Duration(2.7716331533676772, UnitTime.Second);
        private readonly InitialVelocity V = new InitialVelocity(72, UnitVelocity.KilometrePerHour);
        private readonly ElevationAngle Α = new ElevationAngle(41, UnitAngle.Degree);
        private readonly InitialHeight H = new InitialHeight(130, UnitLength.Centimetre);
        private readonly GravAcceleration G = new GravAcceleration(GravAcceleration.GravAccelerations.Earth);

        public MotionUnitTestUtilities Utilities { get; set; }

        public MotionUnitTest()
        {
            Utilities = new MotionUnitTestUtilities(0.02);
        }

        [TestMethod]
        public void TestAssignmentsSpecialCases()
        {
            Duration nullDur = new Duration(0, UnitTime.Second);
            Duration largeDur = new Duration(20, UnitTime.Second);
            Length nullLen = new Length(0, UnitLength.Metre);
            MaximalHeight nullMaxH = new MaximalHeight(0, UnitLength.Metre);
            MaximalHeight smallMaxH = new MaximalHeight(2, UnitLength.Metre);
            Length smallLen = new Length(4, UnitLength.Metre);
            InitialVelocity nullV = new InitialVelocity(0, UnitVelocity.MetrePerSecond);
            ElevationAngle nullΑ = new ElevationAngle(ElevationAngle.ElevationAngleTypes.Horizontal);
            ElevationAngle rightΑ = new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right);
            InitialHeight nullH = new InitialHeight(0, UnitLength.Metre);

            // should not throw the compute exception
            new ProjectileMotionQuantities(Dur, nullΑ, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(Dur, rightΑ, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(Dur, Α, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullDur, Α, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullDur, nullΑ, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullDur, rightΑ, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullDur, nullΑ, V, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullDur, rightΑ, nullH, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullDur, Α, nullH, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullDur, nullΑ, nullH, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullDur, V, nullH, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(smallLen, V, nullH, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullLen, nullV, H, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(Len, nullΑ, V, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullLen, nullΑ, V, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(smallLen, nullΑ, H, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(smallLen, Α, nullH, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullLen, Α, nullH, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullLen, Α, H, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullLen, nullΑ, H, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(smallMaxH, V, nullH, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(MaxH, V, new InitialHeight(MaxH.GetConvertedVal(UnitLength.Metre), UnitLength.Metre),
                G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(MaxH, Α, new InitialHeight(MaxH.GetConvertedVal(UnitLength.Metre), UnitLength.Metre),
                G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullMaxH, Α, nullH, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(MaxH, Α, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(MaxH, nullΑ, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(MaxH, rightΑ, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullMaxH, rightΑ, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullMaxH, nullΑ, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullMaxH, Α, nullV, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullMaxH, nullΑ, V, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullΑ, Len, Dur, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(nullΑ, nullLen, Dur, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(Α, nullLen, Dur, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(Α, nullLen, nullDur, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(V, nullLen, nullDur, G, Utilities.UnitsOfResults);
            new ProjectileMotionQuantities(V, nullLen, largeDur, G, Utilities.UnitsOfResults);
        }

        [TestMethod]
        public void TestAssignments()
        {
            List<ProjectileMotionQuantities> setQuantities = new List<ProjectileMotionQuantities> {
                new ProjectileMotionQuantities(Dur, Α, V, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(Dur, Α, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(Dur, V, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(Len, V, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(Len, Α, V, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(Len, Α, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(MaxH, V, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(MaxH, Α, H, G, Utilities.UnitsOfResults),
                new ProjectileMotionQuantities(MaxH, Α, V, G, Utilities.UnitsOfResults),
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

            Utilities.AlmostSame(new ElevationAngle(44.115006, UnitAngle.Degree), new ProjectileMotion(
                    new ProjectileMotionSettings(new ProjectileMotionQuantities(V, H, G))
                ).Settings.Quantities.Α);
        }

        [TestMethod]
        public void TestInvalidAssignments()
        {
            int i = 0;

            foreach (Func<ProjectileMotionQuantities> quantitiesF in new List<Func<ProjectileMotionQuantities>>() {
                /* 1 */ () => new ProjectileMotionQuantities(Dur, Α, new InitialVelocity(2000, UnitVelocity.MetrePerSecond),
                            G, Utilities.UnitsOfResults),
                /* 2 */ () => new ProjectileMotionQuantities(Dur, new ElevationAngle(85, UnitAngle.Degree), V,
                            G, Utilities.UnitsOfResults),
                /* 3 */ () => new ProjectileMotionQuantities(Dur, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Horizontal), H,
                            G, Utilities.UnitsOfResults),
                /* 4 */ () => new ProjectileMotionQuantities(Dur, Α, new InitialHeight(99, UnitLength.Kilometre),
                            G, Utilities.UnitsOfResults),
                /* 5 */ () => new ProjectileMotionQuantities(Dur, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Horizontal),
                            new InitialHeight(0, UnitLength.Kilometre), G, Utilities.UnitsOfResults),
                /* 6 */ () => new ProjectileMotionQuantities(Dur, new InitialVelocity(4, UnitVelocity.MetrePerSecond), H,
                            G, Utilities.UnitsOfResults),
                /* 7 */ () => new ProjectileMotionQuantities(Dur, new InitialVelocity(0, UnitVelocity.MetrePerSecond),
                            new InitialHeight(0, UnitLength.Metre), G, Utilities.UnitsOfResults),
                /* 8 */ () => new ProjectileMotionQuantities(Dur, V, new InitialHeight(10, UnitLength.Kilometre),
                            G, Utilities.UnitsOfResults),
                /* 9 */ () => new ProjectileMotionQuantities(Len, new InitialVelocity(5, UnitVelocity.MetrePerSecond), H,
                            G, Utilities.UnitsOfResults),
               /* 10 */ () => new ProjectileMotionQuantities(Len, new InitialVelocity(0, UnitVelocity.MetrePerSecond), H,
                            G, Utilities.UnitsOfResults),
               /* 11 */ () => new ProjectileMotionQuantities(Len, V, new InitialHeight(H.GetConvertedVal(UnitLength.Metre) - 1, UnitLength.Metre),
                            G, Utilities.UnitsOfResults),
               /* 12 */ () => new ProjectileMotionQuantities(Len, V, new InitialHeight(0, UnitLength.Metre),
                            G, Utilities.UnitsOfResults),
               /* 13 */ () => new ProjectileMotionQuantities(Len, Α, new InitialVelocity(0, UnitVelocity.MilePerHour),
                            G, Utilities.UnitsOfResults),
               /* 14 */ () => new ProjectileMotionQuantities(Len, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right), V,
                            G, Utilities.UnitsOfResults),
               /* 15 */ () => new ProjectileMotionQuantities(Len, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Horizontal),
                            new InitialVelocity(0, UnitVelocity.MetrePerSecond), G, Utilities.UnitsOfResults),
               /* 16 */ () => new ProjectileMotionQuantities(Len, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right), H,
                            G, Utilities.UnitsOfResults),
               /* 17 */ () => new ProjectileMotionQuantities(Len, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Horizontal),
                            new InitialHeight(0, UnitLength.Metre), G, Utilities.UnitsOfResults),
               /* 18 */ () => new ProjectileMotionQuantities(MaxH, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Horizontal), H,
                            G, Utilities.UnitsOfResults),
               /* 19 */ () => new ProjectileMotionQuantities(MaxH, V, new InitialHeight(MaxH.GetConvertedVal(UnitLength.Metre) + 1, UnitLength.Metre),
                            G, Utilities.UnitsOfResults),
               /* 20 */ () => new ProjectileMotionQuantities(MaxH, new InitialVelocity(0, UnitVelocity.MetrePerSecond),
                            new InitialHeight(0, UnitLength.Metre), G, Utilities.UnitsOfResults),
               /* 21 */ () => new ProjectileMotionQuantities(MaxH, new InitialVelocity(1, UnitVelocity.KilometrePerHour), H,
                            G, Utilities.UnitsOfResults),
               /* 22 */ () => new ProjectileMotionQuantities(MaxH, new InitialVelocity(0, UnitVelocity.KilometrePerHour), H,
                            G, Utilities.UnitsOfResults),
               /* 23 */ () => new ProjectileMotionQuantities(MaxH, new ElevationAngle(88, UnitAngle.Degree), V,
                            G, Utilities.UnitsOfResults),
               /* 24 */ () => new ProjectileMotionQuantities(MaxH, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right), V,
                            G, Utilities.UnitsOfResults),
               /* 25 */ () => new ProjectileMotionQuantities(MaxH, Α, new InitialVelocity(100, UnitVelocity.MetrePerSecond),
                            G, Utilities.UnitsOfResults),
               /* 26 */ () => new ProjectileMotionQuantities(new ElevationAngle(78, UnitAngle.Degree), Len, Dur,
                            G, Utilities.UnitsOfResults),
               /* 27 */ () => new ProjectileMotionQuantities(new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right), Len, Dur,
                            G, Utilities.UnitsOfResults),
               /* 28 */ () => new ProjectileMotionQuantities(Α, Len, new Duration(0.1, UnitTime.Milisecond),
                            G, Utilities.UnitsOfResults),
               /* 29 */ () => new ProjectileMotionQuantities(Α, Len, new Duration(0, UnitTime.Milisecond),
                            G, Utilities.UnitsOfResults),
               /* 30 */ () => new ProjectileMotionQuantities(new ElevationAngle(ElevationAngle.ElevationAngleTypes.Horizontal),
                            Len, new Duration(0, UnitTime.Milisecond), G, Utilities.UnitsOfResults),
               /* 31 */ () => new ProjectileMotionQuantities(new InitialVelocity(2, UnitVelocity.MetrePerSecond), Len, Dur,
                            G, Utilities.UnitsOfResults),
               /* 32 */ () => new ProjectileMotionQuantities(new InitialVelocity(0, UnitVelocity.MetrePerSecond), Len, Dur,
                            G, Utilities.UnitsOfResults),
               /* 33 */ () => new ProjectileMotionQuantities(V, new Length(150, UnitLength.Kilometre), Dur,
                            G, Utilities.UnitsOfResults),
               /* 34 */ () => new ProjectileMotionQuantities(V, new Length(0, UnitLength.Kilometre), Dur,
                            G, Utilities.UnitsOfResults),
               /* 35 */ () => new ProjectileMotionQuantities(V, Len, new Duration(0.1, UnitTime.Second),
                            G, Utilities.UnitsOfResults),
               /* 36 */ () => new ProjectileMotionQuantities(V, Len, new Duration(0, UnitTime.Second),
                            G, Utilities.UnitsOfResults),
               /* 37 */ () => new ProjectileMotionQuantities(new Length(0, UnitLength.Metre), V, new InitialHeight(0, UnitLength.Metre),
                            G, Utilities.UnitsOfResults),
               /* 38 */ () => new ProjectileMotionQuantities(new Length(0, UnitLength.Metre), new InitialVelocity(0, UnitVelocity.MetrePerSecond),
                            new InitialHeight(0, UnitLength.Metre), G, Utilities.UnitsOfResults),
               /* 39 */ () => new ProjectileMotionQuantities(new Length(0, UnitLength.Metre), new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right),
                            V, G, Utilities.UnitsOfResults),
               /* 40 */ () => new ProjectileMotionQuantities(new Length(0, UnitLength.Metre), Α,
                            new InitialVelocity(0, UnitVelocity.MetrePerSecond), G, Utilities.UnitsOfResults),
               /* 41 */ () => new ProjectileMotionQuantities(new Length(0, UnitLength.Metre), new InitialVelocity(0, UnitVelocity.MetrePerSecond),
                            new InitialHeight(0, UnitLength.Metre), G, Utilities.UnitsOfResults),
               /* 42 */ () => new ProjectileMotionQuantities(new Length(0, UnitLength.Metre), new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right),
                            H, G, Utilities.UnitsOfResults),
               /* 43 */ () => new ProjectileMotionQuantities(MaxH, new InitialVelocity(0, UnitVelocity.MetrePerSecond),
                            new InitialHeight(MaxH.GetConvertedVal(UnitLength.Metre), UnitLength.Metre), G, Utilities.UnitsOfResults),
               /* 44 */ () => new ProjectileMotionQuantities(new MaximalHeight(0, UnitLength.Metre), new InitialVelocity(0, UnitVelocity.MetrePerSecond),
                            new InitialHeight(0, UnitLength.Metre), G, Utilities.UnitsOfResults),
               /* 45 */ () => new ProjectileMotionQuantities(new MaximalHeight(0, UnitLength.Metre), V, new InitialHeight(0, UnitLength.Metre),
                            G, Utilities.UnitsOfResults),
               /* 46 */ () => new ProjectileMotionQuantities(MaxH, new InitialVelocity(0, UnitVelocity.MetrePerSecond),
                            new InitialHeight(MaxH.GetConvertedVal(UnitLength.Metre), UnitLength.Metre), G, Utilities.UnitsOfResults),
               /* 47 */ () => new ProjectileMotionQuantities(new MaximalHeight(0, UnitLength.Metre), new ElevationAngle(ElevationAngle.ElevationAngleTypes.Horizontal),
                            new InitialHeight(0, UnitLength.Metre), G, Utilities.UnitsOfResults),
               /* 48 */ () => new ProjectileMotionQuantities(new MaximalHeight(0, UnitLength.Metre), new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right),
                            V, G, Utilities.UnitsOfResults),
               /* 49 */ () => new ProjectileMotionQuantities(new MaximalHeight(0, UnitLength.Metre), Α,
                            V, G, Utilities.UnitsOfResults),
               /* 50 */ () => new ProjectileMotionQuantities(new ElevationAngle(0, UnitAngle.Degree), new Length(0, UnitLength.Metre),
                            new Duration(0, UnitTime.Second), G, Utilities.UnitsOfResults),
               /* 51 */ () => new ProjectileMotionQuantities(new ElevationAngle(0, UnitAngle.Degree), Len,
                            new Duration(0, UnitTime.Second), G, Utilities.UnitsOfResults),
               /* 52 */ () => new ProjectileMotionQuantities(Α, Len, new Duration(0, UnitTime.Second),
                            G, Utilities.UnitsOfResults),
               /* 53 */ () => new ProjectileMotionQuantities(V, Len, new Duration(0, UnitTime.Second),
                            G, Utilities.UnitsOfResults),
               /* 54 */ () => new ProjectileMotionQuantities(new InitialVelocity(0, UnitVelocity.MetrePerSecond), Len,
                            new Duration(0, UnitTime.Second), G, Utilities.UnitsOfResults),
               /* 55 */ () => new ProjectileMotionQuantities(new InitialVelocity(0, UnitVelocity.MetrePerSecond), new Length(0, UnitLength.Metre),
                            Dur, G, Utilities.UnitsOfResults),
               /* 56 */ () => new ProjectileMotionQuantities(new InitialVelocity(0, UnitVelocity.MetrePerSecond), new Length(0, UnitLength.Metre),
                            new Duration(0, UnitTime.Second), G, Utilities.UnitsOfResults),
        })
            {
                Assert.ThrowsException<UnableToComputeQuantityException>(
                    () => quantitiesF(), string.Format("{0}. assignment should have been invalid.", i + 1)
                );

                i++;
            }

            Assert.ThrowsException<InvalidElevationAngleException>(
                () => new ElevationAngle(91, UnitAngle.Degree), "The elevation angle 91 degrees should have thrown exception."
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
