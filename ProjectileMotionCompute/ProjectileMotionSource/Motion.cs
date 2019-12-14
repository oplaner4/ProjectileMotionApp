using System;
using Utilities.Quantities;
using Utilities.Units;
using MathNet.Numerics;
using System.Collections.Generic;
using System.Linq;
using ProjectileMotionSource.Saving;
using ProjectileMotionSource.Point;

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
            Saving = new ProjectileMotionFilesSaving(this);
        }


        /// <summary>
        /// Projectile motion settings.
        /// </summary>
        public ProjectileMotionSettings Settings { get; private set; }

        public ProjectileMotionFilesSaving Saving { get; protected set; }

        public virtual ProjectileMotionPoint GetPoint (Time t)
        {
            return new ProjectileMotionPoint(this, t);
        }

        public virtual ProjectileMotionPoint GetPoint (ProjectileMotionPoint.ProjectileMotionPointTypes type)
        {
            return new ProjectileMotionPoint(this, type);
        }

        /// <summary>
        /// Gets constant horizontal velocity in the set unit for velocity.
        /// </summary>
        public virtual Velocity GetVelocityX()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Initial).Round().Vx.Convert(Settings.Quantities.Units.Velocity);
        }

        /// <summary>
        /// Gets vertical velocity in the set unit for velocity.
        /// </summary>
        /// <param name="t">The time in the set unit.</param>
        public virtual Velocity GetVelocityY(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().Vy.Convert(Settings.Quantities.Units.Velocity);
        }


        /// <summary>
        /// Gets the X coordination of the point in the set unit for length.
        /// </summary>
        /// <param name="t">The time in the set unit.</param>
        public virtual Length GetX(double t)
        {
           return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().X.Convert(Settings.Quantities.Units.Length);
        }


        /// <summary>
        /// Gets the velocity in the point in the set unit for velocity.
        /// </summary>
        /// <param name="t">The time in the set unit.</param>
        public virtual Velocity GetVelocity(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).GetVelocity(Settings.Quantities.Units.Velocity).RoundVal(Settings.RoundDigits);
        }


        /// <summary>
        /// Gets the Y coordination of the point in the set unit for length.
        /// </summary>
        /// <param name="t">The time in the set unit.</param>
        public virtual Length GetY(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().Y.Convert(Settings.Quantities.Units.Length);
        }


        /// <summary>
        /// Gets the distance between the point and beginning in the set unit for length.
        /// </summary>
        /// <param name="t">The time in the set unit.</param>
        public virtual Length GetDistance(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).GetDistance(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        /// <summary>
        /// Gets the distance between two points.
        /// </summary>
        /// <param name="t1">The time of the first point.</param>
        /// <param name="t2">The time of the second point.</param>
        public virtual Length GetDistanceBetweenTwoPoints(double t1, double t2)
        {
            return GetPoint(new Time(t1, Settings.Quantities.Units.Time)).GetDistanceFromPoint(GetPoint(new Time(t2, Settings.Quantities.Units.Time)), Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        /// <summary>
        /// Gets the distance between the farthest point (from the beginning) and the beginning in the set unit for length.
        /// </summary>
        public virtual Length GetMaxDistance()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest).GetDistance(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        /// <summary>
        /// Gets duration of the motion in the set unit for time.
        /// </summary>
        public virtual Time GetDur()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Final).Round().T.Convert(Settings.Quantities.Units.Time);
        }


        /// <summary>
        /// Gets the length in the set unit for length.
        /// </summary>
        public virtual Length GetLength()
        {
            return GetLength(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }


        protected Length GetLength(UnitLength unitLength)
        {
            if (Settings.Quantities.Α.IsRight())
            {
                return new Length(0, unitLength);
            }

            return new Length(Settings.Quantities.V.GetBasicVal() * Math.Cos(Settings.Quantities.Α.GetBasicVal()) * (Settings.Quantities.V.GetBasicVal() * Math.Sin(Settings.Quantities.Α.GetBasicVal()) + Math.Sqrt(Math.Pow(Settings.Quantities.V.GetBasicVal() * Math.Sin(Settings.Quantities.Α.GetBasicVal()), 2.0) + 2 * Settings.Quantities.G.GetBasicVal() * Settings.Quantities.H.GetBasicVal())) / Settings.Quantities.G.GetBasicVal(), UnitLength.Basic).Convert(unitLength);
        }


        /// <summary>
        /// Gets the length of the arc elapsed by the projectile (circuit) using definite integral in the set unit for length.
        /// </summary>
        public virtual Length GetArcLength()
        {
            return GetArcLength(Settings.Quantities.Units.Length).RoundVal(Settings.RoundDigits);
        }

        protected Length GetArcLength(UnitLength unitLength)
        {
            if (Settings.Quantities.Α.IsRight())
            {
                return new Length(2.0 * GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest).Y.GetConvertedVal(unitLength), unitLength);
            }

            return new Length(Integrate.OnClosedInterval(x => Math.Sqrt(1 + Math.Pow(Math.Tan(Settings.Quantities.Α.GetBasicVal()) - (Settings.Quantities.G.GetBasicVal() / Math.Pow(Settings.Quantities.V.GetBasicVal() * Math.Cos(Settings.Quantities.Α.GetBasicVal()), 2.0)) * x, 2.0)), 0, GetLength(UnitLength.Basic).Val), UnitLength.Basic).Convert(unitLength);
        }


        /// <summary>
        /// Gets the area bounded by an arc and X axis in the set unit for area.
        /// </summary>
        public virtual Area GetAreaUnderArc()
        {
            return GetAreaUnderArc(Settings.Quantities.Units.Area).RoundVal(Settings.RoundDigits);
        }

        protected Area GetAreaUnderArc(UnitArea unitArea)
        {
            if (Settings.Quantities.Α.IsRight())
            {
                return new Area(0, unitArea);
            }

            return new Area(Math.Pow(GetLength(UnitLength.Basic).Val, 2.0) * Math.Tan(Settings.Quantities.Α.GetBasicVal()) / 2.0 - Math.Pow(GetLength(UnitLength.Basic).Val, 3.0) * Settings.Quantities.G.GetBasicVal() / (6.0 * Math.Pow(Settings.Quantities.V.GetBasicVal() * Math.Cos(Settings.Quantities.Α.GetBasicVal()), 2.0)) + GetLength(UnitLength.Basic).Val * Settings.Quantities.H.GetBasicVal(), UnitArea.Basic).Convert(unitArea); 
        }


        /// <summary>
        /// Gets height of the highest point.
        /// </summary>
        public virtual Length GetMaxHeight()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest).Round().Y.Convert(Settings.Quantities.Units.Length);
        }


        /// <summary>
        /// Gets coordinates of the point in the set unit for length.
        /// </summary>
        /// <param name="t">the time in the set unit.</param>
        public virtual double[] GetCoords(double t)
        {
            return GetPoint(new Time(t, Settings.Quantities.Units.Time)).Round().GetCoords(Settings.Quantities.Units.Length);
        }


        /// <summary>
        /// Gets the time of the point when fallen in the set unit for time.
        /// </summary>
        public virtual Time GetTimeFallen()
        {
            return GetDur();
        }

        /// <summary>
        /// Gets coordinates of the farthest point in the set unit for length.
        /// </summary>
        public virtual double[] GetCoordsFarthest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest).Round().GetCoords(Settings.Quantities.Units.Length);
        }

        /// <summary>
        /// Gets coordinates of the point when fallen in the set unit for length.
        /// </summary>
        public virtual double[] GetCoordsFallen()
        {
            return new double[2] { GetLength(Settings.Quantities.Units.Length).GetRoundedVal(Settings.RoundDigits), 0 };
        }


        /// <summary>
        /// Gets the time in the point that is the farthest point from the beginning.
        /// </summary>
        /// <returns></returns>
        public virtual Time GetTimeFarthest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest).Round().T.Convert(Settings.Quantities.Units.Time);
        }

        /// <summary>
        /// Gets coordinates of the highest point in the unit set for length.
        /// </summary>
        public virtual double[] GetCoordsHighest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest).Round().GetCoords(Settings.Quantities.Units.Length);
        }


        /// <summary>
        /// Gets the time of the highest point.
        /// </summary>
        public virtual Time GetTimeHighest()
        {
            return GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest).Round().T.Convert(Settings.Quantities.Units.Time);
        }


        /// <summary>
        /// Gets the trajectory with the set number of points that describe the projectile motion.
        /// </summary>
        /// <returns>An array of the points coordinates.</returns>
        public virtual double[][] GetTrajectory()
        {
            return GetListPointsOfTrajectory().Select(p => p.Round().GetCoords(Settings.Quantities.Units.Length)).ToArray();
        }


        public virtual List<ProjectileMotionPoint> GetListPointsOfTrajectory()
        {
            List<ProjectileMotionPoint> ret = new List<ProjectileMotionPoint>();

            ProjectileMotionPoint finalPoint = GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Final);
            ProjectileMotionPoint highestPoint = GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Highest);
            ProjectileMotionPoint farthestPoint = GetPoint(ProjectileMotionPoint.ProjectileMotionPointTypes.Farthest);

            for (int i = 0; i < Settings.PointsForTrajectory - 1; i++)
            {
                Time now = new Time(finalPoint.T.GetBasicVal() / (Settings.PointsForTrajectory - 1) * i, UnitTime.Basic).RoundVal(Settings.RoundDigits);
                Time next = new Time(finalPoint.T.GetBasicVal() / (Settings.PointsForTrajectory - 1) * (i + 1), UnitTime.Basic).RoundVal(Settings.RoundDigits);

                if (now.GetBasicVal() != highestPoint.T.GetBasicVal() && now.GetBasicVal() != farthestPoint.T.GetBasicVal())
                {
                    ret.Add(GetPoint(now));
                }

                if (next >= highestPoint.T && now <= highestPoint.T)
                {
                    ret.Add(highestPoint);
                }
                else if (next >= farthestPoint.T && now <= farthestPoint.T)
                {
                    ret.Add(farthestPoint);
                }
            }

            if (ret.Last().Y.Val > 0) ret.Add(finalPoint);

            return ret;
        }


        /// <summary>
        /// Saves basic information to the defined .txt file to the defined folder path.
        /// </summary>
        /// <returns>Projectile Motion instance.</returns>
        public virtual ProjectileMotion SaveInfoToTxt()
        {
            Saving.InfoToTxt();
            return this;
        }

        /// <summary>
        /// Saves data to the defined .csv file to the defined folder path.
        /// </summary>
        /// <returns>Projectile Motion instance.</returns>
        public virtual ProjectileMotion SaveDataToCsv()
        {
            Saving.DataToCsv();
            return this;
        }
    }
}