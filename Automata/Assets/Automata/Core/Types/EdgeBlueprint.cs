using UnityEngine;

namespace Automata.Core.Types
{
    public class EdgeBlueprint
    {
        public PortBlueprint InputPortBlueprint;
        public PortBlueprint OutputPortBlueprint;
        
        public EdgeBlueprint(PortBlueprint input, PortBlueprint output)
        {
            InputPortBlueprint = input;
            OutputPortBlueprint = output;
        }
    }
}