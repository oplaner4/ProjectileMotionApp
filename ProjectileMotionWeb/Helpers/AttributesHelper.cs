using ProjectileMotionWeb.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;
using Utilities.Quantities;
using Utilities.Units;

namespace ProjectileMotionWeb.Helpers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private object ExpectedValue { get; set; }


        public RequiredIfAttribute(string propertyName, object expectedValue)
        {
            PropertyName = propertyName;
            ExpectedValue = expectedValue;
        }


        public override object TypeId
        {
            get
            {
                return this;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value != null)
            {
                return ValidationResult.Success;
            }

            object instance = context.ObjectInstance;
            if (instance.GetType().GetProperty(PropertyName).GetValue(instance, null).Equals(ExpectedValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }


    public class MaxRightElevationAngleAttributeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if ((double)value <= 0)
            {
                return ValidationResult.Success;
            }

            object instance = context.ObjectInstance;
            string unit = (string)instance.GetType().GetProperty(nameof(SetPropertiesQuantitiesModel.ElevationAngleUnit)).GetValue(instance, null);

            try
            {
                new ElevationAngle((double)value, new UnitsReflectionHelper(typeof(UnitAngle)).GetValueOfStaticProperty(unit) as UnitAngle);
            }
            catch (InvalidElevationAngleException ex)
            {
                return new ValidationResult("* " + ex.Message);
            }

            return ValidationResult.Success;
        }
    }
}