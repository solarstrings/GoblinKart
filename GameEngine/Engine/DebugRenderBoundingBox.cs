using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Engine
{
    class DebugRenderBoundingBox
    {
        BasicEffect effect;
        GraphicsDevice device;
        short[] cubeIndices = { 0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4, 0, 4, 1, 5, 2, 6, 3, 7 };

        public DebugRenderBoundingBox(GraphicsDevice device)
        {
            effect = new BasicEffect(device);
            this.device = device;
        }

        public void RenderBoundingBox(BoundingBox box, Matrix world, Matrix view, Matrix projection)
        {
            Vector3 v1 = box.Min;
            Vector3 v2 = box.Max;
            VertexPositionColor[] cubeVerts = new VertexPositionColor[8];
            cubeVerts[0] = new VertexPositionColor(v1, Color.White);
            cubeVerts[1] = new VertexPositionColor(new Vector3(v2.X,v1.Y,v1.Z), Color.White);
            cubeVerts[2] = new VertexPositionColor(new Vector3(v2.X, v1.Y, v2.Z), Color.Green);
            cubeVerts[3] = new VertexPositionColor(new Vector3(v1.X, v1.Y, v2.Z), Color.Blue);

            cubeVerts[4] = new VertexPositionColor(new Vector3(v1.X, v2.Y, v1.Z), Color.White);
            cubeVerts[5] = new VertexPositionColor(new Vector3(v2.X, v2.Y, v1.Z), Color.Red);
            cubeVerts[6] = new VertexPositionColor(v2, Color.Green);
            cubeVerts[7] = new VertexPositionColor(new Vector3(v1.X, v2.Y, v2.Z), Color.Blue);
            
            effect.World = world;
            effect.View = view;
            effect.Projection = projection;
            effect.VertexColorEnabled = true;

            foreach (EffectPass p in effect.CurrentTechnique.Passes)
            {
                p.Apply();
                device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.LineList,cubeVerts,0,8,cubeIndices,0,12);
            }
        }

    }
}
