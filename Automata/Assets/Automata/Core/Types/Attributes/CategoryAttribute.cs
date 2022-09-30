using System;

namespace Automata.Core.Types.Attributes
{
    // TODO: Nested Categories.
    [AttributeUsage(AttributeTargets.Class)]
    public class CategoryAttribute : Attribute
    {
        public Type Type;
        public string Name;

        public CategoryAttribute(Type type) : this(type, type.Name)
        {
        }

        public CategoryAttribute(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}