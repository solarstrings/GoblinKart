using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace GameEngine {
    /// <summary>
    /// TerrainChunk
    /// A class which holds the information for a chunk of the terrain.
    /// 
    /// NOTE: THIS IS NOT A COMPONENT
    /// </summary>
    public class TerrainChunk{
        public Vector3 offsetPosition { get; set; }

        public BoundingBox boundingBox { get; set; }
        public VertexBuffer vBuffer { get; set; }

        public IndexBuffer iBuffer { get; set; }
        public BasicEffect effect { get; set; }

        public int indicesLenDiv3;

        public int width { get; set; }
        public int height { get; set; }

        public float[,] heightInfo;
        public Texture2D terrainTex { get; set; }

        public VertexPositionNormalTexture[] vertices { get; set; }
        public int[] indices { get; set; }

        private Rectangle terrainRect;

        public TerrainChunk(GraphicsDevice graphicsDevice, Texture2D terrainMap, Rectangle terrainRect, Vector3 offsetPosition, VertexPositionNormalTexture[] vertexNormals) {
            effect = new BasicEffect(graphicsDevice);
            this.terrainRect = terrainRect;

            //set the offset position. Used for drawing the chunk at the right position
            this.offsetPosition = offsetPosition;

            CreateHightmap(terrainMap);

            vertices = InitTerrainVertices();

            //create the bounding box from the vertices
            boundingBox = CreateBoundingBox(vertices);

            effect.FogEnabled = true;
            effect.FogStart = 10f;
            effect.FogColor = Color.LightGray.ToVector3();
            effect.FogEnd = 400f;

            //initialize the indices
            InitIndices();
            //copy the calculated normal values
            CopyNormals(vertexNormals);

            PrepareBuffers(graphicsDevice);
        }

        private void CopyNormals(VertexPositionNormalTexture[] vertexNormals) {
            for (int i = 0; i < vertices.Length; ++i) {
                vertices[i].Normal = vertexNormals[i].Normal;
            }
        }

        private void CreateHightmap(Texture2D terrainMap) {
            width = terrainMap.Width;
            height = terrainMap.Height;

            //get the pixels from the terrain map
            Color[] colors = new Color[width * height];
            terrainMap.GetData(colors);

            //copy the desired portion of the map
            heightInfo = new float[terrainRect.Width, terrainRect.Height];
            for (int x = terrainRect.X; x < terrainRect.X + terrainRect.Width; ++x) {
                for (int y = terrainRect.Y; y < terrainRect.Y + terrainRect.Height; ++y) {
                    heightInfo[x - terrainRect.X, y - terrainRect.Y] = colors[x + y * width].R / 5f;
                }
            }
            width = terrainRect.Width;
            height = terrainRect.Height;
        }

        private void InitIndices() {
            indices = new int[(width - 1) * (height - 1) * 6];
            int indicesCount = 0; ;

            for (int y = 0; y < height - 1; ++y) {
                for (int x = 0; x < width - 1; ++x) {
                    int botLeft = x + y * width;
                    int botRight = (x + 1) + y * width;
                    int topLeft = x + (y + 1) * width;
                    int topRight = (x + 1) + (y + 1) * width;

                    indices[indicesCount++] = topLeft;
                    indices[indicesCount++] = botRight;
                    indices[indicesCount++] = botLeft;

                    indices[indicesCount++] = topLeft;
                    indices[indicesCount++] = topRight;
                    indices[indicesCount++] = botRight;
                }
            }

            indicesLenDiv3 = indices.Length / 3;
        }

        private void InitNormals() {
            int indicesLen = indices.Length / 3;
            for (int i = 0; i < vertices.Length; ++i) {
                vertices[i].Normal = new Vector3(0f, 0f, 0f);
            }

            for (int i = 0; i < indicesLen; ++i) {
                //get indices indexes
                int i1 = indices[i * 3];
                int i2 = indices[i * 3 + 1];
                int i3 = indices[i * 3 + 2];

                //get the two faces
                Vector3 face1 = vertices[i1].Position - vertices[i3].Position;
                Vector3 face2 = vertices[i1].Position - vertices[i2].Position;

                //get the cross product between them
                Vector3 normal = Vector3.Cross(face1, face2);

                //update the normal
                vertices[i1].Normal += normal;
                vertices[i2].Normal += normal;
                vertices[i3].Normal += normal;
            }
        }

        private VertexPositionNormalTexture[] InitTerrainVertices() {
            VertexPositionNormalTexture[] terrainVerts = new VertexPositionNormalTexture[width * height];

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    terrainVerts[x + y * height].Position = new Vector3(x, heightInfo[x, y], -y);
                    terrainVerts[x + y * height].TextureCoordinate.X = (float)x / (width - 1.0f);
                    terrainVerts[x + y * height].TextureCoordinate.Y = (float)y / (height - 1.0f);
                }
            }
            return terrainVerts;
        }

        private void PrepareBuffers(GraphicsDevice graphicsDevice) {
            iBuffer = new IndexBuffer(graphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            iBuffer.SetData(indices);

            vBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.WriteOnly);
            vBuffer.SetData(vertices);
        }

        private BoundingBox CreateBoundingBox(VertexPositionNormalTexture[] vertexArray)
        {
            List<Vector3> points = new List<Vector3>();
            foreach (VertexPositionNormalTexture v in vertexArray)
            {
                points.Add(v.Position);
            }
            BoundingBox b = BoundingBox.CreateFromPoints(points);
            return b;
        }

        public void SetTexture(Texture2D texture) {
            this.terrainTex = texture;
        }



    }
}
