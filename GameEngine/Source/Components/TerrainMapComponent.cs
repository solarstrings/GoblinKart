using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class TerrainMapComponent : IComponent
    {
        public List<TerrainChunk> terrainChunks {get;set;}
        public Texture2D terrainHeightMap { get; set; }

        private Texture2D terrainMap;

        public float[,] heightInfo;

        private int numChunks;

        private int clipX, clipY;
        private int clipW, clipH;
        private int hmWidth, hmHeight;

        public VertexPositionNormalTexture[] vertices { get; set; }
        public int[] indices { get; set; }

        public TerrainMapComponent(GraphicsDevice graphicsDevice,Texture2D HeightmapTexture,Texture2D defaultTexture,int numChunks)
        {
            terrainChunks = new List<TerrainChunk>();
            terrainHeightMap = HeightmapTexture;
            hmWidth = terrainHeightMap.Width;
            hmHeight = terrainHeightMap.Height;
            LoadHighmap(HeightmapTexture);
            vertices = InitTerrainVertices();
            InitIndices();
            InitNormals();

            this.numChunks = numChunks;

            //Get clip width and clip height.
            clipW = hmWidth / numChunks;
            clipH = hmHeight / numChunks;

            //setup the terrain chunks
            SetupTerrainChunks(graphicsDevice, defaultTexture);
            CorrectChunkPositions();
        }

        private void LoadHighmap(Texture2D terrainMap)
        {
            this.terrainMap = terrainMap;

            int width = terrainMap.Width;
            int height = terrainMap.Height;

            //get the pixels from the terrain map
            Color[] colors = new Color[width * height];
            terrainMap.GetData(colors);

            heightInfo = new float[width, height];
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    heightInfo[x, y] = colors[x + y * width].R / 5f;
                }
            }
        }

        private void InitIndices()
        {
            indices = new int[(hmWidth - 1) * (hmHeight - 1) * 6];
            int indicesCount = 0; ;

            for (int y = 0; y < hmHeight - 1; ++y)
            {
                for (int x = 0; x < hmWidth - 1; ++x)
                {
                    int botLeft = x + y * hmWidth;
                    int botRight = (x + 1) + y * hmWidth;
                    int topLeft = x + (y + 1) * hmWidth;
                    int topRight = (x + 1) + (y + 1) * hmWidth;

                    indices[indicesCount++] = topLeft;
                    indices[indicesCount++] = botRight;
                    indices[indicesCount++] = botLeft;

                    indices[indicesCount++] = topLeft;
                    indices[indicesCount++] = topRight;
                    indices[indicesCount++] = botRight;
                }
            }
        }
        private void InitNormals()
        {
            int indicesLen = indices.Length / 3;
            for (int i = 0; i < vertices.Length; ++i)
            {
                vertices[i].Normal = new Vector3(0f, 0f, 0f);
            }

            for (int i = 0; i < indicesLen; ++i)
            {
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

        private VertexPositionNormalTexture[] InitTerrainVertices()
        {
            VertexPositionNormalTexture[] terrainVerts = new VertexPositionNormalTexture[hmWidth * hmHeight];

            for (int x = 0; x < hmWidth; x++)
            {
                for (int y = 0; y < hmHeight; y++)
                {
                    terrainVerts[x + y * hmHeight].Position = new Vector3(x, heightInfo[x, y], -y);
                    terrainVerts[x + y * hmHeight].TextureCoordinate.X = (float)x / 30.0f;
                    terrainVerts[x + y * hmHeight].TextureCoordinate.Y = (float)y / 30.0f;
                }
            }

            return terrainVerts;
        }

        public VertexPositionNormalTexture[] GetVertexTextureNormals(Rectangle rect)
        {
            VertexPositionNormalTexture[] terrainVerts = new VertexPositionNormalTexture[rect.Width * rect.Height];

            for (int x = rect.X; x < rect.X + rect.Width; x++)
            {
                for (int y = rect.Y; y < rect.Y + rect.Height; y++)
                {
                    terrainVerts[(x - rect.X) + (y - rect.Y) * rect.Height].Normal = vertices[x + y * hmHeight].Normal;
                }
            }
            return terrainVerts;
        }


        private void SetupTerrainChunks(GraphicsDevice graphicsDevice, Texture2D defaultTexture)
        {
            //loop through all the chunks to cut out from the heightmap
            for(clipX = 0; clipX <hmWidth-1;clipX+=clipW)
            {
                for (clipY = 0; clipY < hmHeight-1; clipY += clipH)
                {
                    //use this line to see the chunks (don't use in real game)
                    TerrainChunk t = new TerrainChunk(graphicsDevice, terrainHeightMap, new Rectangle(clipX, clipY, clipW, clipH ),
                                     new Vector3(clipX, 0, -clipY), GetVertexTextureNormals(new Rectangle(clipX, clipY, clipW , clipH )));
                    Rectangle clipRect = new Rectangle(clipX, clipY, clipW + 1, clipH + 1);
                    //TerrainChunk t = new TerrainChunk(graphicsDevice, terrainHeightMap, clipRect, 
                    //                 new Vector3(clipX,0,-clipY),GetVertexTextureNormals(clipRect));

                    //apply the default texture to the chunk, so that it is visible
                    t.SetTexture(defaultTexture);

                    //add the chunk to the chunklist
                    terrainChunks.Add(t);
                }
            }
        }

        private void CorrectChunkPositions()
        {
            terrainChunks[1].offsetPosition += new Vector3(0, 0, 0);
        }

        public void SetTextureToChunk(int chunk,Texture2D texture)
        {
            terrainChunks[chunk].SetTexture(texture);
        }

        public float GetTerrainHeight(float x, float z)
        {
            if (x < 0
                || z < 0
                || x > heightInfo.GetLength(0) - 1
                || z > heightInfo.GetLength(1) - 1)
            {
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

            if (posAboveLowTriangle)
            {
                resultHeight = heightLowXLowZ;
                resultHeight += zRel * (heightLowXHighZ - heightLowXLowZ);
                resultHeight += xRel * (heightHighXLowZ - heightLowXLowZ);
            }
            else
            {
                resultHeight = heightHighXHighZ;
                resultHeight += (1.0f - zRel) * (heightHighXLowZ - heightHighXHighZ);
                resultHeight += (1.0f - xRel) * (heightLowXHighZ - heightHighXHighZ);
            }
            return resultHeight;
        }
    }
}
