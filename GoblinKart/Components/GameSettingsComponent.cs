using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Interfaces;

namespace GoblinKart.Components
{
    public class GameSettingsComponent : IComponent
    {
        public int NrOfLaps { get; set; } = 2;
    }
}
