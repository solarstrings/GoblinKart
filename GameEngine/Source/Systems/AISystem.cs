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
                AIComponent aiC = ComponentManager.Instance.GetEntityComponent<AIComponent>(entity);
                TransformComponent transformC = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity);
                var distance = Vector2.Distance(V3ToV2(transformC.Position), aiC.Waypoint.WaypointPosition);
                //Console.WriteLine(distance);
                //distance > ai.LastDistance just in case we pass our "circle" because of our velocity.
                if (distance <= aiC.Waypoint.Radius || distance > aiC.LastDistance)
                {
                    aiC.Waypoint = FindNextWaypoint(aiC.Waypoint);
                    aiC.Waypoint.SetRandomTargetPosition();
                    aiC.LastDistance = float.MaxValue;
                }
                else
                    aiC.LastDistance = distance;

                var angle = GetRotation(transformC.Position, aiC.Waypoint.TargetPosition);
                
                if (transformC.Angle != angle)
                {
                    //MathHelper.Lerp seems to cause some strange behaviour.
                    //var curvedAngle = MathHelper.Lerp(transformC.Angle, angle, 0.05f);
                    var curvedAngle = CurveAngle(transformC.Angle, angle, 0.05f);
                    transformC.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, curvedAngle);
                    transformC.Angle = curvedAngle;
                }
                if (transformC.Velocity.X < PhysicsManager.MaxSpeed)
                    transformC.Velocity += new Vector3(PhysicsManager.Acceleration, 0, 0);
                else
                    transformC.Velocity = new Vector3(PhysicsManager.MaxSpeed, 0, 0);
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

        #region Source: http://circlessuck.blogspot.se/2012/07/xna-smooth-sprite-rotation.html

        private float CurveAngle(float from, float to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            var fromVector = new Vector2((float)Math.Cos(from), (float)Math.Sin(from));
            var toVector = new Vector2((float)Math.Cos(to), (float)Math.Sin(to));
            var currentVector = Slerp(fromVector, toVector, step);

            return (float)Math.Atan2(currentVector.Y, currentVector.X);
        }

        private Vector2 Slerp(Vector2 from, Vector2 to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            double theta = Math.Acos(Vector2.Dot(from, to));
            if (theta == 0) return to;

            double sinTheta = Math.Sin(theta);
            return (float)(Math.Sin((1 - step) * theta) / sinTheta) * from + (float)(Math.Sin(step * theta) / sinTheta) * to;
        }

        #endregion

        private Waypoint FindNextWaypoint(Waypoint wp) {
            if (wp.Id + 1 == Waypoints.Count)
                return Waypoints[0];
            return Waypoints[wp.Id + 1];
        }

        //Might be unnecessary, but keeping for now.
        private Vector2 V3ToV2(Vector3 v3) {
            return new Vector2(v3.X, v3.Z);
        }
    }
}
