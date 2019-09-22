using ProjectileMotionSource.Func;

namespace ProjectileMotionWeb.Models
{
    public class DisplayMotionModel : DisplayMotionBaseModel
    {
        public DisplayMotionModel(ProjectileMotion motion, bool showLargerMotionChart) : base (showLargerMotionChart)
        {
            Motion = motion;
        }

        public ProjectileMotion Motion { get; set; }
    }
}