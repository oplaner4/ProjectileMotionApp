using System;
using Utilities.Quantities;
using ProjectileMotionSource.Saving;

namespace ProjectileMotionSource.Func
{
    /// <summary>
    /// Projectile motion features.
    /// </summary>
    public class ProjectileMotion
    {
        /// <summary>
        /// Constructor for a projectile motion.
        /// </summary>
        /// <param name="settings">Settings object for projectile motion. It cannot be null.</param>
        public ProjectileMotion(ProjectileMotionSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings), "Settings object cannot be null.");
            Trajectory = new ProjectileMotionTrajectory(Settings);
            Saving = new ProjectileMotionFilesSaving(this);
        }

        /// <summary>
        /// Projectile motion settings.
        /// </summary>
        public ProjectileMotionSettings Settings { get; protected set; }

        public virtual ProjectileMotionTrajectory Trajectory { get; protected set; }

        public ProjectileMotionFilesSaving Saving { get; private set; }

        /// <summary>
        /// Gets constant horizontal velocity in the set unit for velocity.
        /// </summary>
        public virtual Velocity GetVelocityX()
        {
            return Trajectory.GetInitialPoint().Vx;
        }

        /// <summary>
        /// Gets the distance between the farthest point (from the beginning) and the beginning in the set unit for length.
        /// </summary>
        public virtual Length GetMaxDistance()
        {
            return Trajectory.GetFarthestPoint().GetDistance();
        }

        /// <summary>
        /// Gets duration of the motion in the set unit for time.
        /// </summary>
        public virtual Time GetDur()
        {
            return Trajectory.GetFinalPoint().T;
        }

        /// <summary>
        /// Gets the length in the set unit for length.
        /// </summary>
        public virtual Length GetLength()
        {
            return Trajectory.GetFinalPoint().X;
        }

        /// <summary>
        /// Gets height of the highest point.
        /// </summary>
        public virtual Length GetMaxHeight()
        {
            return Trajectory.GetHighestPoint().Y;
        }

        /// <summary>
        /// Gets the time in the point that is the farthest point from the beginning.
        /// </summary>
        /// <returns></returns>
        public virtual Time GetTimeFarthest()
        {
            return Trajectory.GetFarthestPoint().T;
        }

        /// <summary>
        /// Gets the time of the highest point.
        /// </summary>
        public virtual Time GetTimeHighest()
        {
            return Trajectory.GetHighestPoint().T;
        }
    }
}