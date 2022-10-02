using System;
using System.Collections.Generic;
using Automata.Core.Types.Interfaces;
using UnityEditor;
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
        public List<INode<NodeBlueprint>> Children { get; set; } = new List<INode<NodeBlueprint>>();

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

        public RuntimePort[] GetInputPorts(UnityEditor.Experimental.GraphView.Node node)
        {
            return null;
        }

        public RuntimePort[] GetOutputPorts(UnityEditor.Experimental.GraphView.Node node)
        {
            return null;
        }

        public Port CreatePort(Type type, UnityEditor.Experimental.GraphView.Node node, Orientation orientation,
            Direction direction, Port.Capacity capacity, string portName = "")
        {
            Port runtimePort = node.InstantiatePort(orientation, direction, capacity, type);
            if (runtimePort != null)
            {
                runtimePort.portName = portName;
            }
            return runtimePort;
        }

        public Port CreatePort<T>(UnityEditor.Experimental.GraphView.Node node, Orientation orientation,
            Direction direction, Port.Capacity capacity, string portName = "")
            => CreatePort(typeof(T), node, orientation, direction, capacity, name);

        public static NodeBlueprint CreateFromType(Type type)
        {
            NodeBlueprint nodeBlueprint = CreateInstance<NodeBlueprint>();
            nodeBlueprint.NodeType = type;
            nodeBlueprint.name = $"ANBP_{type}";
            nodeBlueprint.Guid = GUID.Generate().ToString();

            return nodeBlueprint;
        }

#endif
    }
}