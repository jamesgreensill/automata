using System;
using System.Collections.Generic;
using Automata.Core.Types.Interfaces;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor.Experimental.GraphView;

#endif

namespace Automata.Core.Types
{
    public class NodeBlueprint : ScriptableObject, INode<NodeBlueprint>
    {
        public struct NodeMetadata
        {
            [TextArea] public string Description;
        }

        public NodeBlueprint Base => this;
        public List<INode<NodeBlueprint>> Children { get; set; }

        public Vector2 GraphPosition;
        public NodeMetadata Metadata;
        public bool ShouldDrawGizmos;
        public string Guid;
        public Type NodeType;

        public INode<NodeBlueprint>[] GetChildren() => Children.ToArray();

        public void AddChild(INode<NodeBlueprint> child) => Children.Add(child);

        public bool RemoveChild(INode<NodeBlueprint> child) => Children.Remove(child);

#if UNITY_EDITOR
        public Capabilities Capabilites;

        public virtual bool IsDeletable(UnityEditor.Experimental.GraphView.Node node) => GetCapabilities(node).HasFlag(Capabilities.Deletable);

        public virtual Capabilities GetCapabilities(UnityEditor.Experimental.GraphView.Node node) => node.capabilities;

        public Port[] GetInputPorts(UnityEditor.Experimental.GraphView.Node node)
        {
            return null;
        }

        public Port[] GetOutputPorts(UnityEditor.Experimental.GraphView.Node node)
        {
            return null;
        }

        public Port CreatePort(Type type, UnityEditor.Experimental.GraphView.Node node, Orientation orientation,
            Direction direction, Port.Capacity capacity, string portName = "")
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, type);
            if (port != null)
            {
                port.portName = portName;
            }
            return port;
        }

        public Port CreatePort<T>(UnityEditor.Experimental.GraphView.Node node, Orientation orientation,
            Direction direction, Port.Capacity capacity, string portName = "")
            => CreatePort(typeof(T), node, orientation, direction, capacity, name);

#endif
    }
}