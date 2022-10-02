using System;
using System.Collections.Generic;

namespace Automata.Core.Types
{
    public class RuntimePort
    {
        public enum Direction
        {
            Input,
            Output
        }

        public string Name;
        public Type Type;
        public Direction PortDirection;
        public Action<RuntimePort, RuntimeEdge> OnConnect;
        public Action<RuntimePort> OnDisconnect;

        public IEnumerable<RuntimeEdge> Connections => _Connections;
        public bool Connected => _Connections.Count > 0;

        private HashSet<RuntimeEdge> _Connections;

        public T ConnectTo<T>(RuntimePort other) where T : RuntimeEdge, new()
        {
            var edge = new T
            {
                OutputPort = this.PortDirection == Direction.Output ? other : this,
                InputPort = this.PortDirection == Direction.Input ? this : other
            };

            this.Connect(edge);
            other.Connect(edge);

            return edge;
        }

        public void Connect(RuntimeEdge edge)
        {
            if (!_Connections.Contains(edge))
            {
                _Connections.Add(edge);
                OnConnect?.Invoke(this, edge);
            }
        }

        public void Disconnect(RuntimeEdge edge)
        {
            _Connections.Remove(edge);
            OnDisconnect?.Invoke(this);
        }

        public void DisconnectAll()
        {
            _Connections.Clear();
            OnDisconnect?.Invoke(this);
        }
    }
}