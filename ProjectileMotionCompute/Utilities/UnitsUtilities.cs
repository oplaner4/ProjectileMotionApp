using System;

namespace Utilities.Units
{
    public abstract class Unit
    {
        internal double Coefficient { get; private set; }

        public string Name { get; private set; }

        protected Unit(double coefficient, string name)
        {
            Coefficient = coefficient;
            Name = name;
        }
    }

    public sealed class UnitMass : Unit
    {
        public static readonly UnitMass Kilogram = new UnitMass(1.0, nameof(Kilogram));

        public static readonly Unit Basic = Kilogram;

        public static readonly UnitMass Gram = new UnitMass(0.001, nameof(Gram));

        public static readonly UnitMass Tonne = new UnitMass(1000.0, nameof(Tonne));

        public static readonly UnitMass Megatonne = new UnitMass(1000000000, nameof(Megatonne));

        public static readonly UnitMass Miligram = new UnitMass(0.000001, nameof(Miligram));

        public static readonly UnitMass Dekagram = new UnitMass(0.01, nameof(Dekagram));

        private UnitMass(double coefficient, string name) : base(coefficient, name) {}
    }

    public sealed class UnitAngle : Unit
    {
        public static readonly UnitAngle Radian = new UnitAngle(1.0, nameof(Radian));

        public static readonly UnitAngle Basic = Radian;

        public static readonly UnitAngle Gradian = new UnitAngle(Math.PI / 180.0 * 9.0 / 10.0, nameof(Gradian));

        public static readonly UnitAngle Degree = new UnitAngle(Math.PI / 180.0, nameof(Degree));

        private UnitAngle(double coefficient, string name) : base(coefficient, name) { }
    }

    public sealed class UnitGravAcceleration : Unit
    {
        public static readonly UnitGravAcceleration MetrePerSquareSecond = new UnitGravAcceleration(1.0, nameof(MetrePerSquareSecond));

        public static readonly UnitGravAcceleration Basic = MetrePerSquareSecond;

        private UnitGravAcceleration(double coefficient, string name) : base(coefficient, name) { }
    }

    public sealed class UnitVelocity : Unit
    {
        public static readonly UnitVelocity MetrePerSecond = new UnitVelocity(1.0, nameof(MetrePerSecond));

        public static readonly UnitVelocity Basic = MetrePerSecond;

        public static readonly UnitVelocity KilometrePerHour = new UnitVelocity(0.277777778, nameof(KilometrePerHour));

        public static readonly UnitVelocity MilePerHour = new UnitVelocity(0.44704, nameof(MilePerHour));

        public static readonly UnitVelocity FootPerSecond = new UnitVelocity(0.3048, nameof(FootPerSecond));

        public static readonly UnitVelocity FurlongPerFortnight = new UnitVelocity(0.000166309524, nameof(FurlongPerFortnight));

        public static readonly UnitVelocity Knot = new UnitVelocity(0.514444444, nameof(Knot));

        public static readonly UnitVelocity Mach = new UnitVelocity(340.29, nameof(Mach));

        private UnitVelocity(double coefficient, string name) : base(coefficient, name) { }
    }

    public sealed class UnitLength : Unit
    {
        public static readonly UnitLength Metre = new UnitLength(1.0, nameof(Metre));

        public static readonly UnitLength Basic = Metre;

        public static readonly UnitLength Foot = new UnitLength(0.3048, nameof(Foot));

        public static readonly UnitLength Mile = new UnitLength(1000 * 1.609344, nameof(Mile));

        public static readonly UnitLength Yard = new UnitLength(0.9144, nameof(Yard));

        public static readonly UnitLength Furlong = new UnitLength(201.16800, nameof(Furlong));

        public static readonly UnitLength NauticalMile = new UnitLength(1852.0, nameof(NauticalMile));

        public static readonly UnitLength Kilometre = new UnitLength(1000.0, nameof(Kilometre));

        public static readonly UnitLength Inch = new UnitLength(0.0254, nameof(Inch));

        public static readonly UnitLength Milimetre = new UnitLength(0.001, nameof(Milimetre));

        public static readonly UnitLength Centimetre = new UnitLength(0.01, nameof(Centimetre));

        public static readonly UnitLength Decimetre = new UnitLength(0.1, nameof(Decimetre));

        private UnitLength(double coefficient, string name) : base(coefficient, name) { }
    }

    public sealed class UnitTime : Unit
    {
        public static readonly UnitTime Second = new UnitTime(1.0, nameof(Second));

        public static readonly UnitTime Basic = Second;

        public static readonly UnitTime Milisecond = new UnitTime(0.001, nameof(Milisecond));

        public static readonly UnitTime Minute = new UnitTime(60.0, nameof(Minute));

        public static readonly UnitTime Hour = new UnitTime(60.0 * 60.0, nameof(Hour));

        public static readonly UnitTime Day = new UnitTime(60.0 * 60.0 * 24.0, nameof(Day));

        public static readonly UnitTime Week = new UnitTime(60.0 * 60.0 * 24.0 * 7.0, nameof(Week));

        public static readonly UnitTime Month = new UnitTime(60.0 * 60.0 * 24.0 * 30.0, nameof(Month));

        public static readonly UnitTime Year = new UnitTime(60.0 * 60.0 * 24.0 * 365.25, nameof(Year));

        private UnitTime(double coefficient, string name) : base(coefficient, name) { }
    }

    public sealed class UnitDensity : Unit
    {
        public static readonly UnitDensity KilogramPerCubicMetre = new UnitDensity(1.0, nameof(KilogramPerCubicMetre));

        public static readonly UnitDensity Basic = KilogramPerCubicMetre;

        public static readonly UnitDensity KilogramPerCubicDecimetre = new UnitDensity(0.001, nameof(KilogramPerCubicDecimetre));

        public static readonly UnitDensity GramPerCubicDecimetre = new UnitDensity(1.0, nameof(GramPerCubicDecimetre));

        private UnitDensity(double coefficient, string name) : base(coefficient, name) { }
    }

    public sealed class UnitArea : Unit
    {

        public static readonly UnitArea SquareMetre = new UnitArea(1.0, nameof(SquareMetre));

        public static readonly UnitArea Basic = SquareMetre;

        public static readonly UnitArea SquareFoot = new UnitArea(Math.Pow(0.3048, 2.0), nameof(SquareFoot));

        public static readonly UnitArea SquareMile = new UnitArea(Math.Pow(1000 * 1.609344, 2.0), nameof(SquareMile));

        public static readonly UnitArea SquareYard = new UnitArea(Math.Pow(0.9144, 2.0), nameof(SquareYard));

        public static readonly UnitArea SquareFurlong = new UnitArea(Math.Pow(201.16800, 2.0), nameof(SquareFurlong));

        public static readonly UnitArea SquareNauticalMile = new UnitArea(Math.Pow(1852.0, 2.0), nameof(SquareNauticalMile));

        public static readonly UnitArea SquareKilometre = new UnitArea(Math.Pow(1000.0, 2.0), nameof(SquareKilometre));

        public static readonly UnitArea SquareInch = new UnitArea(Math.Pow(0.0254, 2.0), nameof(SquareInch));

        public static readonly UnitArea SquareMilimetre = new UnitArea(Math.Pow(0.001, 2.0), nameof(SquareMilimetre));

        public static readonly UnitArea SquareCentimetre = new UnitArea(Math.Pow(0.01, 2.0), nameof(SquareCentimetre));

        public static readonly UnitArea SquareDecimetre = new UnitArea(Math.Pow(0.1, 2.0), nameof(SquareDecimetre));

        public static readonly UnitArea Hectare = new UnitArea(10000.0, nameof(Hectare));

        public static readonly UnitArea Acre = new UnitArea(100.0, nameof(Acre));

        private UnitArea(double coefficient, string name) : base(coefficient, name) { }
    }
}