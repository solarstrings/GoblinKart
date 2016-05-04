using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    class BoundingBoxToWorldSpace
    {
        public BoundingBox ConvertBoundingBoxToWorldCoords(BoundingBox box, Matrix world)
        {
            Vector3 pos = Vector3.Transform(Vector3.Zero, Matrix.Invert(world));
            BoundingBox b = new BoundingBox(box.Min - pos, box.Max - pos);
            return b;
        }
    }
}
