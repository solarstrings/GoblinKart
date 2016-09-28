using System;
using System.Diagnostics;
using GameEngine.Components;
using GameEngine.Interfaces;
using GameEngine.Managers;
using GameEngine.Systems;
using GoblinKart.Components;
using GoblinKart.Utilities;
using Microsoft.Xna.Framework;

namespace GameEngine.Engine
{
    public class RaceState : IState
    {
        private const float Epsilon = 0.1f;

        public void DoAction(Entity entity)
        {
            var aiC = ComponentManager.Instance.GetEntityComponent<AiComponent>(entity);
            var transformC = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity);
            var player = ComponentManager.Instance.GetFirstEntityOfType<PlayerComponent>();
            var playerTransform = ComponentManager.Instance.GetEntityComponent<TransformComponent>(player);

            var distance = Vector2.Distance(AiHelper.V3ToV2(transformC.Position), aiC.Waypoint.WaypointPosition);
            //Debug.WriteLine("distance:" + distance + " tranform.pos:" + transformC.Position + " waypointpos:" + aiC.Waypoint.WaypointPosition);
            if (distance <= aiC.Waypoint.Radius)
            {
                Debug.WriteLine("Reached waypoint: " + (aiC.Waypoint.Id + 1));
                AiHelper.FindNextWaypoint(aiC);
                aiC.Waypoint.SetRandomTargetPosition();
            }
            //TODO: Lös varför AI:n snurrar runt waypoints ibland.
            var distanceToPlayer = Vector2.Distance(AiHelper.V3ToV2(transformC.Position), AiHelper.V3ToV2(playerTransform.Position));
            if (distanceToPlayer < 40 && distance > aiC.Waypoint.Radius*2)
            {
                //State change from within state as discussed here:
                //https://sourcemaking.com/design_patterns/state
                Debug.WriteLine("Changing State to Ram");
                aiC.SetState(new RamState());
                return;
            }
            var powerup = ComponentManager.Instance.GetFirstEntityOfType<PowerupModelComponent>();
            var powerupTransform = ComponentManager.Instance.GetEntityComponent<TransformComponent>(powerup);
            var distanceToPowerup = Vector2.Distance(AiHelper.V3ToV2(transformC.Position), AiHelper.V3ToV2(powerupTransform.Position));
            if (distanceToPowerup < 40)
            {
                //State change from within state as discussed here:
                //https://sourcemaking.com/design_patterns/state
                Debug.WriteLine("Changing State to Pickup");
                aiC.SetState(new PickupState());
                return;
            }

            var angle = AiSystem.GetRotation(transformC.Position, aiC.Waypoint.TargetPosition);

            //Run everytime?
            if (!AiHelper.NearlyEqual(transformC.Angle, angle, Epsilon))
            {
                //MathHelper.Lerp seems to cause some strange behaviour.
                //var curvedAngle = MathHelper.Lerp(transformC.Angle, angle, 0.05f);
                var curvedAngle = AiHelper.CurveAngle(transformC.Angle, angle, 0.05f);
                transformC.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, curvedAngle);
                transformC.Angle = curvedAngle;
            }
            transformC.Velocity = AiHelper.Accelerate(transformC.Velocity);
        }
    }
}
