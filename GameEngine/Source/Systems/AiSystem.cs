using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;

namespace GameEngine.Systems
{
    public class AiSystem : IUpdateSystem
    {
        public static List<Waypoint> Waypoints { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime) 
        {
            var entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<AiComponent>();

            Parallel.ForEach (entities, entity =>
            {
                if (entity.Updateable)
                {
                    var aiC = ComponentManager.Instance.GetEntityComponent<AiComponent>(entity);
                    aiC.GetState().DoAction(entity);
                }
            });
        }

        public static float GetRotation(Vector3 transformPos, Vector2 targetPos)
        {
            var currentVector = new Vector2(transformPos.X, transformPos.Z);

            var direction = currentVector - targetPos;
            direction.Normalize();
            var angle = Vector2ToRadian(direction);
            return angle;
        }

        private static float Vector2ToRadian(Vector2 direction)
        {
            return (float)Math.Atan2(direction.X, direction.Y);
        }
    }
}
