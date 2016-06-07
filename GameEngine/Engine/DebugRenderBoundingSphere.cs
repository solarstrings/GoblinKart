using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine
{
    public class DebugRenderBoundingSphere
    {
        /*
        GraphicsDevice device;

        public DebugRenderBoundingSphere(GraphicsDevice device)
        {
            this.device = device;
            CreateShape();
            basicEffect = new BasicEffect(device);
        }

        public static float RADIANS_FOR_90DEGREES = MathHelper.ToRadians(90);//(float)(Math.PI / 2.0);
        public static float RADIANS_FOR_180DEGREES = RADIANS_FOR_90DEGREES * 2;

        protected VertexBuffer buffer;
        protected VertexDeclaration vertexDecl;

        private BasicEffect basicEffect;

        private const int CIRCLE_NUM_POINTS = 32;
        private IndexBuffer _indexBuffer;
        private VertexPositionNormalTexture[] _vertices;

        public void CreateShape()
        {
            double angle = MathHelper.TwoPi / CIRCLE_NUM_POINTS;

            _vertices = new VertexPositionNormalTexture[CIRCLE_NUM_POINTS + 1];

            _vertices[0] = new VertexPositionNormalTexture(
                Vector3.Zero, Vector3.Forward, Vector2.One);

            for (int i = 1; i <= CIRCLE_NUM_POINTS; i++)
            {
                float x = (float)Math.Round(Math.Sin(angle * i), 4);
                float y = (float)Math.Round(Math.Cos(angle * i), 4);
                Vector3 point = new Vector3(
                                 x,
                                 y,
                                  0.0f);
                
                _vertices[i] = new VertexPositionNormalTexture(
                    point,
                    Vector3.Forward,
                    new Vector2());
            }

            // Initialize the vertex buffer, allocating memory for each vertex
            buffer = new VertexBuffer(device,
                typeof(VertexPositionNormalTexture),_vertices.Length,
                BufferUsage.None);


            // Set the vertex buffer data to the array of vertices
            buffer.SetData<VertexPositionNormalTexture>(_vertices);

            InitializeLineStrip();
        }

        private void InitializeLineStrip()
        {
            // Initialize an array of indices of type short
            short[] lineStripIndices = new short[CIRCLE_NUM_POINTS + 1];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < CIRCLE_NUM_POINTS; i++)
            {
                lineStripIndices[i] = (short)(i + 1);
            }

            lineStripIndices[CIRCLE_NUM_POINTS] = 1;

            // Initialize the index buffer, allocating memory for each index
            _indexBuffer = new IndexBuffer(device,typeof(short),lineStripIndices.Length,BufferUsage.None);

            // Set the data in the index buffer to our array
            _indexBuffer.SetData<short>(lineStripIndices);

        }

    public void Draw(BoundingSphere bs, Color color, Matrix world, Matrix view, Matrix projection)
    {

        if (bs != null)
        {
            Matrix scaleMatrix = Matrix.CreateScale(bs.Radius);
            Matrix translateMat = Matrix.CreateTranslation(bs.Center);
            Matrix rotateYMatrix = Matrix.CreateRotationY(RADIANS_FOR_90DEGREES);
            Matrix rotateXMatrix = Matrix.CreateRotationX(RADIANS_FOR_90DEGREES);


            //device.RenderState.DepthBufferEnable = true;
            //device.RenderState.DepthBufferWriteEnable = true;
            //device.RenderState.AlphaBlendEnable = true;
            //device.RenderState.SourceBlend = Blend.SourceAlpha;
            //device.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            //device.RenderState.CullMode = CullMode.CullCounterClockwiseFace;

            device.BlendState = BlendState.AlphaBlend;
            device.DepthStencilState = DepthStencilState.DepthRead;
            device.RasterizerState = RasterizerState.CullCounterClockwise;

            // effect is a compiled effect created and compiled elsewhere
            // in the application
            basicEffect.EnableDefaultLighting();
            basicEffect.View = view;
            basicEffect.Projection = projection;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                    device.Indices = _indexBuffer;

                    basicEffect.Alpha = ((float)color.A / (float)byte.MaxValue);

                    basicEffect.World = scaleMatrix * translateMat;
                    basicEffect.DiffuseColor = color.ToVector3();

                    

                device.DrawIndexedPrimitives(
                        PrimitiveType.LineStrip,
                        0,  // vertex buffer offset to add to each element of the index buffer
                        0,  // minimum vertex index
                        CIRCLE_NUM_POINTS + 1, // number of vertices. If this gets an exception for you try changing it to 0.  Seems to work just as well.
                        0,  // first index element to read
                        CIRCLE_NUM_POINTS); // number of primitives to draw

                    basicEffect.World = rotateYMatrix * scaleMatrix * translateMat;
                    basicEffect.DiffuseColor = color.ToVector3() * 0.5f;

                    device.DrawIndexedPrimitives(
                        PrimitiveType.LineStrip,
                        0,  // vertex buffer offset to add to each element of the index buffer
                        0,  // minimum vertex index
                        CIRCLE_NUM_POINTS + 1, // number of vertices. If this gets an exception for you try changing it to 0.  Seems to work just as well.
                        0,  // first index element to read
                        CIRCLE_NUM_POINTS); // number of primitives to draw

                    basicEffect.World = rotateXMatrix * scaleMatrix * translateMat;
                    basicEffect.DiffuseColor = color.ToVector3() * 0.5f;

                    device.DrawIndexedPrimitives(
                        PrimitiveType.LineStrip,
                        0,  // vertex buffer offset to add to each element of the index buffer
                        0,  // minimum vertex index
                        CIRCLE_NUM_POINTS + 1, // number of vertices. If this gets an exception for you try changing it to 0.  Seems to work just as well.
                        0,  // first index element to read
                        CIRCLE_NUM_POINTS); // number of primitives to draw
                }
            }
        }*/
    }
}
