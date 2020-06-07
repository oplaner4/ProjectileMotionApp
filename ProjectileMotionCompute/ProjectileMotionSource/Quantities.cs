using System;
using Utilities.Quantities;
using Utilities.Units;
using System.Collections.Generic;
using ProjectileMotionSource.Exceptions;
using EquationSolver = Solver1D.Solver1D;

namespace ProjectileMotionSource.Func
{
    /// <summary>
    /// Projectile motion Quantities.
    /// </summary>
    public class ProjectileMotionQuantities
    {
        /// <summary>
        /// 1. Constructor for a projectile motion Quantities.
        /// </summary>
        /// <param name="v">An initial velocity.</param>
        /// <param name="α">An elevation angle.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(InitialVelocity v, ElevationAngle α, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            Α = α;
            H = h;
            G = g;

            UsedAssignmentType = AssignmentsTypes.Basic;
        }

        /// <summary>
        /// 2. Constructor for a projectile motion Quantities. Computes an elevation angle based on the duration.
        /// </summary>
        /// <param name="v">An initial velocity.</param>
        /// <param name="dur">The duration.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Duration dur, InitialVelocity v, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            H = h;
            G = g;
            Α = new ElevationAngle(
                GetResultWithComputeExpection(Math.Asin((Math.Pow(dur.GetBasicVal(), 2.0) * G.GetBasicVal() - 2 * H.GetBasicVal()) / (2.0 * V.GetBasicVal() * dur.GetBasicVal()))), 
                UnitAngle.Basic).Convert(Units.Angle);

            UsedAssignmentType = AssignmentsTypes.ElevationAngleByDuration;
        }

        /// <summary>
        /// 3. Constructor for a projectile motion Quantities. Computes an initial velocity based on the duration.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="dur">The duration.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Duration dur, ElevationAngle α, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            Α = α;
            H = h;
            G = g;

            

            V = new InitialVelocity(
                GetResultWithComputeExpection((Math.Pow(dur.GetBasicVal(), 2.0) * G.GetBasicVal() - 2 * H.GetBasicVal()) / (2.0 * Math.Sin(Α.GetBasicVal()) * dur.GetBasicVal())),
                UnitVelocity.Basic).Convert(Units.Velocity);

            UsedAssignmentType = AssignmentsTypes.InitialVelocityByDuration;
        }

        /// <summary>
        /// 4. Constructor for a projectile motion Quantities. Computes an initial height based on the duration.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="dur">The duration.</param>
        /// <param name="v">An initial velocity.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Duration dur, ElevationAngle α, InitialVelocity v, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            Α = α;
            G = g;
            H = new InitialHeight(
                GetResultWithComputeExpection((Math.Pow(dur.GetBasicVal(), 2.0) * G.GetBasicVal() - 2.0 * Math.Sin(Α.GetBasicVal()) * dur.GetBasicVal() * V.GetBasicVal()) / 2.0), 
                UnitLength.Basic).Convert(Units.Length);

            UsedAssignmentType = AssignmentsTypes.InitialHeightByDuration;
        }

        /// <summary>
        /// 5. Constructor for a projectile motion Quantities. Computes an initial height based on the length.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="l">The length.</param>
        /// <param name="v">An initial velocity.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Length l, ElevationAngle α, InitialVelocity v, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            Α = α;
            G = g;
            H = new InitialHeight(
                GetResultWithComputeExpection((Math.Pow(l.GetBasicVal(), 2.0) * G.GetBasicVal() * 1 / Math.Pow(Math.Cos(Α.GetBasicVal()), 2.0) - 2.0 * l.GetBasicVal() * Math.Pow(V.GetBasicVal(), 2.0) * Math.Tan(Α.GetBasicVal())) / (2.0 * Math.Pow(V.GetBasicVal(), 2.0))),
                UnitLength.Basic).Convert(Units.Length);


            UsedAssignmentType = AssignmentsTypes.InitialHeightByLength;
        }

        /// <summary>
        /// 6. Constructor for a projectile motion Quantities. Computes an initial velocity based on the length.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="l">The length.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Length l, ElevationAngle α, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            Α = α;
            H = h;
            G = g;
            V = new InitialVelocity(
               GetResultWithComputeExpection(l.GetBasicVal() * Math.Sqrt(G.GetBasicVal() * 1 / Math.Cos(Α.GetBasicVal())) / Math.Sqrt(2.0 * l.GetBasicVal() * Math.Sin(Α.GetBasicVal()) + 2.0 * H.GetBasicVal() * Math.Cos(Α.GetBasicVal()))), 
                UnitVelocity.Basic).Convert(Units.Velocity);

            UsedAssignmentType = AssignmentsTypes.InitialVelocityByLength;
        }

        /// <summary>
        /// 7. Constructor for a projectile motion Quantities. Computes an elevation angle based on the length.
        /// </summary>
        /// <param name="v">An initial velocity.</param>
        /// <param name="l">The length.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(Length l, InitialVelocity v, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            H = h;
            G = g;

            Α = new ElevationAngle(
                    GetResultWithComputeExpection(EquationSolver.BisectionFindRoot(a => V.GetBasicVal() * Math.Cos(a) * (V.GetBasicVal() * Math.Sin(a) + Math.Sqrt(Math.Pow(V.GetBasicVal() * Math.Sin(a), 2) + 2.0 * G.GetBasicVal() * H.GetBasicVal())) / G.GetBasicVal() - l.GetBasicVal(), 0, new ElevationAngle(ElevationAngle.ElevationAngleTypes.Right).Val, 1E-4)),
                    UnitAngle.Basic
                ).Convert(Units.Angle);

            UsedAssignmentType = AssignmentsTypes.ElevationAngleByLength;
        }

        /// <summary>
        /// 8. Constructor for a projectile motion Quantities. Computes an initial velocity based on the maximal height.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="maxHeight">The maximal height.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(MaximalHeight maxHeight, ElevationAngle α, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            Α = α;
            H = h;
            G = g;
            V = new InitialVelocity(
               GetResultWithComputeExpection(Math.Sqrt(2.0 * G.GetBasicVal() * (maxHeight.GetBasicVal() - H.GetBasicVal()) / Math.Pow(Math.Sin(Α.GetBasicVal()), 2.0))),
                UnitVelocity.Basic).Convert(Units.Velocity);

            UsedAssignmentType = AssignmentsTypes.InitialVelocityByMaxHeight;
        }

        /// <summary>
        /// 9. Constructor for a projectile motion Quantities. Computes an initial height based on the maximal height.
        /// </summary>
        /// <param name="α">An elevation angle.</param>
        /// <param name="maxHeight">The maximal height.</param>
        /// <param name="v">An initial velocity.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(MaximalHeight maxHeight, ElevationAngle α, InitialVelocity v, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            Α = α;
            G = g;
            H = new InitialHeight(
               GetResultWithComputeExpection(maxHeight.GetBasicVal() - Math.Pow(V.GetBasicVal() * Math.Sin(Α.GetBasicVal()), 2.0) / (2.0 * G.GetBasicVal())), 
                UnitLength.Basic).Convert(Units.Length);

            UsedAssignmentType = AssignmentsTypes.InitialHeightByMaxHeight;
        }

        /// <summary>
        /// 10. Constructor for a projectile motion Quantities. Computes an elevation angle based on the maximal height.
        /// </summary>
        /// <param name="v">An initial velocity.</param>
        /// <param name="dur">The maximal height.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(MaximalHeight maxHeight, InitialVelocity v, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            H = h;
            G = g;
            

            Α = new ElevationAngle(
                GetResultWithComputeExpection(Math.Asin(Math.Sqrt(2.0 * G.GetBasicVal() * maxHeight.GetBasicVal() / Math.Pow(V.GetBasicVal(), 2)))),
                UnitAngle.Basic).Convert(Units.Angle);

            UsedAssignmentType = AssignmentsTypes.ElevationAngleByMaxHeight;
        }


        /// <summary>
        /// 11. Constructor for a projectile motion Quantities. Computes an elevation angle for motion with maximum range.
        /// </summary>
        /// <param name="v">An initial velocity.</param>
        /// <param name="h">An initial height.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(InitialVelocity v, InitialHeight h, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            V = v;
            H = h;
            G = g;

            Α = new ElevationAngle(
                GetResultWithComputeExpection(Math.Acos(Math.Sqrt((2.0 * G.GetBasicVal() * H.GetBasicVal() + Math.Pow(V.GetBasicVal(), 2.0)) / (2.0 * G.GetBasicVal() * H.GetBasicVal() + 2.0 * Math.Pow(V.GetBasicVal(), 2.0))))), 
                UnitAngle.Basic).Convert(Units.Angle);

            UsedAssignmentType = AssignmentsTypes.ElevationAngleGetMaxRange;
        }

        /// <summary>
        /// 12. Constructor for a projectile motion Quantities. Computes an initial velocity based on the duration and the length.
        /// </summary>
        /// <param name="α">An initial velocity.</param>
        /// <param name="l">The length.</param>
        /// <param name="dur">The duration.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(ElevationAngle α, Length l, Duration dur, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();

            Α = α;
            G = g;
            V = new InitialVelocity(GetResultWithComputeExpection(l.GetBasicVal() / (Math.Cos(Α.GetBasicVal()) * dur.GetBasicVal())), UnitVelocity.Basic).Convert(Units.Velocity);
            H = new InitialHeight(GetResultWithComputeExpection(0.5 * G.GetBasicVal() * Math.Pow(dur.GetBasicVal(), 2.0) - Math.Sin(Α.GetBasicVal()) * V.GetBasicVal() * dur.GetBasicVal()), UnitLength.Basic).Convert(Units.Length);

            UsedAssignmentType = AssignmentsTypes.InitialVelocityByLengthAndDur;
        }

        /// <summary>
        /// 13. Constructor for a projectile motion Quantities. Computes an elevation angle based on the duration and the length.
        /// </summary>
        /// <param name="v">An initial velocity.</param>
        /// <param name="l">The length.</param>
        /// <param name="dur">The duration.</param>
        /// <param name="g">A gravitation acceleration of the planet.</param>
        /// <param name="units">The units of Quantities. By default metre per second, radian, metre and metre per square second.</param>
        public ProjectileMotionQuantities(InitialVelocity v, Length l, Duration dur, GravAcceleration g, ProjectileMotionResultsUnits units = null)
        {
            Units = units ?? new ProjectileMotionResultsUnits();


            G = g;
            V = v;
            Α = new ElevationAngle(GetResultWithComputeExpection(Math.Acos(l.GetBasicVal()/(V.GetBasicVal() * dur.GetBasicVal()))), UnitAngle.Basic).Convert(Units.Angle);
            H = new InitialHeight(GetResultWithComputeExpection(0.5 * G.GetBasicVal() * Math.Pow(dur.GetBasicVal(), 2.0) - Math.Sin(Α.GetBasicVal()) * V.GetBasicVal() * dur.GetBasicVal()), UnitLength.Basic).Convert(Units.Length);

            UsedAssignmentType = AssignmentsTypes.ElevationAngleByLengthAndDur;
        }


        public AssignmentsTypes UsedAssignmentType { get; set; }

        public enum AssignmentsTypes
        {
            Basic,
            InitialHeightByDuration,
            ElevationAngleByDuration,
            InitialVelocityByDuration,
            InitialHeightByMaxHeight,
            ElevationAngleByMaxHeight,
            InitialVelocityByMaxHeight,
            InitialHeightByLength,
            ElevationAngleByLength,
            InitialVelocityByLength,
            ElevationAngleGetMaxRange,
            InitialVelocityByLengthAndDur,
            ElevationAngleByLengthAndDur
        }


        public static Dictionary<AssignmentsTypes, string> AssignmentsTypesTranslations = new Dictionary<AssignmentsTypes, string>()
        {
            { AssignmentsTypes.Basic, "Basic quantities" },
            { AssignmentsTypes.InitialHeightByDuration, "An initial height by duration" },
            { AssignmentsTypes.ElevationAngleByDuration, "An elevation angle by duration" },
            { AssignmentsTypes.InitialVelocityByDuration, "An initial velocity by duration" },
            { AssignmentsTypes.InitialHeightByMaxHeight, "An initial height by maximal height" },
            { AssignmentsTypes.ElevationAngleByMaxHeight, "An elevation angle by maximal height" },
            { AssignmentsTypes.InitialVelocityByMaxHeight, "An initial velocity by maximal height" },
            { AssignmentsTypes.InitialHeightByLength, "An initial height by length" },
            { AssignmentsTypes.ElevationAngleByLength, "An elevation angle by length" },
            { AssignmentsTypes.InitialVelocityByLength, "An initial velocity by length" },
            { AssignmentsTypes.ElevationAngleGetMaxRange, "An elevation angle to get maximal range" },
            { AssignmentsTypes.InitialVelocityByLengthAndDur, "An initial velocity by length and duration" },
            { AssignmentsTypes.ElevationAngleByLengthAndDur, "An elevation angle by length and duration" }
        };


        protected double GetResultWithComputeExpection(double? solverRootResult)
        {
            if (solverRootResult.HasValue)
            {
                return GetResultWithComputeExpection(solverRootResult.Value);
            }

            return GetResultWithComputeExpection(-1);
        }

        protected double GetResultWithComputeExpection(double expectedResult)
        {
            if (expectedResult < -Quantity.CompareToleratedDifference || double.IsInfinity(expectedResult) || double.IsNaN(expectedResult))
            {
                throw new UnableToComputeQuantityException(UNABLETOCOMPUTEQUANTITYTEXT);
            }

            return expectedResult;
        }


        public InitialVelocity V { get; private set; }

        public ElevationAngle Α { get; private set; }

        public InitialHeight H { get; private set; }

        public GravAcceleration G { get; private set; }

        private const string UNABLETOCOMPUTEQUANTITYTEXT = "Unable to find the definite real projectile motion. The combination of entered quantities is invalid or indefinite.";

        /// <summary>
        /// Units of projectile motion Quantities.
        /// </summary>
        public ProjectileMotionResultsUnits Units { get; private set; }
    }
}