using ProjectileMotionSource.Point;
using Utilities.Units;

namespace ProjectileMotionWeb.Classes
{
    public class _MotionChartPoint
    {
        public _MotionChartPoint(ProjectileMotionPoint point, bool isFarthest, bool isHighest, bool isFinal, int roundDigits)
        {
            X = point.X.GetRoundedVal(roundDigits);
            Y = point.Y.GetRoundedVal(roundDigits);
            T = point.T.GetRoundedVal(roundDigits);
            Vx = point.Vx.GetRoundedVal(roundDigits);
            Vy = point.Vy.GetRoundedVal(roundDigits);
            V = point.GetVelocity().GetRoundedVal(roundDigits);

            IsFarthest = isFarthest;
            IsHighest = isHighest;
            IsFinal = isFinal;
            TMiliseconds = point.T.GetConvertedVal(UnitTime.Milisecond);
        }

        public double X { get; private set; }

        public double Y { get; private set; }
        public double T { get; private set; }

        public double Vx { get; private set; }

        public double Vy { get; private set; }

        public double V { get; private set; }

        public bool IsFarthest { get; private set; }
        public bool IsHighest { get; private set; }
        public bool IsFinal { get; private set; }
        public double TMiliseconds { get; private set; }
    }
}