using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine {
    public class TerrainMapRenderSystem : IRender3DSystem{
        DebugRenderBoundingBox boxRenderer;
        BoundingBoxToWorldSpace boxConvert;
        bool renderBoxInitialised = false;

        public void Render(GraphicsDevice graphicsDevice, GameTime gameTime) {
            if (renderBoxInitialised.Equals(false)) {
                boxConvert = new BoundingBoxToWorldSpace();
                boxRenderer = new DebugRenderBoundingBox(graphicsDevice);
                renderBoxInitialised = true;
            }

            Entity e = ComponentManager.Instance.GetFirstEntityOfType<TerrainMapComponent>();
            Entity c = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();

            CameraComponent camera = ComponentManager.Instance.GetEntityComponent<CameraComponent>(c);
            TerrainMapComponent terrainComponent = ComponentManager.Instance.GetEntityComponent<TerrainMapComponent>(e);
            TransformComponent transformComponent = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);

            if (terrainComponent != null) {
                if (transformComponent != null) {
                    /*RasterizerState r = new RasterizerState();
                    r.CullMode = CullMode.None;
                    //r.FillMode = FillMode.WireFrame;
                    graphicsDevice.RasterizerState = r;//*/

                    terrainComponent.numChunksInView = 0;

                    for (int i = 0; i < terrainComponent.terrainChunks.Count; ++i) {
                        terrainComponent.terrainChunks[i].effect.TextureEnabled = true;
                        terrainComponent.terrainChunks[i].effect.VertexColorEnabled = false;
                        terrainComponent.terrainChunks[i].effect.Texture = terrainComponent.terrainChunks[i].terrainTex;
                        terrainComponent.terrainChunks[i].effect.Projection = camera.projectionMatrix;
                        terrainComponent.terrainChunks[i].effect.View = camera.viewMatrix;
                        terrainComponent.terrainChunks[i].effect.World = transformComponent.world * Matrix.CreateTranslation(terrainComponent.terrainChunks[i].offsetPosition);
                        terrainComponent.terrainChunks[i].effect.EnableDefaultLighting();

                        BoundingBox box = boxConvert.ConvertBoundingBoxToWorldCoords(terrainComponent.terrainChunks[i].boundingBox, terrainComponent.terrainChunks[i].effect.World);

                        if (box.Intersects(camera.cameraFrustrum)) {
                            foreach (EffectPass p in terrainComponent.terrainChunks[i].effect.CurrentTechnique.Passes) {
                                p.Apply();
                                graphicsDevice.Indices = terrainComponent.terrainChunks[i].iBuffer;
                                graphicsDevice.SetVertexBuffer(terrainComponent.terrainChunks[i].vBuffer);
                                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, terrainComponent.terrainChunks[0].indicesLenDiv3);
                            }
                            boxRenderer.RenderBoundingBox(terrainComponent.terrainChunks[i].boundingBox, terrainComponent.terrainChunks[i].effect.World, camera.viewMatrix, camera.projectionMatrix);
                            terrainComponent.numChunksInView++;
                        }
                    }
                }
            }
        }

        public static void LoadHeightMap(ref TerrainMapComponent terrain, Texture2D terrainMap, Texture2D defaultTex, GraphicsDevice graphicsDevice) {
            terrain.terrainMap = terrainMap;

            int width = terrainMap.Width;
            int height = terrainMap.Height;

            //get the pixels from the terrain map
            Color[] colors = new Color[width * height];
            terrainMap.GetData(colors);

            terrain.heightInfo = new float[width, height];
            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    terrain.heightInfo[x, y] = colors[x + y * width].R / 5f;
                }
            }

            terrain.vertices = InitTerrainVertices(terrain.heightInfo, width, height);
            terrain.indices = InitIndices(width, height);
            InitNormals(ref terrain);

            //setup the terrain chunks
            SetupTerrainChunks(ref terrain, graphicsDevice, defaultTex);
            CorrectChunkPositions(ref terrain);
        }

        internal static int[] InitIndices(int width, int height) {
            int[] indices = new int[(width - 1) * (height - 1) * 6];
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
            return indices;
        }

        private static void InitNormals(ref TerrainMapComponent terrain) {
            int indicesLen = terrain.indices.Length / 3;

            for (int i = 0; i < terrain.vertices.Length; ++i) {
                terrain.vertices[i].Normal = new Vector3(0f, 0f, 0f);
            }

            for (int i = 0; i < indicesLen; ++i) {
                //get indices indexes
                int i1 = terrain.indices[i * 3];
                int i2 = terrain.indices[i * 3 + 1];
                int i3 = terrain.indices[i * 3 + 2];

                //get the two faces
                Vector3 face1 = terrain.vertices[i1].Position - terrain.vertices[i3].Position;
                Vector3 face2 = terrain.vertices[i1].Position - terrain.vertices[i2].Position;

                //get the cross product between them
                Vector3 normal = Vector3.Cross(face1, face2);

                //update the normal
                terrain.vertices[i1].Normal += normal;
                terrain.vertices[i2].Normal += normal;
                terrain.vertices[i3].Normal += normal;
            }
        }

        internal static VertexPositionNormalTexture[] InitTerrainVertices(float[,] heightInfo, int width, int height) {
            VertexPositionNormalTexture[] terrainVerts = new VertexPositionNormalTexture[width * height];

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    terrainVerts[x + y * height].Position = new Vector3(x, heightInfo[x, y], -y);
                    terrainVerts[x + y * height].TextureCoordinate.X = x / (width - 1.0f);
                    terrainVerts[x + y * height].TextureCoordinate.Y = y / (height - 1.0f);
                }
            }
            return terrainVerts;
        }

        private static void SetupTerrainChunks(ref TerrainMapComponent terrain, GraphicsDevice graphicsDevice, Texture2D defaultTexture) {
            //loop through all the chunks to cut out from the heightmap
            for (terrain.clipX = 0; terrain.clipX < terrain.hmWidth - 1; terrain.clipX += terrain.clipW) {

                for (terrain.clipY = 0; terrain.clipY < terrain.hmHeight - 1; terrain.clipY += terrain.clipH) {
                    //use this line to see the chunks (don't use in real game)
                    //TerrainChunk t = new TerrainChunk(graphicsDevice, terrainHeightMap, new Rectangle(clipX, clipY, clipW, clipH ),
                    //                 new Vector3(clipX, 0, -clipY), GetVertexTextureNormals(new Rectangle(clipX, clipY, clipW , clipH )));
                    Rectangle clipRect = new Rectangle(terrain.clipX, terrain.clipY, terrain.clipW + 1, terrain.clipH + 1);
                    TerrainChunk t = new TerrainChunk(graphicsDevice, terrain.terrainHeightMap, clipRect,
                                     new Vector3(terrain.clipX, 0, -terrain.clipY), GetVertexTextureNormals(terrain, clipRect));

                    //apply the default texture to the chunk, so that it is visible
                    t.SetTexture(defaultTexture);

                    //add the chunk to the chunklist
                    terrain.terrainChunks.Add(t);
                }
            }
        }

        private static VertexPositionNormalTexture[] GetVertexTextureNormals(TerrainMapComponent terrain, Rectangle rect) {
            VertexPositionNormalTexture[] terrainVerts = new VertexPositionNormalTexture[rect.Width * rect.Height];

            for (int x = rect.X; x < rect.X + rect.Width; x++) {
                for (int y = rect.Y; y < rect.Y + rect.Height; y++) {
                    terrainVerts[(x - rect.X) + (y - rect.Y) * rect.Height].Normal = terrain.vertices[x + y * terrain.hmHeight].Normal;
                }
            }
            return terrainVerts;
        }

        private static void CorrectChunkPositions(ref TerrainMapComponent terrain) {
            terrain.terrainChunks[1].offsetPosition += new Vector3(0, 0, 0);
        }

        public static float GetTerrainHeight(TerrainMapComponent terrain, float x, float z) {
            if (x < 0
                || z < 0
                || x > terrain.heightInfo.GetLength(0) - 1
                || z > terrain.heightInfo.GetLength(1) - 1) {
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
            float heightLowXLowZ = terrain.heightInfo[xLow, zLow];
            float heightLowXHighZ = terrain.heightInfo[xLow, zHigh];
            float heightHighXLowZ = terrain.heightInfo[xHigh, zLow];
            float heightHighXHighZ = terrain.heightInfo[xHigh, zHigh];

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
