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
                    Vector3 velForward = trsComps[i].World.Forward;
                    Vector3 velDownward = trsComps[i].World.Down;
                    velForward *= trsComps[i].Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    velDownward *= trsComps[i].Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    trsComps[i].Position += velForward;
                    trsComps[i].Position -= velDownward;
                }
            }
        }

        public static void ApplyGravity(ref TransformComponent trsComp, GameTime gameTime,bool airborne) {
            if (airborne)
            {
                trsComp.Velocity.Y += trsComp.Gravity;
            }
        }

        /* Adds friction to the object, the amount depending on if it is in the air or not. */
        public static void ApplyFriction(ref TransformComponent trsComp, bool airborne) {
            if (airborne) {
                trsComp.Velocity.X *= trsComp.Drag;
                trsComp.Velocity.Z *= trsComp.Drag;
            }
            else {
                trsComp.Velocity.X *= trsComp.Friction;
                trsComp.Velocity.Z *= trsComp.Friction;
            }
        }
    }
}