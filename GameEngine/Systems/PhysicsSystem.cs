using System;
using System.Collections.Generic;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;
using Microsoft.Xna.Framework;

namespace GameEngine.Systems {
    public class PhysicsSystem : IUpdateSystem {

        /* Updates position of entities with transformComponents by their respective velocities. */
        public void Update(GameTime gameTime) {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();

            if (entities != null)
            {
                List<TransformComponent> trsComps = ComponentManager.Instance.GetComponentsFromEntities<TransformComponent>(entities);

                foreach (var t in trsComps)
                {
                    var velForward = t.World.Forward;
                    var velDownward = t.World.Down;
                    velForward *= t.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    velDownward *= t.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    t.Position += velForward;
                    t.Position -= velDownward;
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