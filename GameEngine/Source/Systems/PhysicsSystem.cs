using System;
using System.Collections.Generic;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;

namespace GameEngine.Systems {
    public class PhysicsSystem : IUpdateSystem {

        /* Updates position of entities with transformComponents by their respective velocities. */
        public void Update(GameTime gameTime)
        {

            // Loop all entities with a PhysicsComponent
            var physicsEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<PhysicsComponent>();

            foreach (var physicsEntity in physicsEntities)
            {
                var pyhsicsComponent = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(physicsEntity);

                ApplyGravity(physicsEntity, pyhsicsComponent);
                ApplyFriction(physicsEntity, pyhsicsComponent);
                ApplyDrag(physicsEntity, pyhsicsComponent);

                UpdatePosition();

                
            }

            // Update position depending on velocity/acceleration etc.
            
            // vel += acc * delTime
            // pos += vel * delTime
            




            //foreach (var t in trsComps)
            //{
            //    var velForward = t.World.Forward;
            //    var velDownward = t.World.Down;
            //    velForward *= t.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    velDownward *= t.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //    t.Position += velForward;
            //    t.Position -= velDownward;
            //}
        }

        private void UpdatePosition(Entity physicsEntity, GameTime gameTime)
        {
            var transformComponent = ComponentManager.Instance.GetEntityComponent<TransformComponent>(physicsEntity);

            //transformComponent.Velocity += transformComponent.Acceleration*(float)gameTime.ElapsedGameTime.TotalSeconds;

            var velForward = transformComponent.World.Forward;
            var velUpwards = transformComponent.World.Up;
            velForward *= transformComponent.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            velUpwards *= transformComponent.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void ApplyDrag(Entity physicsEntity, PhysicsComponent physicsComponent)
        {
            var dragComponent = ComponentManager.Instance.GetEntityComponent<DragComponent>(physicsEntity);

            if (dragComponent != null)
            {
                // Apply drag
                
            }
        }


        private void ApplyGravity(Entity physicsEntity, PhysicsComponent physicsComponent)
        {
            var gravityComponent = ComponentManager.Instance.GetEntityComponent<GravityComponent>(physicsEntity);

            if (gravityComponent != null)
            {
                // Apply gravity!
            }

            //if (airborne)
            //{
            //    trsComp.Velocity.Y += trsComp.Gravity;
            //}
        }

        /* Adds friction to the object, the amount depending on if it is in the air or not. */
        private void ApplyFriction(Entity physicsEntity, PhysicsComponent physicsComponent)
        {
            var frictionComponent = ComponentManager.Instance.GetEntityComponent<FrictionComponent>(physicsEntity);
            var dragComponent = ComponentManager.Instance.GetEntityComponent<DragComponent>(physicsEntity);

            if (frictionComponent != null)
            {

                if (dragComponent != null)
                {
                    // Apply drag
                    //velocity += acceleration - friction * velocity
                }
                else
                {
                    // Apply friction
                }

            }

            //if (airborne) {
            //    trsComp.Velocity.X *= trsComp.Drag;
            //    trsComp.Velocity.Z *= trsComp.Drag;
            //}
            //else {
            //    trsComp.Velocity.X *= trsComp.Friction;
            //    trsComp.Velocity.Z *= trsComp.Friction;
            //}
        }
    }
}