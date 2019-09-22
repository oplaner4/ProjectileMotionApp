using ProjectileMotionSource.WithRezistance.Func;

namespace ProjectileMotionWeb.Models
{
    public class DisplayMotionWithRezistanceModel : DisplayMotionBaseModel
    {
        public DisplayMotionWithRezistanceModel(ProjectileMotionWithRezistance motion, bool showMotionWithoutRezistanceCourseToo, bool showLargerMotionChart) : base (showLargerMotionChart)
        {
            Motion = motion;
            ShowMotionWithoutRezistanceCourseToo = showMotionWithoutRezistanceCourseToo;
        }

        public ProjectileMotionWithRezistance Motion { get; set; }

        public bool ShowMotionWithoutRezistanceCourseToo { get; set; }
    }
}