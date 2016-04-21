using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class TriggerSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<TriggerComponent>();

            foreach (Entity entity in entities)
            {
                TriggerComponent t = ComponentManager.Instance.GetEntityComponent<TriggerComponent>(entity);
                t.Script.Update(gameTime);
            }
        }
    }
}
