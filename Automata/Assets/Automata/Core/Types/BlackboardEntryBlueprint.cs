using Automata.Core.Types;
using Automata.Core.Types.Interfaces;
using UnityEngine;

public class BlackboardEntryBlueprint : ScriptableObject
{
    public BlackboardEntryBlueprint Base => this;
    [field: SerializeField] public string Id { get; set; }
    public DirtyProperty<BlackboardEntryBlueprint> Value { get; set; }
}