using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine 
{
    public class TerrainMapRenderSystem : IRender3DSystem
    {
        public void Render(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            Entity e = ComponentManager.Instance.GetFirstEntityOfType<TerrainMapComponent>();
            Entity c = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();
            CameraComponent camera = ComponentManager.Instance.GetEntityComponent<CameraComponent>(c);
            TerrainMapComponent terrainComponent = ComponentManager.Instance.GetEntityComponent<TerrainMapComponent>(e);
            TransformComponent transformComponent = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);

            if (terrainComponent != null)
            {
                if (transformComponent != null)
                {
                    /*RasterizerState r = new RasterizerState();
                    r.CullMode = CullMode.None;
                    //r.FillMode = FillMode.WireFrame;
                    graphicsDevice.RasterizerState = r;//*/

                    for (int i = 0; i < terrainComponent.terrainChunks.Count; ++i)
                    {
                        terrainComponent.terrainChunks[i].effect.TextureEnabled = true;
                        terrainComponent.terrainChunks[i].effect.VertexColorEnabled = false;
                        terrainComponent.terrainChunks[i].effect.Texture = terrainComponent.terrainChunks[i].terrainTex;
                        terrainComponent.terrainChunks[i].effect.Projection = camera.projectionMatrix;
                        terrainComponent.terrainChunks[i].effect.View = camera.viewMatrix;
                        terrainComponent.terrainChunks[i].effect.World = transformComponent.world * Matrix.CreateTranslation(terrainComponent.terrainChunks[i].offsetPosition);
                        terrainComponent.terrainChunks[i].effect.EnableDefaultLighting();
                        
                        foreach (EffectPass p in terrainComponent.terrainChunks[i].effect.CurrentTechnique.Passes)
                        {
                            p.Apply();
                            //graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                            graphicsDevice.Indices = terrainComponent.terrainChunks[i].iBuffer;
                            graphicsDevice.SetVertexBuffer(terrainComponent.terrainChunks[i].vBuffer);
                            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, terrainComponent.terrainChunks[0].indicesLenDiv3);
                        }
                    }
                    /*foreach (TerrainChunk chunk in terrainComponent.terrainChunks)
                    {
                        chunk.effect.TextureEnabled = true;
                        chunk.effect.Texture = chunk.terrainTex;
                        chunk.effect.Projection = camera.projectionMatrix;
                        chunk.effect.View = camera.viewMatrix;
                        chunk.effect.World = transformComponent.world;
                        chunk.effect.EnableDefaultLighting();

                        foreach (EffectPass p in terrainComponent.effect.CurrentTechnique.Passes)
                        {
                            p.Apply();
                            graphicsDevice.Indices = chunk.iBuffer;
                            graphicsDevice.SetVertexBuffer(chunk.vBuffer);
                            graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.indicesLenDiv3);
                        }
                    }*/
                }
            }
        }
    }
}

