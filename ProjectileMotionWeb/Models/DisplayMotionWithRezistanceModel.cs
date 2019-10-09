using ProjectileMotionSource.WithRezistance.Func;

namespace ProjectileMotionWeb.Models
{
    public class DisplayMotionWithRezistanceModel : DisplayMotionBaseModel
    {
        public DisplayMotionWithRezistanceModel(ProjectileMotionWithRezistance motion, bool showLargerMotionChart) : base (showLargerMotionChart)
        {
            Motion = motion;
        }

        public ProjectileMotionWithRezistance Motion { get; set; }
    }
}