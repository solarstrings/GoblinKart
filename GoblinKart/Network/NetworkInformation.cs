using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GoblinKart.Network
{
    public class NetworkInformation
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Velocity { get; set; }
    }
}
