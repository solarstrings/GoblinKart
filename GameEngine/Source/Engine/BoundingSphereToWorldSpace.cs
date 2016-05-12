using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    class BoundingSphereToWorldSpace
    {
        public BoundingSphere ConvertBoundingSphereToWorldCoords(BoundingSphere sphere, Matrix world)
        {
            Vector3 pos = Vector3.Transform(Vector3.Zero, Matrix.Invert(world));
            BoundingSphere s = new BoundingSphere(sphere.Center - pos, sphere.Radius);
            return s;
        }
    }
}
