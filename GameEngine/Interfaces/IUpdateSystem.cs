using Microsoft.Xna.Framework;

namespace GameEngine.Interfaces
{
    public interface IUpdateSystem : ISystem
    {
        void Update(GameTime gameTime);
    }
}
