using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Engine;
using GameEngine.Interfaces;

namespace GoblinKart.Components
{
    public class GameSettingsComponent : IComponent
    {
        public int NrOfLaps { get; set; } = 1;
        public List<Waypoint> Waypoints = new List<Waypoint>();

    }
}
