using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace GameEngine.Source.Components
{
    public class NetworkServerComponent : IComponent
    {
        public NetServer Server { get; set; }
    }
}
