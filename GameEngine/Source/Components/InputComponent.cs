using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public class InputComponent : IComponent
    {
        public IScript Script { get; set; }

        public InputComponent(IScript script)
        {
            Script = script;
        }
    }
}
