using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Interfaces
{
    public interface IRenderSystem : ISystem
    {
        void Render(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
