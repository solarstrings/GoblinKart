using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public class CircleCollisionComponent : IComponent
    {
        public double Radius { set; get; }

        public CircleCollisionComponent(float radius)
        {
            Radius = radius;
        }
    }
}
