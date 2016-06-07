using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Components
{
    public class Position2DComponent : IComponent
    {
        public Vector2 Position { get; set; }
        
        public Position2DComponent(Vector2 pos) 
        {
            Position = pos;
        }
    }
}
