using System;
using System.Collections.Generic;
using GameEngine.Components;
using GameEngine.Engine;

namespace GameEngine.Managers {
    /// <summary>
    /// Thread safe singleton without using locks
    /// See link: "http://csharpindepth.com/Articles/General/Singleton.aspx#nested-cctor"
    /// </summary>
    public class PhysicsManager {
        public const float MaxSpeed = 50f;
        public const float MaxReverseSpeed = -30f;
        public const float Acceleration = 2f;
        public const float TurningAcceleration = 2.8f;
        public const float JumpingAcceleration = 75f;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static PhysicsManager() { }
        private PhysicsManager() { }
        

        public static PhysicsManager Instance { get; } = new PhysicsManager();


        public bool PhysicsOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="collideEntities"></param>
        /// <returns></returns>
        public bool RectangleCollision(Entity entity, List<Entity> collideEntities) {
            if (PhysicsOn) {
                RectangleCollisionComponent eRect = ComponentManager.Instance.GetEntityComponent<RectangleCollisionComponent>(entity);

                foreach (Entity e in collideEntities) {
                    RectangleCollisionComponent eTarget = ComponentManager.Instance.GetEntityComponent<RectangleCollisionComponent>(e);
                    if (eTarget != null) {
                        if (eRect.CollisionRect.Intersects(eTarget.CollisionRect) && eRect != eTarget) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CircleCollision(Entity entity, List<Entity> collideEntities) {
            if (PhysicsOn) {
                CircleCollisionComponent eCircle = ComponentManager.Instance.GetEntityComponent<CircleCollisionComponent>(entity);
                Position2DComponent ePos = ComponentManager.Instance.GetEntityComponent<Position2DComponent>(entity);

                foreach (Entity e in collideEntities) {
                    CircleCollisionComponent eTarget = ComponentManager.Instance.GetEntityComponent<CircleCollisionComponent>(e);
                    Position2DComponent eTargetPos = ComponentManager.Instance.GetEntityComponent<Position2DComponent>(e);

                    if (eTarget != null && eTargetPos != null) {
                        double distSquare = Math.Pow(ePos.Position.X - eTargetPos.Position.X, 2) + Math.Pow(ePos.Position.Y - eTargetPos.Position.Y, 2);
                        double totalCollisionRadius = Math.Pow(eCircle.Radius + eTarget.Radius, 2);
                        if (distSquare - totalCollisionRadius < 0 && eCircle != eTarget) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}
