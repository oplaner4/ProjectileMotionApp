using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities.Units;
using Utilities.Quantities;
using ProjectileMotionSource.WithResistance.Func;

namespace ProjectileMotionUnitTest
{
    [TestClass]
    public class MotionWithResistanceUnitTest
    {
        private readonly MaximalHeight MaxH = new MaximalHeight(13.6774, UnitLength.Metre);
        private readonly Length Len = new Length(78.5142, UnitLength.Metre);
        private readonly Length ArcLength = new Length(85.0101, UnitLength.Metre);
        private readonly Length MaxDist = new Length(78.525, UnitLength.Metre);
        private readonly Area AreaUnderArc = new Area(731.1633508, UnitArea.SquareMetre);
        private readonly Duration Dur = new Duration(2.827, UnitTime.Second);

        private readonly InitialVelocity V = new InitialVelocity(73.06, UnitVelocity.MetrePerSecond);
        private readonly ElevationAngle Α = new ElevationAngle(20, UnitAngle.Degree);
        private readonly Density Ρ = new Density(Density.Densities.Air);
        private readonly FrontalArea A = new FrontalArea(4.90625, UnitArea.SquareInch);
        private readonly DragCoefficient C = new DragCoefficient(0.55);
        private readonly Mass M = new Mass(56.7, UnitMass.Gram);
        private readonly InitialHeight H = new InitialHeight(130, UnitLength.Centimetre);
        private readonly GravAcceleration G = new GravAcceleration(GravAcceleration.GravAccelerations.Earth);

        public int UsePoints = 6000;

        public MotionUnitTestUtilities Utilities { get; set; }

        public MotionWithResistanceUnitTest()
        {
            Utilities = new MotionUnitTestUtilities(0.09);
        }

        [TestMethod]
        public void TestCorrectResultsMotionWithResistanceRight()
        {
            ProjectileMotionWithResistance motionRight = new ProjectileMotionWithResistance(
                new ProjectileMotionWithResistanceSettings(
                    new ProjectileMotionWithResistanceQuantities(V,
                    new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right),
                    H, G, M, Ρ, A, C, Utilities.UnitsOfResults)
                )
                {
                    PointsForTrajectory = UsePoints
                }
            );

            Utilities.AlmostSame(motionRight.GetLength(), new Length(0, UnitLength.Basic));
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
        public void TestCorrectResultsMotionWithResistanceHorizontal()
        {
            ProjectileMotionWithResistance motionHorizontal = new ProjectileMotionWithResistance(
                new ProjectileMotionWithResistanceSettings(
                    new ProjectileMotionWithResistanceQuantities(V,
                    new ElevationAngle(ElevationAngle.ElevationAngleTypes.Horizontal),
                    H, G, M, Ρ, A, C, Utilities.UnitsOfResults)
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
        public void TestCorrectResultsMotionWithResistanceLow()
        {
            ProjectileMotionWithResistance motion = new ProjectileMotionWithResistance(
                new ProjectileMotionWithResistanceSettings(
                    new ProjectileMotionWithResistanceQuantities(V, Α, H, G, M, Ρ, A, C, Utilities.UnitsOfResults)
                )
                {
                    PointsForTrajectory = UsePoints
                }
            );

            Utilities.AlmostSame(motion.GetDur(), Dur);
            Utilities.AlmostSame(motion.GetLength(), Len);
            Utilities.AlmostSame(motion.GetMaxHeight(), MaxH);
            Utilities.AlmostSame(motion.GetMaxDistance(), MaxDist);
            Utilities.AlmostSame(motion.Trajectory.GetAreaUnderArc(), AreaUnderArc);
            Utilities.AlmostSame(motion.Trajectory.GetArcLength(), ArcLength);
        }

        [TestMethod]
        public void TestCorrectResultsMotionWithResistanceHigh()
        {
            ProjectileMotionWithResistance motion = new ProjectileMotionWithResistance(
                new ProjectileMotionWithResistanceSettings(
                    new ProjectileMotionWithResistanceQuantities(V, new ElevationAngle(82, UnitAngle.Degree), H, G, M, Ρ, A, C, Utilities.UnitsOfResults)
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
    }
}
