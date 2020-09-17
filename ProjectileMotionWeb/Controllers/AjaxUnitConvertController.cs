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
            UnitsReflectionHelper<UnitVelocity> helper = new UnitsReflectionHelper<UnitVelocity>();
            UnitVelocity unitFrom = helper.GetUnit(from);
            UnitVelocity unitTo = helper.GetUnit(to);

            return new Velocity(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValLength(double value, string from, string to)
        {
            UnitsReflectionHelper<UnitLength> helper = new UnitsReflectionHelper<UnitLength>();
            UnitLength unitFrom = helper.GetUnit(from);
            UnitLength unitTo = helper.GetUnit(to);

            return new Length(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValMass(double value, string from, string to)
        {
            UnitsReflectionHelper<UnitMass> helper = new UnitsReflectionHelper<UnitMass>();
            UnitMass unitFrom = helper.GetUnit(from);
            UnitMass unitTo = helper.GetUnit(to);

            return new Mass(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValArea(double value, string from, string to)
        {
            UnitsReflectionHelper<UnitArea> helper = new UnitsReflectionHelper<UnitArea>();
            UnitArea unitFrom = helper.GetUnit(from);
            UnitArea unitTo = helper.GetUnit(to);

            return new Area(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValDensity(double value, string from, string to)
        {
            UnitsReflectionHelper<UnitDensity> helper = new UnitsReflectionHelper<UnitDensity>();
            UnitDensity unitFrom = helper.GetUnit(from);
            UnitDensity unitTo = helper.GetUnit(to);

            return new Density(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValGravAcceleration(double value, string from, string to)
        {
            UnitsReflectionHelper<UnitGravAcceleration> helper = new UnitsReflectionHelper<UnitGravAcceleration>();
            UnitGravAcceleration unitFrom = helper.GetUnit(from);
            UnitGravAcceleration unitTo = helper.GetUnit(to);

            return new GravAcceleration(value, unitFrom).GetConvertedVal(unitTo);
        }



        public double GetConvertedValAngle(double value, string from, string to)
        {
            UnitsReflectionHelper<UnitAngle> helper = new UnitsReflectionHelper<UnitAngle>();
            UnitAngle unitFrom = helper.GetUnit(from);
            UnitAngle unitTo = helper.GetUnit(to);

            return new Angle(value, unitFrom).GetConvertedVal(unitTo);
        }


        public double GetConvertedValTime(double value, string from, string to)
        {
            UnitsReflectionHelper<UnitTime> helper = new UnitsReflectionHelper<UnitTime>();
            UnitTime unitFrom = helper.GetUnit(from);
            UnitTime unitTo = helper.GetUnit(to);

            return new Time(value, unitFrom).GetConvertedVal(unitTo);
        }

    }
}