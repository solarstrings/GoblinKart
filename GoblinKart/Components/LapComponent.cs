using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Engine;
using GameEngine.Interfaces;

namespace GoblinKart.Components
{
    public class LapComponent : IComponent
    {
        public int Laps { get; set; } = 0;
        public int CurrentWaypoint { get; set; } = 0;
    }
}
