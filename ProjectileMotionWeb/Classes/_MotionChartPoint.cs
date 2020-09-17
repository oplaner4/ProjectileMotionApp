using ProjectileMotionSource.Point;
using Utilities.Units;

namespace ProjectileMotionWeb.Classes
{
    public class _MotionChartPoint
    {
        public _MotionChartPoint(ProjectileMotionPoint point, bool isFarthest)
        {
            point.Round();
            X = point.X.Val;
            Y = point.Y.Val;
            T = point.T.Val;

            IsFarthest = isFarthest;
            IsHighest = point.IsHighest;
            TMiliseconds = point.T.GetConvertedVal(UnitTime.Milisecond);
        }

        public double X { get; private set; }

        public double Y { get; private set; }
        public double T { get; private set; }
        public bool IsFarthest { get; private set; }
        public bool IsHighest { get; private set; }
        public double TMiliseconds { get; private set; }
    }
}