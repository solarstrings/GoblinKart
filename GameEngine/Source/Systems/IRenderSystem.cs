using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public interface IRenderSystem : ISystem
    {
        void Render(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
