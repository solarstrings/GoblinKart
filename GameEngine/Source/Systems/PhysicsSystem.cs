using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameEngine {
    public class PhysicsSystem : IUpdateSystem {

        /* Updates position of entities with transformComponents by their respective velocities. */
        public void Update(GameTime gameTime) {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();

            if (entities != null) {
                List<TransformComponent> trsComps = ComponentManager.Instance.GetComponentsFromEntities<TransformComponent>(entities);

                for(int i = 0; i < trsComps.Count; i++) {
                    Vector3 velForward = trsComps[i].world.Forward;
                    Vector3 velDownward = trsComps[i].world.Down;
                    velForward *= trsComps[i].velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    velDownward *= trsComps[i].velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    trsComps[i].position += velForward;
                    trsComps[i].position -= velDownward;
                }
            }
        }

        public static void ApplyGravity(ref TransformComponent trsComp, GameTime gameTime) {
            trsComp.velocity.Y += trsComp.gravity;
        }

        /* Adds friction to the object, the amount depending on if it is in the air or not. */
        public static void ApplyFriction(ref TransformComponent trsComp, bool airborne) {
            if (airborne) {
                trsComp.velocity.X *= trsComp.drag;
                trsComp.velocity.Y *= trsComp.drag;
            }
            else {
                trsComp.velocity.X *= trsComp.friction;
                trsComp.velocity.Y *= trsComp.friction;
            }
        }
    }
}