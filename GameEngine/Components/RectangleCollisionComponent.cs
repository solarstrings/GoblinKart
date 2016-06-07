using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Components
{
    public class RectangleCollisionComponent : IComponent
    {
        public Rectangle CollisionRect { get; set; }

        public RectangleCollisionComponent(Rectangle rect)
        {
            CollisionRect = rect;
        }
    }
}
