using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Interfaces;

namespace GoblinKart.Components
{
    public class LapComponent : IComponent
    {
        public int Lap { get; set; } = 0;
    }
}
