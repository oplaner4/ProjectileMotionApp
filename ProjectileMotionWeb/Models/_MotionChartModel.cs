using ProjectileMotionSource.Func;
using ProjectileMotionSource.Point;
using ProjectileMotionSource.WithRezistance.Func;
using ProjectileMotionWeb.Classes;
using ProjectileMotionWeb.Helpers;
using System.Collections.Generic;

namespace ProjectileMotionWeb.Models
{
    public class _MotionChartModel : BaseModel
    {
        public string GetSpeciallySerializedFunctionCourse(List<ProjectileMotionPoint> listFunctionCourse)
        {
            _MotionChartPoint[] points = new _MotionChartPoint[listFunctionCourse.Count];
            int i = 0;
            foreach (ProjectileMotionPoint point in listFunctionCourse)
            {
                points[i] = new _MotionChartPoint(point.Round());
                i++;
            }

            return new JsonSerializerHelper(points).Serialize();
        }

        public ProjectileMotion Motion { get; private set; }

        public _MotionChartModel(ProjectileMotion motion, bool showMotionWithoutRezistanceCourseToo)
        {
            Motion = motion;
            DegradedMotion = Motion;
            ShowMotionWithoutRezistanceCourseToo = showMotionWithoutRezistanceCourseToo;
        }


        public _MotionChartModel(ProjectileMotionWithRezistance motion, bool showMotionWithoutRezistanceCourseToo)
        {
            DegradedMotion = motion.Degrade();
            Motion = motion;
            ShowMotionWithoutRezistanceCourseToo = showMotionWithoutRezistanceCourseToo;
        }

        public ProjectileMotion DegradedMotion { get; private set; }

        public bool ShowMotionWithoutRezistanceCourseToo { get; private set; }
    }
}