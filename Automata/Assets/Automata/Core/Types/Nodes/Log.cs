using Automata.Core.Types;
using UnityEngine;

public class Log : Composite
{
    public BlackboardEntry<string> Message;
    public BlackboardEntry<Vector2> Positionn;

    protected override State OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}