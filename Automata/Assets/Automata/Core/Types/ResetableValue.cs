using System;
using UnityEngine;

namespace Automata.Core.Types
{
    [Serializable]
    public class ResetableValue<T>
    {
        [HideInInspector] public T DefaultValue;
        [SerializeField] public T Value;

        public ResetableValue(T value) => DefaultValue = Value = value;

        public void Reset() => Value = DefaultValue;

        public static implicit operator T(ResetableValue<T> value) => value.Value;
    }
}