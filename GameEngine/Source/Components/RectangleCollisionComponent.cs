using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
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
