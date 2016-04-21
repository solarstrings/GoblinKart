using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class InputSystem : IUpdateSystem
    {
        /// <summary>
        /// Runs the script attacked to the input component.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<InputComponent>();

            foreach (Entity entity in entities)
            {
                InputComponent input = ComponentManager.Instance.GetEntityComponent<InputComponent>(entity);
                input.Script.Update(gameTime);
            }
        }

    }
}
