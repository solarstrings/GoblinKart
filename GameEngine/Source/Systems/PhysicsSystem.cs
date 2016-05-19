using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Systems {
    public class PhysicsSystem : IUpdateSystem {

        /* Updates position of entities with transformComponents by their respective velocities. */
        public void Update(GameTime gameTime) {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();

            if (entities != null) {
                List<TransformComponent> trsComps = ComponentManager.Instance.GetComponentsFromEntities<TransformComponent>(entities);

                for(int i = 0; i < trsComps.Count; i++) {
                    Vector3 velForward = trsComps[i].world.Forward;
                    Vector3 velDownward = trsComps[i].world.Down;
                    velForward *= trsComps[i].Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    velDownward *= trsComps[i].Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    trsComps[i].position += velForward;
                    trsComps[i].position -= velDownward;
                    //trsComps[i].position += new Vector3(0, trsComps[i].Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                }
            }
        }
    }
}