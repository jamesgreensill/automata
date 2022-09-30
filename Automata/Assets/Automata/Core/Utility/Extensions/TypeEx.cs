using System;
using System.Collections.Generic;
using System.Linq;

namespace Automata.Core.Utility.Extensions
{
    public static class TypeEx
    {
        public static bool HasDefaultConstructor(this Type type) => type.GetConstructors().Select(constructor => constructor.GetParameters()).Any(parameters => parameters.Length <= 0);

        public static IEnumerable<Type> GetDerivedTypes(Type type) =>
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type.IsSubclassOf);

        public static Attribute[] GetAttributes(Type type) => Attribute.GetCustomAttributes(type, true);
    }
}