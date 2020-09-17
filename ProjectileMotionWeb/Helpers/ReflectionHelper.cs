using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utilities.Units;

namespace ProjectileMotionWeb.Helpers
{
    public class ReflectionHelper
    {
        public ReflectionHelper(Type classType)
        {
            ClassType = classType;
        }

        protected Type ClassType { get; set; }
    }

    public class ReflectionHelper<T> : ReflectionHelper
    {
        public ReflectionHelper() : base (typeof(T))
        {}
    }

    public class UnitsReflectionHelper : ReflectionHelper
    {
        public UnitsReflectionHelper(Type classType) : base (classType)
        {}

        public List<string> GetListUnitsNames()
        {
            return ClassType.GetFields(BindingFlags.Public | BindingFlags.Static).Where(f => f.Name != "Basic").Select(f => f.Name).ToList();
        }
    }

    public class UnitsReflectionHelper<T> : UnitsReflectionHelper
    {
        public UnitsReflectionHelper() : base(typeof(T))
        { }

        public T GetUnit(string unitName)
        {
            return (T)ClassType.GetField(unitName, BindingFlags.Static | BindingFlags.Public).GetValue(null);
        }
    }
}