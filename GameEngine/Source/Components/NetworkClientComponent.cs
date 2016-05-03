using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace GameEngine.Source.Components
{
    public class NetworkClientComponent : IComponent
    {
        public NetClient Client { get; set; }
    }
}
