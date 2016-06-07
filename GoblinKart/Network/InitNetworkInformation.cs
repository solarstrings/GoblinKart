using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Components;
using Microsoft.Xna.Framework;

namespace GoblinKart.Network
{
    public class InitNetworkInformation
    {
        public List<PlayerComponent> Players { get; set; } = new List<PlayerComponent>();
    }
}
