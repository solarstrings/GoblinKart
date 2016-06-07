using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Components
{
    public class Velocity2DComponent : IComponent
    {
        public Vector2 Velocity { get; set; }
        public float Speed { get; set; }
        public int direction { get; set; }

        public Velocity2DComponent(Vector2 vel, float speed)
        {
            Velocity = vel;
            Speed = speed;
        }
    }
}
