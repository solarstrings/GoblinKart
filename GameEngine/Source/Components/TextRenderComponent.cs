using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class TextRenderComponent : IComponent
    {
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public SpriteFont Font { get; set; }

        public TextRenderComponent(string text, Color color, SpriteFont font)
        {
            Text = text;
            TextColor = color;
            Font = font;
        }
    }
}
