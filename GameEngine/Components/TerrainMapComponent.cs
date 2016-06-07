using System.Collections.Generic;
using GameEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Components {
    public class TerrainMapComponent : IComponent {
        public List<TerrainChunk> terrainChunks { get; set; }
        public Texture2D terrainHeightMap { get; set; }
        public Texture2D terrainMap;

        public float[,] heightInfo;

        public int numChunks;
        public int clipX, clipY;
        public int clipW, clipH;
        public int hmWidth, hmHeight;

        public VertexPositionNormalTexture[] vertices { get; set; }
        public int[] indices { get; set; }
        public int numChunksInView { get; set; }

        public int numModelsInView { get; set; }

        public TerrainMapComponent(GraphicsDevice graphicsDevice, Texture2D HeightmapTexture, Texture2D defaultTexture, int numChunks) {
            terrainChunks = new List<TerrainChunk>();
            terrainHeightMap = HeightmapTexture;
            hmWidth = terrainHeightMap.Width;
            hmHeight = terrainHeightMap.Height;
            this.numChunks = numChunks;

            //Get clip width and clip height.
            clipW = hmWidth / numChunks;
            clipH = hmHeight / numChunks;
        }

        public void SetTextureToChunk(int chunk, Texture2D texture) {
            terrainChunks[chunk].SetTexture(texture);
        }
    }
}
