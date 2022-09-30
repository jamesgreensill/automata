using Automata.Core.Types.Interfaces;
using Automata.Core.Utility;
using System.Collections.Generic;

namespace Automata.Core.Types
{
    [System.Serializable]
    public abstract class Node : INode<Node>
    {
        public enum State
        {
            Running, Failure, Success
        }

        public enum Type
        {
            Root,
            Branch,
            Leaf
        }

        public Node Base => this;

        public List<INode<Node>> Children { get; set; }

        public virtual INode<Node>[] GetChildren() => Children.ToArray();

        public virtual void AddChild(INode<Node> child) => Children.Add(child);

        public virtual bool RemoveChild(INode<Node> child) => Children.Remove(child);

        protected abstract State OnUpdate();

        protected virtual void OnDrawGizmosSelected()
        {
            /*void*/
        }

        protected virtual void OnDrawGizmos()
        {
            /*void*/
        }

        protected virtual void OnStart()
        {
            /*void*/
        }

        protected virtual void OnStop()
        {
            /*void*/
        }

        protected bool OnVerifyValues() => this.Validate();
    }
}