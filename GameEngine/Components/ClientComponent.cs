using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Interfaces;
using Lidgren.Network;

namespace GameEngine.Components
{
    public class ClientComponent : IComponent
    {
        public NetClient Client { get; set; }
    }
}
