using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameEngine.Source.Components
{
    public class PhysicsComponent : IComponent
    {
        public float Mass { get; set; }
        public Vector3 Force { get; set; }
        public bool InAir { get; set; } = false;
    }
}

