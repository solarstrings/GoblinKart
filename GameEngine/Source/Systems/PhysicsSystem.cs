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

        public void Update(GameTime gameTime)
        {

            // Loop all entities with a PhysicsComponent
            var physicsEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<PhysicsComponent>();

            foreach (var physicsEntity in physicsEntities)
            {
                var transformComponent = ComponentManager.Instance.GetEntityComponent<TransformComponent>(physicsEntity);
                var pyhsicsComponent = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(physicsEntity);

                ApplyGravity(physicsEntity, pyhsicsComponent, transformComponent);
                ApplyFriction(physicsEntity, pyhsicsComponent, transformComponent);
                ApplyDrag(physicsEntity, transformComponent);

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

        public void ApplyDrag(Entity physicsEntity, TransformComponent transformComponent)
        {
            var dragComponent = ComponentManager.Instance.GetEntityComponent<DragComponent>(physicsEntity);

            if (dragComponent != null)
            {
                transformComponent.Velocity.X *= PhysicsManager.Instance.Drag;
                transformComponent.Velocity.Z *= PhysicsManager.Instance.Drag;
            }
        }


        private void ApplyGravity(Entity physicsEntity, PhysicsComponent physicsComponent, TransformComponent transformComponent)
        {
            var gravityComponent = ComponentManager.Instance.GetEntityComponent<GravityComponent>(physicsEntity);

            if (gravityComponent != null)
            {
                transformComponent.Velocity.Y -= PhysicsManager.Instance.Gravity;           
            }      
        }

        private void ApplyFriction(Entity physicsEntity, PhysicsComponent physicsComponent, TransformComponent transformComponent)
        {           
            var frictionComponent = ComponentManager.Instance.GetEntityComponent<FrictionComponent>(physicsEntity);           
           
            if (frictionComponent != null)
            {
                if (!physicsComponent.InAir)
                {
                    transformComponent.Velocity.X *= PhysicsManager.Instance.Friction;
                    transformComponent.Velocity.Z *= PhysicsManager.Instance.Friction;
                }               
            }
        }        
    }
}