using System.Web.Mvc;
using ProjectileMotionWeb.Helpers;
using Utilities.Units;
using Utilities.Quantities;

namespace ProjectileMotionWeb.Controllers
{
    public class AjaxUnitConvertController : BaseController
    {
        [HttpPost]
        public double GetConvertedVal(double value, string from, string to, string unittype)
        {
            return (double)GetType().GetMethod("GetConvertedVal" + unittype).Invoke(this, new object[] { value, from, to });
        }

        public double GetConvertedValVelocity(double value, string from, string to)
        {
            UnitVelocity unitFrom = new ReflectionHelper(typeof(UnitVelocity)).GetValueOfStaticProperty(from) as UnitVelocity;
            UnitVelocity unitTo = new ReflectionHelper(typeof(UnitVelocity)).GetValueOfStaticProperty(to) as UnitVelocity;

            return new Velocity(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValLength(double value, string from, string to)
        {
            UnitLength unitFrom = new ReflectionHelper(typeof(UnitLength)).GetValueOfStaticProperty(from) as UnitLength;
            UnitLength unitTo = new ReflectionHelper(typeof(UnitLength)).GetValueOfStaticProperty(to) as UnitLength;

            return new Length(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValMass(double value, string from, string to)
        {
            UnitMass unitFrom = new ReflectionHelper(typeof(UnitMass)).GetValueOfStaticProperty(from) as UnitMass;
            UnitMass unitTo = new ReflectionHelper(typeof(UnitMass)).GetValueOfStaticProperty(to) as UnitMass;

            return new Mass(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValArea(double value, string from, string to)
        {
            UnitArea unitFrom = new ReflectionHelper(typeof(UnitArea)).GetValueOfStaticProperty(from) as UnitArea;
            UnitArea unitTo = new ReflectionHelper(typeof(UnitArea)).GetValueOfStaticProperty(to) as UnitArea;

            return new Area(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValDensity(double value, string from, string to)
        {
            UnitDensity unitFrom = new ReflectionHelper(typeof(UnitDensity)).GetValueOfStaticProperty(from) as UnitDensity;
            UnitDensity unitTo = new ReflectionHelper(typeof(UnitDensity)).GetValueOfStaticProperty(to) as UnitDensity;

            return new Density(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValGravAcceleration(double value, string from, string to)
        {
            UnitGravAcceleration unitFrom = new ReflectionHelper(typeof(UnitGravAcceleration)).GetValueOfStaticProperty(from) as UnitGravAcceleration;
            UnitGravAcceleration unitTo = new ReflectionHelper(typeof(UnitGravAcceleration)).GetValueOfStaticProperty(to) as UnitGravAcceleration;

            return new GravAcceleration(value, unitFrom).GetConvertedVal(unitTo);
        }



        public double GetConvertedValAngle(double value, string from, string to)
        {
            UnitAngle unitFrom = new ReflectionHelper(typeof(UnitAngle)).GetValueOfStaticProperty(from) as UnitAngle;
            UnitAngle unitTo = new ReflectionHelper(typeof(UnitAngle)).GetValueOfStaticProperty(to) as UnitAngle;

            return new Angle(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValTime(double value, string from, string to)
        {
            UnitTime unitFrom = new ReflectionHelper(typeof(UnitTime)).GetValueOfStaticProperty(from) as UnitTime;
            UnitTime unitTo = new ReflectionHelper(typeof(UnitTime)).GetValueOfStaticProperty(to) as UnitTime;

            return new Time(value, unitFrom).GetConvertedVal(unitTo);
        }

    }
}