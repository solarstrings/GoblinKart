using System;
using System.Diagnostics;
using GameEngine.Components;
using GameEngine.Interfaces;
using GameEngine.Managers;
using GameEngine.Systems;
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
            var distance = Vector2.Distance(V3ToV2(transformC.Position), aiC.Waypoint.WaypointPosition);
            //Debug.WriteLine(distance);
            if (distance <= aiC.Waypoint.Radius)
            {
                Debug.WriteLine("Reached Waypoint " + (aiC.Waypoint.Id + 1));
                FindNextWaypoint(aiC);
                aiC.Waypoint.SetRandomTargetPosition();
            }

            var angle = AiSystem.GetRotation(transformC.Position, aiC.Waypoint.TargetPosition);

            //Run everytime?
            if (!NearlyEqual(transformC.Angle, angle, Epsilon))
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
        }
        public bool NearlyEqual(float a, float b, float epsilon)
        {
            var absA = Math.Abs(a);
            var absB = Math.Abs(b);
            var diff = Math.Abs(a - b);

            if (a == b) return true;
            if (a == 0 || b == 0 || diff < float.Epsilon) return diff < epsilon;
            return diff / (absA + absB) < epsilon;
        }

        #region Source: http://circlessuck.blogspot.se/2012/07/xna-smooth-sprite-rotation.html

        private static float CurveAngle(float from, float to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            var fromVector = new Vector2((float)Math.Cos(from), (float)Math.Sin(from));
            var toVector = new Vector2((float)Math.Cos(to), (float)Math.Sin(to));
            var currentVector = Slerp(fromVector, toVector, step);

            return (float)Math.Atan2(currentVector.Y, currentVector.X);
        }

        private static Vector2 Slerp(Vector2 from, Vector2 to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            var theta = Math.Acos(Vector2.Dot(from, to));
            if (theta == 0) return to;

            var sinTheta = Math.Sin(theta);
            return (float)(Math.Sin((1 - step) * theta) / sinTheta) * from + (float)(Math.Sin(step * theta) / sinTheta) * to;
        }

        #endregion

        private static void FindNextWaypoint(AiComponent aiC)
        {
            if (aiC.Waypoint.Id + 1 == AiSystem.Waypoints.Count)
            {
                aiC.SetState(new IdleState());
                Debug.WriteLine("Changing State to Idle");
            }
            else
            {
                aiC.Waypoint = AiSystem.Waypoints[aiC.Waypoint.Id + 1];
                Debug.WriteLine("Changing Waypoint to " + (aiC.Waypoint.Id + 1) + " / " + AiSystem.Waypoints.Count);
            }
        }

        //Might be unnecessary, but keeping for now.
        private static Vector2 V3ToV2(Vector3 v3)
        {
            return new Vector2(v3.X, v3.Z);
        }
    }
}
