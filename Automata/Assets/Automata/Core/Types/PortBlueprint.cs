using System;
using System.Collections.Generic;

namespace Automata.Core.Types
{
    public enum PortDirection
    {
        Input,
        Output
    }

    public enum PortCapacity
    {
        Single,
        Multi
    }

    [Serializable]
    public class PortBlueprint
    {
        public string Name;
        public Type Type;
        public Type DisplayType;
        public PortDirection Direction;
        public PortCapacity Capacity;
        public Action<PortBlueprint, EdgeBlueprint> OnConnect;
        public Action<PortBlueprint> OnDisconnect;

        public IEnumerable<EdgeBlueprint> Connections => _Connections;
        public bool Connected => _Connections.Count > 0;

        private HashSet<EdgeBlueprint> _Connections;

        public PortBlueprint(Type type, string name, PortDirection direction, PortCapacity capacity) : this(type, type,
            name, direction, capacity)
        {
          
        }

        public PortBlueprint(Type type, Type displayType, string name, PortDirection direction, PortCapacity capacity)
        {
            Type = type;
            DisplayType = displayType;
            Direction = direction;
            Capacity = capacity;
            Name = $"{name} : {displayType.Name}";
            _Connections = new HashSet<EdgeBlueprint>();
        }

        public T ConnectTo<T>(PortBlueprint other) where T : EdgeBlueprint, new()
        {
            var edge = new T
            {
                OutputPortBlueprint = this.Direction == PortDirection.Output ? other : this,
                InputPortBlueprint = this.Direction == PortDirection.Input ? this : other
            };

            this.Connect(edge);
            other.Connect(edge);

            return edge;
        }

        public void Connect(EdgeBlueprint edgeBlueprint)
        {
            if (!_Connections.Contains(edgeBlueprint))
            {
                _Connections.Add(edgeBlueprint);
                OnConnect?.Invoke(this, edgeBlueprint);
            }
        }

        public void Disconnect(EdgeBlueprint edgeBlueprint)
        {
            _Connections.Remove(edgeBlueprint);
            OnDisconnect?.Invoke(this);
        }

        public void DisconnectAll()
        {
            _Connections.Clear();
            OnDisconnect?.Invoke(this);
        }
    }
}