using System;
using Automata.Core.Types.Interfaces;

namespace Automata.Core.Types
{
    public class BlackboardEntry<T>
    {
        public string Id { get; set; }
        public DirtyProperty<T> Value { get; set; }

        public Action<T, T> OnValueChanged;
        public Action<T> OnValueSet;

        public BlackboardEntry(T value, string id)
        {
            Value = new DirtyProperty<T>(value);
            Id = id;
            Value.OnValueChanged += OnValueOnValueChanged;
            Value.OnValueSet += OnValueOnValueSet;
        }

        ~BlackboardEntry()
        {
            Value.OnValueChanged -= OnValueChanged;
            Value.OnValueSet -= OnValueSet;
        }

        public bool IsNull() => Value.Value == null;

        public T Read() => Value.Value;

        public void Write(T data) => Value.Value = data;

        private void OnValueOnValueSet(T o1) => OnValueSet?.Invoke(o1);

        private void OnValueOnValueChanged(T o1, T o2) => OnValueChanged?.Invoke(o1, o2);
    }
}