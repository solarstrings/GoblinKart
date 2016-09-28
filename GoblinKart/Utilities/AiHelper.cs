using System;
using System.Diagnostics;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Engine.InputDefs;
using GameEngine.Managers;
using GameEngine.Systems;
using Microsoft.Xna.Framework;

namespace GoblinKart.Utilities
{
    public static class AiHelper
    {
        public static Vector3 Accelerate(Vector3 velocity)
        {
            if (velocity.X < PhysicsManager.MaxSpeed - (PhysicsManager.MaxSpeed * 0.15))
                velocity += new Vector3(PhysicsManager.Instance.Acceleration, 0, 0);
            else
                velocity = new Vector3((PhysicsManager.MaxSpeed - (PhysicsManager.MaxSpeed * 0.15f)),  0, 0);
            return velocity;
        }

        public static bool NearlyEqual(float a, float b, float epsilon)
        {
            var absA = Math.Abs(a);
            var absB = Math.Abs(b);
            var diff = Math.Abs(a - b);

            if (a == b) return true;
            if (a == 0 || b == 0 || diff < float.Epsilon) return diff < epsilon;
            return diff/(absA + absB) < epsilon;
        }

        #region Source: http://circlessuck.blogspot.se/2012/07/xna-smooth-sprite-rotation.html

        public static float CurveAngle(float from, float to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            var fromVector = new Vector2((float) Math.Cos(from), (float) Math.Sin(from));
            var toVector = new Vector2((float) Math.Cos(to), (float) Math.Sin(to));
            var currentVector = Slerp(fromVector, toVector, step);

            return (float) Math.Atan2(currentVector.Y, currentVector.X);
        }

        private static Vector2 Slerp(Vector2 from, Vector2 to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            var theta = Math.Acos(Vector2.Dot(from, to));
            if (theta == 0) return to;

            var sinTheta = Math.Sin(theta);
            return (float) (Math.Sin((1 - step)*theta)/sinTheta)*from + (float) (Math.Sin(step*theta)/sinTheta)*to;
        }

        #endregion

        public static void FindNextWaypoint(AiComponent aiC)
        {
            if (aiC.Waypoint.Id + 1 == AiSystem.Waypoints.Count)
            {
                //State change from within state as discussed here:
                //https://sourcemaking.com/design_patterns/state
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
        public static Vector2 V3ToV2(Vector3 v3)
        {
            return new Vector2(v3.X, v3.Z);
        }
    }
}
