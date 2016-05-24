using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Waypoint
    {
        private static Random rand = new Random();
        public int Id { get; set; }
        public Vector2 WaypointPosition { get; set; }
        public int Radius { get; set; }
        public Vector2 TargetPosition { get; set; }

        internal void SetRandomTargetPosition()
        {
            var x = WaypointPosition.X + rand.Next(-Radius, Radius);
            var y = WaypointPosition.Y + rand.Next(-Radius, Radius);
            TargetPosition = new Vector2(x, y);
        }
    }
}
