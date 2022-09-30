using System.Collections.Generic;

namespace Automata.Core.Types.Interfaces
{
    public interface INode<T>
    {
        public T Base { get; }
        public List<INode<T>> Children { get; set; }

        public INode<T>[] GetChildren();

        public void AddChild(INode<T> child);

        public bool RemoveChild(INode<T> child);
    }
}