using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class TerrainRenderSystem : IRender3DSystem
    {
        public void Render(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            Entity e = ComponentManager.Instance.GetFirstEntityOfType<TerrainComponent>();
            Entity c = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();
            CameraComponent camera = ComponentManager.Instance.GetEntityComponent<CameraComponent>(c);
            TerrainComponent terrainComponent = ComponentManager.Instance.GetEntityComponent<TerrainComponent>(e);
            TransformComponent transformComponent = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);

            if(terrainComponent!=null)
            {
                if(transformComponent!=null)
                {
                    terrainComponent.effect.TextureEnabled = true;
                    terrainComponent.effect.Texture = terrainComponent.terrainTex;
                    terrainComponent.effect.Projection = camera.projectionMatrix;
                    terrainComponent.effect.View = camera.viewMatrix;
                    terrainComponent.effect.World = transformComponent.world;
                    terrainComponent.effect.EnableDefaultLighting();

                    /*RasterizerState r = new RasterizerState();
                    r.CullMode = CullMode.None;
                    //r.FillMode = FillMode.WireFrame;
                    graphicsDevice.RasterizerState = r;//*/

                    foreach (EffectPass p in terrainComponent.effect.CurrentTechnique.Passes)
                    {
                        p.Apply();
                        graphicsDevice.Indices = terrainComponent.iBuffer;
                        graphicsDevice.SetVertexBuffer(terrainComponent.vBuffer);
                        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, terrainComponent.indicesLenDiv3);
                    }
                }
            }
        }
        
    }
}
