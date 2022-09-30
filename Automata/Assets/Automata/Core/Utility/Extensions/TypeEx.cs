using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Automata.Core.Utility.Extensions
{
    public static class TypeEx
    {
        public static bool HasDefaultConstructor(this Type type) => type.GetConstructors().Select(constructor => constructor.GetParameters()).Any(parameters => parameters.Length <= 0);

        public static IEnumerable<Type> GetDerivedTypes(Type type)
        {
            return GetDerivedTypes(type, Assembly.GetAssembly(type));
        }

        public static IEnumerable<Type> GetDerivedTypes(Type type, Assembly assembly) => assembly.GetTypes().Where(t => t != type && type.IsAssignableFrom(t)).ToList();

        public static Attribute[] GetAttributes(Type type) => Attribute.GetCustomAttributes(type, true);
    }
}