using System;
using System.Collections.Generic;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;

namespace GameEngine.Systems {
    public class PhysicsSystem : IUpdateSystem
    {

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

                UpdatePosition(physicsEntity, gameTime);


            }
        }


        private void UpdatePosition(Entity physicsEntity, GameTime gameTime)
        {
            var transformComponent = ComponentManager.Instance.GetEntityComponent<TransformComponent>(physicsEntity);

            var velForward = transformComponent.World.Forward;
            var velUpwards = transformComponent.World.Up;
            velForward *= transformComponent.Velocity.X*(float) gameTime.ElapsedGameTime.TotalSeconds;
            velUpwards *= transformComponent.Velocity.Y*(float) gameTime.ElapsedGameTime.TotalSeconds;

            transformComponent.Position += velForward;
            transformComponent.Position += velUpwards;
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


            if (gravityComponent == null) return;

            // TODO only if in air!!
            var transformComponent = ComponentManager.Instance.GetEntityComponent<TransformComponent>(physicsEntity);
            transformComponent.Velocity.Y -= PhysicsManager.Instance.Gravity;
        }

        /* Adds friction to the object, the amount depending on if it is in the air or not. */

        private void ApplyFriction(Entity physicsEntity, PhysicsComponent physicsComponent)
        {
            var frictionComponent = ComponentManager.Instance.GetEntityComponent<FrictionComponent>(physicsEntity);
            var dragComponent = ComponentManager.Instance.GetEntityComponent<DragComponent>(physicsEntity);
            var transformComponent = ComponentManager.Instance.GetEntityComponent<TransformComponent>(physicsEntity);

            if (frictionComponent != null)
            {
                transformComponent.Velocity.X *= PhysicsManager.Instance.Friction;
                transformComponent.Velocity.Z *= PhysicsManager.Instance.Friction;
            }

            if (dragComponent != null)
            {
                // TODO if in-air...
                transformComponent.Velocity.X *= PhysicsManager.Instance.Drag;
                transformComponent.Velocity.Z *= PhysicsManager.Instance.Drag;
            }
        }        
    }
}