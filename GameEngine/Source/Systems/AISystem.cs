using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class AISystem : IUpdateSystem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<AIComponent>();

            foreach (Entity entity in entities)
            {
                AIComponent ai = ComponentManager.Instance.GetEntityComponent<AIComponent>(entity);
                ai.Script.Update(gameTime);
            }
        }
    }
}
