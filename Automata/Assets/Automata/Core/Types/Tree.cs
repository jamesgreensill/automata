using Automata.Core.Types.Interfaces;
using Automata.Core.Utility.Extensions;
using System;
using System.Collections.Generic;

namespace Automata.Core.Types
{
    public class Tree
    {
        public Node Root;
        public Node.State State;
        public List<Node> Nodes = new List<Node>();

        public TreeBlueprint Blueprint;

        public IActivator Activator;

        public INode<Node>[] GetChildren(Node parent) => parent.GetChildren();

        public void AddChild(Node parentNode, Node childNode) => parentNode.AddChild(childNode);

        public bool RemoveChild(Node parentNode, Node childNode) => parentNode.RemoveChild(childNode);

        public Node CreateNode(Type type)
        {
            if (type.BaseType != typeof(Node))
                return null;

            if (!type.HasDefaultConstructor())
                return null;

            var node = System.Activator.CreateInstance(type) as Node;
            if (node != null)
            {
                Nodes.Add(node);
            }
            return node;
        }

        public Node CreateNode<T>() where T : Node => CreateNode(typeof(T));
    }
}