namespace Automata.Core.Types.Interfaces
{
    public interface ITree<T> where T : INode<T>
    {
        public void AddChild(INode<T> parentNode, INode<T> childNode);

        public void RemoveChild(INode<T> parentNode, INode<T> childNode);

        public INode<T>[] GetChildren(INode<T> node);
    }
}