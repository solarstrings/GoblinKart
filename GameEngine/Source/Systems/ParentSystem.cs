using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class ParentSystem : IUpdateSystem
    {
        /// <summary>
        /// Updates a child position relative to its parent
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<ParentComponent>();

            if (entities == null)
            {
                return;
            }

            foreach (Entity e in entities)
            {
                if (e.Updateable)
                {
                    ParentComponent parent = ComponentManager.Instance.GetEntityComponent<ParentComponent>(e);                    
                    Position2DComponent parentPos = ComponentManager.Instance.GetEntityComponent<Position2DComponent>(parent.Parent);
                    Position2DComponent myPos = ComponentManager.Instance.GetEntityComponent<Position2DComponent>(e);

                    if (parentPos != null && myPos != null)
                        myPos.Position = new Vector2(parent.OffsetX, parent.OffsetY) + parentPos.Position;
                }
            }
        }
    }
}
