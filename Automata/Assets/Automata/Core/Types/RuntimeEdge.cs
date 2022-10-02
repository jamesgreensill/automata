using UnityEngine;

namespace Automata.Core.Types
{
    public class RuntimeEdge
    {
        public RuntimePort InputPort;
        public RuntimePort OutputPort;

        public RuntimeEdge(RuntimePort input, RuntimePort output)
        {
            InputPort = input;
            OutputPort = output;
        }
    }
}