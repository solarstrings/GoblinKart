using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class AIComponent : IComponent
    {
        public Waypoint Waypoint { get; set; }
        public Quaternion Rotation { get; set; }
        public float Direction { get; set; }
        public float LastDistance { get; set; }

        public AIComponent(Waypoint wp)
        {
            Waypoint = wp;
            Direction = 0f;
            wp.SetRandomTargetPosition();
        }

        public Quaternion GetRotation(Vector3 position)
        {
            var tV = new Vector2(position.X, position.Z);
            var wV = new Vector2(Waypoint.TargetPosition.X, Waypoint.TargetPosition.Y);

            var moveDirection = tV - wV;
            moveDirection.Normalize();
            var angle = Vector2ToRadian(moveDirection);
            Direction = angle;
            return Quaternion.CreateFromAxisAngle(Vector3.UnitY, angle);
        }
        private static float Vector2ToRadian(Vector2 direction)
        {
            return (float)Math.Atan2(direction.X, direction.Y);
        }
    }
}
