using System.Collections.Generic;
using System.Linq;
using Automata.Core.Types.Interfaces;

namespace Automata.Core.Types
{
    public class RuntimeBlackboard
    {
        private readonly Dictionary<string, BlackboardEntry<object>> _Data = new Dictionary<string, BlackboardEntry<object>>();
        public int Count => _Data.Count;

        public void Add(KeyValuePair<string, BlackboardEntry<object>> item) => _Data.Add(item.Key, item.Value);

        public void Clear() => _Data.Clear();

        public bool Contains(KeyValuePair<string, BlackboardEntry<object>> item) => _Data.Contains(item);

        public bool Remove(KeyValuePair<string, BlackboardEntry<object>> item) => _Data.Remove(item.Key);

        public void Add(string key, BlackboardEntry<object> value) => _Data.Add(key, value);

        public bool ContainsKey(string key) => _Data.ContainsKey(key);

        public bool Remove(string key) => _Data.Remove(key);

        public bool TryGetValue(string key, out BlackboardEntry<object> value) => _Data.TryGetValue(key, out value);

        public BlackboardEntry<object> this[string key]
        {
            get => _Data[key];
            set => _Data[key] = value;
        }
    }
}