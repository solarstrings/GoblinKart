using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace GameEngine
{
    public class AISystem : IUpdateSystem
    {
        public static List<Waypoint> Waypoints { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<AIComponent>();

            Parallel.ForEach (entities, entity =>
            {
                AIComponent ai = ComponentManager.Instance.GetEntityComponent<AIComponent>(entity);
                TransformComponent transformC = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity);
                var distance = Vector2.Distance(V3ToV2(transformC.Position), ai.Waypoint.WaypointPosition);
                //Console.WriteLine(distance);
                //distance > ai.LastDistance just in case we pass our "circle" because of our velocity.
                if (distance <= ai.Waypoint.Radius || distance > ai.LastDistance)
                {
                    ai.Waypoint = FindNextWaypoint(ai.Waypoint);
                    ai.Waypoint.SetRandomTargetPosition();
                    ai.LastDistance = float.MaxValue;
                    transformC.Rotation = ai.GetRotation(transformC.Position);
                }
                else
                    ai.LastDistance = distance;
                transformC.Velocity = new Vector3(20f, 0f, 0f);
            });
        }

        private Waypoint FindNextWaypoint(Waypoint wp) {
            if (wp.Id + 1 == Waypoints.Count)
                return Waypoints[0];
            return Waypoints[wp.Id + 1];
        }

        private Vector2 V3ToV2(Vector3 v3) {
            return new Vector2(v3.X, v3.Z);
        }
    }
}
