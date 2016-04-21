using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public class AIComponent : IComponent
    {
        public IScript Script { get; set; }

        public AIComponent(IScript script)
        {
            Script = script;
        }
    }
}
