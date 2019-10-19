using ProjectileMotionSource.Func;

namespace ProjectileMotionWeb.Models
{
    public class _MotionInformationModel : BaseModel
    {
        public _MotionInformationModel (ProjectileMotion motion)
        {
            Motion = motion;
        }

        public ProjectileMotion Motion { get; private set; }
    }
}