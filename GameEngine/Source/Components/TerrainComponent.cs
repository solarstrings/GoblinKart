using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameEngine {
    public class TerrainComponent : IComponent {
        public VertexBuffer vBuffer { get; set; }

        public IndexBuffer iBuffer { get; set; }
        public BasicEffect effect { get; set; }

        public int indicesLenDiv3;

        public int width { get; set; }
        public int height { get; set; }

        public float[,] heightInfo;

        Texture2D terrainMap { get; set; }
        public Texture2D terrainTex { get; set; }

        public VertexPositionNormalTexture[] vertices { get; set; }
        public int[] indices { get; set; }

        public TerrainComponent() {
        }

        public TerrainComponent(GraphicsDevice graphicsDevice, Texture2D terrainMap, Texture2D terrainTex) {
            effect = new BasicEffect(graphicsDevice);
            this.terrainTex = terrainTex;
            LoadHighmap(terrainMap);

            vertices = InitTerrainVertices();

            effect.FogEnabled = true;
            effect.FogStart = 10f;
            effect.FogColor = Color.LightGray.ToVector3();
            effect.FogEnd = 400f;
            InitIndices();
            InitNormals();
            PrepareBuffers(graphicsDevice);
            effect.VertexColorEnabled = false;
        }

        private void LoadHighmap(Texture2D terrainMap) {
            this.terrainMap = terrainMap;

            width = terrainMap.Width;
            height = terrainMap.Height;

            //get the pixels from the terrain map
            Color[] colors = new Color[width * height];
            terrainMap.GetData(colors);

            heightInfo = new float[width, height];
            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    heightInfo[x, y] = colors[x + y * width].R / 5f;
                }
            }
        }

        protected void InitIndices() {
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
                    terrainVerts[x + y * height].TextureCoordinate.X = (float)x / 30.0f;
                    terrainVerts[x + y * height].TextureCoordinate.Y = (float)y / 30.0f;
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

        public void SetTerrainTexture(Texture2D texture) {
            this.terrainTex = texture;
        }

        public float GetTerrainHeight(float x, float z) {
            if (x < 0
                || z < 0
                || x > heightInfo.GetLength(0) - 1
                || z > heightInfo.GetLength(1) - 1) {
                return 10f;
            }
            //find the two x vertices
            int xLow = (int)x;
            int xHigh = xLow + 1;
            //get the relative x value between the two points
            float xRel = (x - xLow) / ((float)xHigh - (float)xLow);

            //find the two z verticies
            int zLow = (int)z;
            int zHigh = zLow + 1;

            //get the relative z value between the two points
            float zRel = (z - zLow) / ((float)zHigh - (float)zLow);

            //get the minY and MaxY values from the four vertices
            float heightLowXLowZ = heightInfo[xLow, zLow];
            float heightLowXHighZ = heightInfo[xLow, zHigh];
            float heightHighXLowZ = heightInfo[xHigh, zLow];
            float heightHighXHighZ = heightInfo[xHigh, zHigh];

            //test if the position is above the low triangle
            bool posAboveLowTriangle = (xRel + zRel < 1);

            float resultHeight;

            if (posAboveLowTriangle) {
                resultHeight = heightLowXLowZ;
                resultHeight += zRel * (heightLowXHighZ - heightLowXLowZ);
                resultHeight += xRel * (heightHighXLowZ - heightLowXLowZ);
            }
            else {
                resultHeight = heightHighXHighZ;
                resultHeight += (1.0f - zRel) * (heightHighXLowZ - heightHighXHighZ);
                resultHeight += (1.0f - xRel) * (heightLowXHighZ - heightHighXHighZ);
            }
            return resultHeight;
        }
    }
}
