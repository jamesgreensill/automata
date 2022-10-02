using Automata.Core.Types.Interfaces;
using Automata.Core.Utility.Extensions;
using System;
using System.Collections.Generic;

namespace Automata.Core.Types
{
    public class RuntimeTree
    {
        public RuntimeNode Root;
        public RuntimeNode.State State;
        public List<RuntimeNode> Nodes = new List<RuntimeNode>();

        public TreeBlueprint Blueprint;

        public IActivator Activator;

        public INode<RuntimeNode>[] GetChildren(RuntimeNode parent) => parent.GetChildren();

        public void AddChild(RuntimeNode parentRuntimeNode, RuntimeNode childRuntimeNode) => parentRuntimeNode.AddChild(childRuntimeNode);

        public bool RemoveChild(RuntimeNode parentRuntimeNode, RuntimeNode childRuntimeNode) => parentRuntimeNode.RemoveChild(childRuntimeNode);

        public RuntimeNode CreateNode(Type type)
        {
            if (type.BaseType != typeof(RuntimeNode))
                return null;

            if (!type.HasDefaultConstructor())
                return null;

            var node = System.Activator.CreateInstance(type) as RuntimeNode;
            if (node != null)
            {
                Nodes.Add(node);
            }
            return node;
        }

        public RuntimeNode CreateNode<T>() where T : RuntimeNode => CreateNode(typeof(T));
    }
}