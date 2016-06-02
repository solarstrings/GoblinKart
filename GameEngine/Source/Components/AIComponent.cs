using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class AIComponent : IComponent
    {
        public Waypoint Waypoint { get; set; }
        public float LastDistance { get; set; }
        public AIComponent(Waypoint wp)
        {
            Waypoint = wp;
            wp.SetRandomTargetPosition();
        }
    }
}
