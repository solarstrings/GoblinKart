using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine
{
    public class Waypoint
    {
        private static readonly Random Rand = new Random();
        public int Id { get; set; }
        public Vector2 WaypointPosition { get; set; }
        public int Radius { get; set; }
        public Vector2 TargetPosition { get; set; }

        public void SetRandomTargetPosition()
        {
            var x = WaypointPosition.X + Rand.Next(-Radius-1, Radius-1);
            var y = WaypointPosition.Y + Rand.Next(-Radius-1, Radius-1);
            TargetPosition = new Vector2(x, y);
        }
    }
}
