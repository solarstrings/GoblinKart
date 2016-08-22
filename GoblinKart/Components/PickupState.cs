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
    public class PickupState : IState
    {
        private const float Epsilon = 0.1f;
        
        public void DoAction(Entity entity)
        {
            var aiC = ComponentManager.Instance.GetEntityComponent<AiComponent>(entity);
            var transformC = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity);
            var powerup = ComponentManager.Instance.GetFirstEntityOfType<PowerupModelComponent>();
            var powerupTransform = ComponentManager.Instance.GetEntityComponent<TransformComponent>(powerup);

            var distanceToPowerup = Vector2.Distance(AiHelper.V3ToV2(transformC.Position), AiHelper.V3ToV2(powerupTransform.Position));
            if (distanceToPowerup > 60)
            {
                //State change from within state as discussed here:
                //https://sourcemaking.com/design_patterns/state
                Debug.WriteLine("Changing State to Race");
                aiC.SetState(new RaceState());
                return;
            }

            var angle = AiSystem.GetRotation(transformC.Position, AiHelper.V3ToV2(powerupTransform.Position));

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
