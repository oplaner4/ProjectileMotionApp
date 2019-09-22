using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProjectileMotionWeb.Helpers
{
    public class ReflectionHelper
    {

        public ReflectionHelper(Type classType)
        {
            ClassType = classType;
        }


        public object GetValueOfStaticProperty(string propName)
        {
            FieldInfo fieldInfo = ClassType.GetTypeInfo().DeclaredFields.Where(x => x.Name == propName).FirstOrDefault();
            if (fieldInfo != null)
                return fieldInfo.GetValue(null);
            return null;
        }


        public List<string> GetListStaticPropertiesNames()
        {
            return ClassType.GetTypeInfo().DeclaredFields.Where(x => x.IsStatic).Select(x => x.Name).ToList();
        }

        public Type ClassType { get; private set; }
    }


    public class UnitsReflectionHelper : ReflectionHelper
    {
        public UnitsReflectionHelper(Type classType) : base (classType)
        {

        }

        public List<string> GetListUnitsNames()
        {
            return ClassType.GetTypeInfo().DeclaredFields.Where(x => x.IsStatic && x.Name != "Basic").Select(x => x.Name).ToList();
        }
    }
}