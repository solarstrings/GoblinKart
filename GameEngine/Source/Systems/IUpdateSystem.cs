using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameEngine
{
    public interface IUpdateSystem : ISystem
    {
        void Update(GameTime gameTime);
    }
}
