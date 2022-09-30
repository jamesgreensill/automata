using System;

namespace Automata.Core.Types.Attributes
{
    // TODO: Nested Categories.
    [AttributeUsage(AttributeTargets.Class)]
    public class CategoryAttribute : Attribute
    {
        public string Name { get; set; }

        public CategoryAttribute(string name)
        {
            Name = name;
        }
    }
}