using Automata.Core.Types.Interfaces;
using System;

namespace Automata.Core.Utility.Extensions
{
    public static class INodeEx
    {
        public static void Traverse<T>(this INode<T> parent, Action<INode<T>> visitor) where T : INode<T>
        {
            visitor?.Invoke(parent);
            INode<T>[] children = parent.GetChildren();
            foreach (INode<T> child in children)
            {
                child.Traverse(visitor);
            }
        }
    }
}