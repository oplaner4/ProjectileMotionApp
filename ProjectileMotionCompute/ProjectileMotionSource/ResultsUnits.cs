using Utilities.Units;

namespace ProjectileMotionSource.Func
{
    /// <summary>
    /// Units of projectile motion results Quantities.
    /// </summary>
    public class ProjectileMotionResultsUnits
    {
        public ProjectileMotionResultsUnits ()
        {
            Velocity = UnitVelocity.Basic;
            Angle = UnitAngle.Basic;
            Length = UnitLength.Basic;
            Time = UnitTime.Basic;
            GravAcceleration = UnitGravAcceleration.Basic;
            Area = UnitArea.Basic;
        }

        /// <summary>
        /// The unit of velocity.
        /// </summary>
        public UnitVelocity Velocity { get; set; }


        /// <summary>
        /// The unit of length.
        /// </summary>
        public UnitLength Length { get; set; }


        /// <summary>
        /// The unit of time.
        /// </summary>
        public UnitTime Time { get; set; }


        /// <summary>
        /// The unit of area.
        /// </summary>
        public UnitArea Area { get; set; }


        /// <summary>
        /// The unit of angle.
        /// </summary>
        public UnitAngle Angle { get; set; }


        /// <summary>
        /// The unit of gravitation acceleration.
        /// </summary>
        public UnitGravAcceleration GravAcceleration { get; set; }
    }
}