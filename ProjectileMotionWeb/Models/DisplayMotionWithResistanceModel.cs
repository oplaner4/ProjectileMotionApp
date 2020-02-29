using ProjectileMotionSource.WithResistance.Func;

namespace ProjectileMotionWeb.Models
{
    public class DisplayMotionWithResistanceModel : DisplayMotionBaseModel
    {
        public DisplayMotionWithResistanceModel(ProjectileMotionWithResistance motion, bool showLargerMotionChart) : base (showLargerMotionChart)
        {
            Motion = motion;
        }

        public ProjectileMotionWithResistance Motion { get; set; }
    }
}