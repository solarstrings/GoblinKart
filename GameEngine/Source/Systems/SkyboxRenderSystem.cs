using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{

    public class SkyboxRenderSystem : IRender3DSystem
    {
        public void Render(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            Entity cameraEnt = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();
            CameraComponent camComp = ComponentManager.Instance.GetEntityComponent<CameraComponent>(cameraEnt);

            Entity skyboxEnt = ComponentManager.Instance.GetFirstEntityOfType<SkyboxComponent>();
            SkyboxComponent skyboxComp = ComponentManager.Instance.GetEntityComponent<SkyboxComponent>(skyboxEnt);


            RasterizerState originalRasterizerState = graphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullClockwiseFace;
            graphicsDevice.RasterizerState = rasterizerState;

            //Vector3 cameraPosition = new Vector3(camComp.position.X, camComp.position.Y, camComp.position.Z);

            // Go through each pass in the effect, but we know there is only one...
            foreach (EffectPass pass in skyboxComp.skyBoxEffect.CurrentTechnique.Passes)
            {
                // Draw all of the components of the mesh, but we know the cube really
                // only has one mesh
                foreach (ModelMesh mesh in skyboxComp.SkyboxModel.Meshes)
                {
                    // Assign the appropriate values to each of the parameters
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = skyboxComp.skyBoxEffect;
                        part.Effect.Parameters["World"].SetValue(
                            Matrix.CreateScale(skyboxComp.size) * Matrix.CreateTranslation(camComp.position));
                        part.Effect.Parameters["View"].SetValue(camComp.viewMatrix);
                        part.Effect.Parameters["Projection"].SetValue(camComp.projectionMatrix);
                        part.Effect.Parameters["SkyBoxTexture"].SetValue(skyboxComp.skyBoxTextureCube);
                        part.Effect.Parameters["CameraPosition"].SetValue(camComp.position);
                    }
                    // Draw the mesh with the skybox effect
                    mesh.Draw();
                }
            }
            graphicsDevice.RasterizerState = originalRasterizerState; ;
        }
    }
}