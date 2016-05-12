using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Systems
{
    public class PhysicsSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            // This should be for all components with a position effected by gravity
            var entities = SceneManager.Instance.GetActiveScene().GetAllEntities();

            if (entities != null)
            {
                var transformComponents =
                    ComponentManager.Instance.GetComponentsFromEntities<TransformComponent>(entities);

                foreach (var transformComponent in transformComponents)
                {
                    //transformComponent.velocity += new Vector3(0, PhysicsManager.Instance.gravity*(float) gameTime.ElapsedGameTime.TotalSeconds, 0);

                    // Temporary hack for the friction.
                    if (transformComponent.Velocity.X > 0)
                    {
                        transformComponent.Velocity -= new Vector3(PhysicsManager.Instance.Friction, 0, 0);
                    }
                    else if (transformComponent.Velocity.X < 0)
                    {
                        transformComponent.Velocity += new Vector3(PhysicsManager.Instance.Friction, 0, 0);
                    }

                    var forward = transformComponent.world.Forward;
                    forward *= transformComponent.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    var asdf = transformComponent.world.Down;
                    asdf *= transformComponent.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    transformComponent.position += forward -= asdf;
                }
            }
        }
    }
}