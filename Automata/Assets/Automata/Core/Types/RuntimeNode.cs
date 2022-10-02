using Automata.Core.Types.Interfaces;
using Automata.Core.Utility;
using System.Collections.Generic;

namespace Automata.Core.Types
{
    [System.Serializable]
    public abstract class RuntimeNode : INode<RuntimeNode>
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

        public RuntimeNode Base => this;

        public List<INode<RuntimeNode>> Children { get; set; }

        public virtual INode<RuntimeNode>[] GetChildren() => Children.ToArray();

        public virtual void AddChild(INode<RuntimeNode> child) => Children.Add(child);

        public virtual bool RemoveChild(INode<RuntimeNode> child) => Children.Remove(child);

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