﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Utilities.Exceptions;
using Utilities.Units;

namespace Utilities.Quantities
{
    /// <summary>
    /// Physical quantity.
    /// </summary>
    public abstract class Quantity
    {
        public Quantity(double val)
        {
            if (double.IsInfinity(val) || double.IsNaN(val))
            {
                throw new InvalidQuantityValueException("The quantity value must be numeric and finite.");
            }

            Val = val;
        }

        /// <summary>
        /// The value of the physical quantity.
        /// </summary>
        public double Val { get; protected set; }

        public Quantity RoundVal(int roundDigits)
        {
            Val = GetRoundedVal(roundDigits);
            return this;
        }

        public double GetRoundedVal(int roundDigits)
        {
            return Math.Round(Val, roundDigits);
        }

        public static double CompareToleratedDifference = Math.Pow(10, -12);

        public enum CheckValueCompareWithZero
        {
            LargerOrEqualToZero,
            LargerThanZero
        }

        public void CheckValueWithException (CheckValueCompareWithZero compare, string message)
        {
            if (
                (compare == CheckValueCompareWithZero.LargerOrEqualToZero && Val < -CompareToleratedDifference) || 
                (compare == CheckValueCompareWithZero.LargerThanZero && Val <= -CompareToleratedDifference)
            )
            { 
                throw new InvalidQuantityValueException(message);
            }
        }
    }

    public abstract class QuantityWithUnit : Quantity {
        public QuantityWithUnit(double val, Unit unit) : base(val)
        {
            Unit = unit ?? throw new ArgumentNullException(nameof(unit), "Unit object cannot be null.");
        }

        public Unit Unit { get; private set; }


        public double GetBasicVal ()
        {
            return Val * Unit.Coefficient;
        }

        public double GetConvertedVal(Unit to)
        {
            return GetBasicVal() / to.Coefficient;
        }

        public Quantity Convert(Unit to)
        {
            Val = GetConvertedVal(to);
            Unit = to;

            return this;
        }

        public static bool operator > (QuantityWithUnit q1, QuantityWithUnit q2)
        {
            return q1.GetBasicVal() > q2.GetBasicVal();
        }

        public static bool operator < (QuantityWithUnit q1, QuantityWithUnit q2)
        {
            return q1.GetBasicVal() < q2.GetBasicVal();
        }

        public static bool operator >= (QuantityWithUnit q1, QuantityWithUnit q2)
        {
            return q1.GetBasicVal() >= q2.GetBasicVal();
        }

        public static bool operator <= (QuantityWithUnit q1, QuantityWithUnit q2)
        {
            return q1.GetBasicVal() <= q2.GetBasicVal();
        }

        public static bool operator != (QuantityWithUnit q1, QuantityWithUnit q2)
        {
            return Math.Abs(q1.GetBasicVal() - q2.GetBasicVal()) > CompareToleratedDifference;
        }

        public static bool operator == (QuantityWithUnit q1, QuantityWithUnit q2)
        {
            return Math.Abs(q1.GetBasicVal() - q2.GetBasicVal()) <= CompareToleratedDifference;
        }

        public override bool Equals(object obj)
        {
            var unit = obj as QuantityWithUnit;
            return unit != null &&
                   EqualityComparer<Unit>.Default.Equals(Unit, unit.Unit);
        }

        public override int GetHashCode()
        {
            return -1325969601 + EqualityComparer<Unit>.Default.GetHashCode(Unit);
        }
    }

    public class Angle : QuantityWithUnit
    {
        public Angle(double val, UnitAngle unit) : base(val, unit)
        {}

        public Angle Convert(UnitAngle to)
        {
            base.Convert(to);
            return this;
        }


        public new Angle RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }
    }

    public class ElevationAngle : Angle
    {
        public enum ElevationAngleTypes
        {
            Right,
            Horizontal
        }

        private static readonly Dictionary<ElevationAngleTypes, double> ElevationAngleTypesDic = new Dictionary<ElevationAngleTypes, double>()
        {
            { ElevationAngleTypes.Right, Math.PI / 2 }, { ElevationAngleTypes.Horizontal, 0.0 }
        };

        public ElevationAngle(ElevationAngleTypes type) : base(ElevationAngleTypesDic[type], UnitAngle.Basic)
        { }

        public bool IsRight()
        {
            return this == new ElevationAngle(ElevationAngleTypes.Right);
        }

        public static double GetElevationAngleValue(ElevationAngleTypes type)
        {
            return ElevationAngleTypesDic[type];
        }

        public ElevationAngle(double val, UnitAngle unit) : base(val, unit)
        {
            if (this > new ElevationAngle(ElevationAngleTypes.Right).RoundVal(12))
            {
                throw new InvalidElevationAngleException(BuidRightAngleExceptionMessage());
            }

            CheckValueWithException(CheckValueCompareWithZero.LargerOrEqualToZero, "An elevation angle must be larger or equal to zero");
        }

        public new ElevationAngle RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }


        public new ElevationAngle Convert(UnitAngle to)
        {
            base.Convert(to);
            return this;
        }


        private string BuidRightAngleExceptionMessage()
        {
            string mess = string.Empty;
            string sep = ", ";
            foreach (Angle rightAngle in GetRightAngles())
                mess += sep + rightAngle.Val.ToString(CultureInfo.InvariantCulture) + " " + rightAngle.Unit.Name;

            return "An elevation angle must be smaller or equal to the right angle (" + mess.Substring(sep.Length) + ")";
        }


        private List<Angle> GetRightAngles()
        {
            List<Angle> ret = new List<Angle>();

            foreach (UnitAngle un in new List<UnitAngle>() { UnitAngle.Radian, UnitAngle.Degree, UnitAngle.Gradian })
            {
                ret.Add(new ElevationAngle(ElevationAngleTypes.Right).Convert(un));
            }

            return ret;
        }
    }

    public class GravAcceleration : QuantityWithUnit
    {
        public GravAcceleration(double val, UnitGravAcceleration unit) : base(val, unit)
        {
            CheckValueWithException(CheckValueCompareWithZero.LargerThanZero, "A gravitation acceleration must be larger than zero");
        }

        public GravAcceleration(GravAccelerations val = GravAccelerations.Earth) : base(GetGravAccelerationValue(val), UnitGravAcceleration.Basic)
        {}

        /// <summary>
        /// Gravitation accelerations of planets.
        /// </summary>
        public enum GravAccelerations
        {
            Earth,
            Sun,
            Mercury,
            Venus,
            Mars,
            Jupiter,
            Saturn,
            Uranus,
            Neptune,
            Pluto
        };

        private static readonly Dictionary<GravAccelerations, double> GravAccelerationsDic = new Dictionary<GravAccelerations, double>()
        {
            { GravAccelerations.Sun, 274.98}, {GravAccelerations.Mercury, 3.70}, {GravAccelerations.Venus, 8.87}, {GravAccelerations.Earth, 9.80665}, {GravAccelerations.Mars, 3.71}, {GravAccelerations.Jupiter, 23.12}, {GravAccelerations.Saturn, 8.96}, {GravAccelerations.Uranus, 7.77}, {GravAccelerations.Neptune, 11.00}, {GravAccelerations.Pluto, 0.72}
        };

        /// <summary>
        /// Gets the value of gravitation acceleration.
        /// </summary>
        /// <param name="g">Any value from predefined gravitation accelerations enum.</param>
        public static double GetGravAccelerationValue(GravAccelerations g)
        {
            return GravAccelerationsDic[g];
        }

        public new GravAcceleration RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }


        public GravAcceleration Convert(UnitGravAcceleration to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class Velocity : QuantityWithUnit
    {
        /// <summary>
        /// Constructor for initial velocity.
        /// </summary>
        /// <param name="val">The value of velocity. Larger or equal to zero.</param>
        public Velocity(double val, UnitVelocity unit) : base(val, unit)
        {
            CheckValueWithException(CheckValueCompareWithZero.LargerOrEqualToZero, "A velocity must be larger or equal to zero");
        }

        public new Velocity RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }


        public Velocity Convert(UnitVelocity to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class InitialVelocity : Velocity
    {
        public InitialVelocity(double val, UnitVelocity unit) : base(val, unit)
        {}

        public new InitialVelocity RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }


        public new InitialVelocity Convert(UnitVelocity to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class Length : QuantityWithUnit
    {
        public Length(double val, UnitLength unit) : base(val, unit)
        {
            CheckValueWithException(CheckValueCompareWithZero.LargerOrEqualToZero, "The length or height must be larger or equal to zero");
        }

        public new Length RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }


        public Length Convert(UnitLength to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class InitialHeight : Length
    {
        public InitialHeight(double val, UnitLength unit) : base(val, unit)
        { }

        public new InitialHeight RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }

        public new InitialHeight Convert(UnitLength to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class MaximalHeight : Length
    {
        public MaximalHeight(double val, UnitLength unit) : base(val, unit)
        {
        }

        public new MaximalHeight RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }
        public new MaximalHeight Convert(UnitLength to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class Time : QuantityWithUnit
    {
        public Time(double val, UnitTime unit) : base(val, unit)
        {
            CheckValueWithException(CheckValueCompareWithZero.LargerOrEqualToZero, "The time or duration must be larger or equal to zero");
        }

        public new Time RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }

        public Time Convert(UnitTime to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class Duration : Time
    {
        public Duration(double val, UnitTime unit) : base(val, unit)
        {}

        public new Duration RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }

        public new Duration Convert(UnitTime to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class Area : QuantityWithUnit
    {
        public Area (double val, UnitArea unit) : base (val, unit)
        {
            CheckValueWithException(CheckValueCompareWithZero.LargerThanZero, "An area must be larger than zero");
        }


        public new Area RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }


        public Area Convert(UnitArea to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class FrontalArea : Area
    {
        public FrontalArea (double val, UnitArea unit) : base (val, unit) { }

        public new FrontalArea RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }


        public new FrontalArea Convert(UnitArea to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class Mass : QuantityWithUnit
    {
        public Mass(double val, UnitMass unit) : base(val, unit)
        {
            CheckValueWithException(CheckValueCompareWithZero.LargerThanZero, "The mass must be larger than zero");
        }


        public new Mass RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }

        public Mass Convert(UnitMass to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class Density : QuantityWithUnit
    {
        public Density(double val, UnitDensity unit) : base(val, unit)
        {
            CheckValueWithException(CheckValueCompareWithZero.LargerThanZero, "The density must be larger than zero");
        }


        public Density(Densities val) : base(GetDensityValue(val), UnitDensity.Basic)
        {}

        public enum Densities {
            Air,
            carbonDioxide,
            Nitrogen,
            Helium,
            Argon
        }


        private static readonly Dictionary<Densities, double> DensitiesDic = new Dictionary<Densities, double>()
        {
            { Densities.Air, 1.274 }, { Densities.carbonDioxide, 1.997 }, { Densities.Nitrogen, 1.251 }, { Densities.Helium, 0.178 }, { Densities.Argon, 1.7572 }
        };


        public static double GetDensityValue(Densities ρ)
        {
            return DensitiesDic[ρ];
        }

        public new Density RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }

        public Density Convert(UnitDensity to)
        {
            base.Convert(to);
            return this;
        }
    }

    public class DragCoefficient : Quantity
    {
        public DragCoefficient (double val) : base (val) {
            CheckValueWithException(CheckValueCompareWithZero.LargerThanZero, "A drag coefficient must be larger than zero");
        }

        public DragCoefficient(DragCoefficients val) : base(GetDragCoefficientValue(val))
        { }

        public enum DragCoefficients
        {
            Sphere,
            HalfSphere,
            Cone,
            Cube,
            AngledCube,
            LongCylinder,
            ShortCylinder,
            StreamlinedBody,
            StreamlinedHalfBody
        };

        private static readonly Dictionary<DragCoefficients, double> DragCoefficientsDic = new Dictionary<DragCoefficients, double>()
        {
            { DragCoefficients.Sphere, 0.47 }, { DragCoefficients.HalfSphere, 0.42 },{ DragCoefficients.Cone, 0.5 },{ DragCoefficients.Cube, 1.05 },{ DragCoefficients.AngledCube, 0.8 },{ DragCoefficients.LongCylinder, 0.82 },{ DragCoefficients.ShortCylinder, 1.15 },{ DragCoefficients.StreamlinedBody, 0.04 },{ DragCoefficients.StreamlinedHalfBody, 0.09 }
        };

        public static double GetDragCoefficientValue(DragCoefficients c)
        {
            return DragCoefficientsDic[c];
        }

        public new DragCoefficient RoundVal(int roundDigits)
        {
            base.RoundVal(roundDigits);
            return this;
        }
    }
}