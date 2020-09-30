using ProjectileMotionSource.Func;
using ProjectileMotionSource.Point;
using ProjectileMotionSource.WithResistance.Func;
using ProjectileMotionWeb.Classes;
using ProjectileMotionWeb.Helpers;
using System.Linq;

namespace ProjectileMotionWeb.Models
{
    public class _MotionChartModel : BaseModel
    {
        public string GetSpeciallySerializedTrajectory(bool usingDegradedMotion)
        {
            ProjectileMotionTrajectory trajectory = (usingDegradedMotion ? DegradedMotion : Motion).Trajectory;
            ProjectileMotionPoint farthestPoint = trajectory.GetFarthestPoint();
            ProjectileMotionPoint highestPoint = trajectory.GetHighestPoint();
            ProjectileMotionPoint finalPoint = trajectory.GetFinalPoint();

            return new JsonSerializerHelper(
                trajectory.GetPointsList().Select(p => new _MotionChartPoint(p, p.T == farthestPoint.T, p.T == highestPoint.T, p.T == finalPoint.T, Motion.Settings.RoundDigits))
                ).Serialize();
        }

        public ProjectileMotion Motion { get; private set; }

        public _MotionChartModel(ProjectileMotion motion)
        {
            Motion = motion;
            DegradedMotion = null;
            ShowMotionWithoutResistanceTrajectoryToo = false;
        }


        public _MotionChartModel(ProjectileMotionWithResistance motion)
        {
            Motion = motion;
            DegradedMotion = motion.NeglectResistance();
            ShowMotionWithoutResistanceTrajectoryToo = motion.Settings.ShowMotionWithoutResistanceTrajectoryToo;
        }

        public ProjectileMotion DegradedMotion { get; private set; }

        public bool ShowMotionWithoutResistanceTrajectoryToo { get; private set; }
    }
}