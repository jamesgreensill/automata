using Automata.Core.Types.Attributes;
using UnityEngine;

public class Log : Composite
{
    [Port] public string Message; 
    [Port] public Vector2 Positionnnnn;

    protected override State OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}