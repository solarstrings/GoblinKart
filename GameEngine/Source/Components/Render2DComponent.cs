using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class Render2DComponent : IComponent
    {
        public Texture2D Texture { get; set; }
        public SpriteEffects Effect { get; set; }
        public Rectangle DestRect { get; set; }
        public Rectangle SourceRect { get; set; }
        public bool Flipped { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public Render2DComponent(Texture2D texture) 
        {
            Texture = texture;
            Height = texture.Height;
            Width = texture.Width;
            DestRect = new Rectangle(0, 0, Width, Height);
            Effect = SpriteEffects.None;
        }
    }
}
