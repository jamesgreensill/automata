using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Automata.Core.Types.Interfaces;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

using Automata.Core.Types.Attributes;
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

        public List<PortBlueprint> InputPorts = new List<PortBlueprint>();

        public PortBlueprint OutputPort;

        [SerializeField] public Vector2 GraphPosition;
        [SerializeField] public NodeMetadata Metadata;
        [SerializeField] public bool ShouldDrawGizmos;
        [SerializeField] public string Guid;

        public Type NodeType
        {
            get
            {
                if (_NodeType == null)
                {
                    var type = Type.GetType(TypeReflectionAddress);
                    if (type != null)
                    {
                        return _NodeType = type;
                    }
                    // TODO: Debug Error.
                }
                return _NodeType;
            }
            set
            {
                TypeReflectionAddress = value.FullName;
                _NodeType = value;
            }
        }

        private Type _NodeType;

        internal string TypeReflectionAddress;

        public INode<NodeBlueprint>[] GetChildren() => Children.ToArray();

        public void AddChild(INode<NodeBlueprint> child) => Children.Add(child);

        public bool RemoveChild(INode<NodeBlueprint> child) => Children.Remove(child);

#if UNITY_EDITOR
        public Capabilities Capabilites;

        public virtual bool IsDeletable(Node node) =>
            GetCapabilities(node).HasFlag(Capabilities.Deletable);

        public virtual Capabilities GetCapabilities(Node node) => node.capabilities;

        public Port[] GetInputPorts(Node node)
        {
            return InputPorts
                .Select(inputPort => CreatePort(inputPort, node, Orientation.Horizontal))
                .ToArray();
        }

        public Port GetOutputPort(Node node) => CreatePort(OutputPort, node, Orientation.Horizontal);

        public Port CreatePort(PortBlueprint portBlueprint, Node node, Orientation orientation)
        {
            Direction direction =
                portBlueprint.Direction == PortDirection.Input ? Direction.Input : Direction.Output;
            Port.Capacity capacity = portBlueprint.Capacity == PortCapacity.Single ? Port.Capacity.Single : Port.Capacity.Multi;
            Port port = node.InstantiatePort(orientation, direction, capacity, portBlueprint.Type);
            port.portName = portBlueprint.Name;
            return port;
        }

        public static NodeBlueprint CreateFromType(string typeString)
        {
            var nodeBlueprint = CreateInstance<NodeBlueprint>();

            nodeBlueprint.TypeReflectionAddress = typeString;
            nodeBlueprint.name = $"ANBP_{nodeBlueprint.NodeType.Name}";
            nodeBlueprint.Guid = GUID.Generate().ToString();

            nodeBlueprint.ReloadPorts();

            return nodeBlueprint;
        }

        public void ReloadPorts()
        {
            InputPorts = new List<PortBlueprint>()
            {
                new PortBlueprint(typeof(NodeBlueprint), "Input", PortDirection.Input, PortCapacity.Multi)
            };
            OutputPort =
                new PortBlueprint(typeof(NodeBlueprint), "Output", PortDirection.Output, PortCapacity.Multi);
            foreach (FieldInfo fieldInfo in NodeType.GetFields())
            {
                IEnumerable<PortAttribute> attributes = fieldInfo.GetCustomAttributes<PortAttribute>();
                foreach (PortAttribute attribute in attributes)
                {
                    InputPorts.Add(new PortBlueprint(fieldInfo.FieldType, fieldInfo.Name, PortDirection.Input, PortCapacity.Single));
                }
            }
        }

#endif
    }
}