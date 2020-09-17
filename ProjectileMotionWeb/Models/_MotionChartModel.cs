using ProjectileMotionSource.Func;
using ProjectileMotionSource.Point;
using ProjectileMotionSource.WithResistance.Func;
using ProjectileMotionWeb.Classes;
using ProjectileMotionWeb.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace ProjectileMotionWeb.Models
{
    public class _MotionChartModel : BaseModel
    {
        public string GetSpeciallySerializedTrajectory(List<ProjectileMotionPoint> listPoints)
        {
            ProjectileMotionPoint farthestPoint = Motion.GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest);
            return new JsonSerializerHelper(
                listPoints.Select(p => new _MotionChartPoint(p, p.T == farthestPoint.T))
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
            DegradedMotion = motion.Degrade();
            ShowMotionWithoutResistanceTrajectoryToo = motion.Settings.ShowMotionWithoutResistanceTrajectoryToo;
        }

        public ProjectileMotion DegradedMotion { get; private set; }

        public bool ShowMotionWithoutResistanceTrajectoryToo { get; private set; }
    }
}