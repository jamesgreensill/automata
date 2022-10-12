using Automata.Core.Types.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Automata.Core.Types
{
    public class BlackboardBlueprint : ScriptableObject
    {
        private readonly IDictionary<string, BlackboardEntryBlueprint> _Data = new Dictionary<string, BlackboardEntryBlueprint>();
        public int Count => _Data.Count;

        public void Add(KeyValuePair<string, BlackboardEntryBlueprint> item) => _Data.Add(item.Key, item.Value);

        public void Clear() => _Data.Clear();

        public bool Contains(KeyValuePair<string, BlackboardEntryBlueprint> item) => _Data.Contains(item);

        public bool Remove(KeyValuePair<string, BlackboardEntryBlueprint> item) => _Data.Remove(item.Key);

        public void Add(string key, BlackboardEntryBlueprint value) => _Data.Add(key, value);

        public bool ContainsKey(string key) => _Data.ContainsKey(key);

        public bool Remove(string key) => _Data.Remove(key);

        public bool TryGetValue(string key, out BlackboardEntryBlueprint value) => _Data.TryGetValue(key, out value);

        public BlackboardEntryBlueprint this[string key]
        {
            get => _Data[key];
            set => _Data[key] = value;
        }

    }
}