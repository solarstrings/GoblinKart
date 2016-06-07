using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Interfaces
{
    public interface IRender3DSystem : ISystem
    {
        void Render(GraphicsDevice graphicsDevice , GameTime gameTime);
    }
}
