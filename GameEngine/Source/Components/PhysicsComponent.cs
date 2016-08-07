using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Interfaces;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameEngine.Source.Components
{
    public class PhysicsComponent : IComponent
    {
        public float Mass { get; set; }
        public float Friction { get; set; }
        public float Drag { get; set; }
    }
}

