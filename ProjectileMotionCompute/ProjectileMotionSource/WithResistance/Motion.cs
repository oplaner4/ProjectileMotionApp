using ProjectileMotionSource.Exceptions;
using ProjectileMotionSource.Func;
using Utilities.Quantities;

namespace ProjectileMotionSource.WithResistance.Func
{
    /// <summary>
    /// Projectile motion with resistance features.
    /// </summary>
    public class ProjectileMotionWithResistance : ProjectileMotion
    {
        public ProjectileMotionWithResistance(ProjectileMotionWithResistanceSettings settings) : base(settings)
        {
            Settings = settings;
            Trajectory = new ProjectileMotionWithResistanceTrajectory(settings);
        }

        public new ProjectileMotionWithResistanceSettings Settings { get; private set; }

        public override Velocity GetVelocityX()
        {
            throw new OnlySuperClassMethodException("This method cannot be used for motions with resistance");
        }

        public ProjectileMotion NeglectResistance()
        {
            ProjectileMotion DegradedMotion = new ProjectileMotion(new ProjectileMotionSettings(Settings.Quantities)
            {
                RoundDigits = Settings.RoundDigits
            });

            if (GetLength().Val > 0)
            {
                DegradedMotion.Settings.PointsForTrajectory = (int)(Settings.PointsForTrajectory * DegradedMotion.GetLength().Val / GetLength().Val);
            }

            return DegradedMotion;
        }
    }
}