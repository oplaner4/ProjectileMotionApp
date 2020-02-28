using ProjectileMotionSource.Func;
using Utilities.Quantities;

namespace ProjectileMotionSource.WithResistance.Func
{
    public class ProjectileMotionWithResistanceQuantities : ProjectileMotionQuantities
    {
        public ProjectileMotionWithResistanceQuantities(InitialVelocity v, ElevationAngle α, InitialHeight h, GravAcceleration g, Mass m, Density ρ, FrontalArea a, DragCoefficient c, ProjectileMotionResultsUnits units = null) : base(v, α, h, g, units)
        {
            M = m;
            Ρ = ρ;
            C = c;
            A = a;
        }

        public Mass M { get; private set; }

        public Density Ρ { get; private set; }

        public DragCoefficient C { get; private set; }

        public FrontalArea A { get; private set; }
    }
}