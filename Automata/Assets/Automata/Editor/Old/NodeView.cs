using Automata.Core.Types;
using Automata.Core.Types.Interfaces;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Automata.Editor
{
    public sealed class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;
        public NodeBlueprint Node;
        public Port[] InputPorts;
        public Port OutputPort;

        public NodeView(NodeBlueprint node)
        {
            Node = node;
            title = node.name;
            viewDataKey = node.Guid;
            style.left = node.GraphPosition.x;
            style.top = node.GraphPosition.y;

            _Initialize();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Node.GraphPosition.x = newPos.xMin;
            Node.GraphPosition.y = newPos.yMin;
        }

        public override void OnSelected() => OnNodeSelected?.Invoke(this);

        public void SortChildren(Comparison<INode<NodeBlueprint>> comparer) => Node.Children.Sort(comparer);

        public static int SortByHorizontalPosition(NodeBlueprint left, NodeBlueprint right) => left.GraphPosition.x < right.GraphPosition.x ? -1 : 1;

        public static int SortByVerticalPosition(INode<NodeBlueprint> top, INode<NodeBlueprint> bottom) => top.Base.GraphPosition.y < bottom.Base.GraphPosition.y ? -1 : 1;

        private void _Initialize()
        {
            _CreateInputPorts();
            _CreateOutputPorts();

            /*
             *  ARCHIVED CODE
             *  PURPOSE: BLACKBOARD VISUAL EDITOR
             *  ARCHIVED BY: JAMES GREENSILL
             */
            // _CreateBlackboardPorts();
        }

        private void _CreateOutputPorts()
        {
            Port port = Node.GetOutputPort(this);

            if (port != null)
            {
                OutputPort = port;
                outputContainer.Add(port);
            }
        }

        private void _CreateInputPorts()
        {
            if (AutomataEditor.Instance.DoReload)
            {
                Node.ReloadPorts();
            }
            Port[] ports = Node.GetInputPorts(this);

            foreach (var port in ports)
            {
                if (port == null) continue;
                InputPorts = ports;
                inputContainer.Add(port);
            }
        }

        /*
         *  ARCHIVED CODE
         *  PURPOSE: BLACKBOARD VISUAL EDITOR
         *  ARCHIVED BY: JAMES GREENSILL
         */

        // private void _CreateBlackboardPorts()
        // {
        //     // List<Port> ports = _GetBlackboardPorts();
        //     // foreach (var port in ports)
        //     // {
        //     //     inputContainer.Add(port);
        //     // }
        // }

        // private List<Port> _GetBlackboardPorts()
        // {
        //    // List<Port> ports = new List<Port>();
        //    // var fields = RuntimeNode.GetType().GetFields();
        //    // foreach (var property in fields)
        //    // {
        //    //     var attributes = property.GetCustomAttributes<InputPortValueAttribute>(true);
        //    //     foreach (var attribute in attributes)
        //    //     {
        //    //         ports.Add(RuntimeNode.CreatePort(property.FieldType, this, Orientation.Horizontal, Direction.Input, Port.Capacity.Single, property.Name));
        //    //     }
        //    // }
        //    //
        //    // return ports;
        // }
    }
}