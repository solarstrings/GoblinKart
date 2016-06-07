using System;
using System.Collections.Generic;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Systems {
    public class EnvironmentmappingSystem : IRender3DSystem {
        public static bool useEnvironmentMapping = true;

        public void Render(GraphicsDevice graphicsDevice, GameTime gameTime) {
            if (useEnvironmentMapping) {
                Entity camEnt = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();
                CameraComponent camComp = ComponentManager.Instance.GetEntityComponent<CameraComponent>(camEnt);

                Entity envEnt = ComponentManager.Instance.GetFirstEntityOfType<EnvironmentmappingComponent>();
                ModelComponent modelComp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(envEnt);
                EnvironmentmappingComponent envComp = ComponentManager.Instance.GetEntityComponent<EnvironmentmappingComponent>(envEnt);
                TransformComponent tComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(envEnt);

                CreateCubeMap(ref envComp, graphicsDevice, gameTime);

                RasterizerState originalRasterizerState = graphicsDevice.RasterizerState;
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                graphicsDevice.RasterizerState = rasterizerState;

                foreach (ModelMesh mesh in modelComp.model.Meshes) {
                    foreach (ModelMeshPart part in mesh.MeshParts) {
                        part.Effect = modelComp.effect;
                        part.Effect.Parameters["World"].SetValue(tComp.World * mesh.ParentBone.Transform * Matrix.CreateScale(new Vector3(20f)));
                        part.Effect.Parameters["View"].SetValue(camComp.viewMatrix);
                        part.Effect.Parameters["Projection"].SetValue(camComp.projectionMatrix);
                        part.Effect.Parameters["SkyboxTexture"].SetValue(envComp.environmentMap);
                        part.Effect.Parameters["CameraPosition"].SetValue(camComp.position);
                        part.Effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(tComp.World * mesh.ParentBone.Transform)));
                    }
                    mesh.Draw();
                }
                graphicsDevice.RasterizerState = originalRasterizerState;
            }
        }

        public static void CreateCubeMap(ref EnvironmentmappingComponent envComp, GraphicsDevice graphicsDevice, GameTime gameTime) {
            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
            Entity camEnt = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();
            CameraComponent cam = ComponentManager.Instance.GetEntityComponent<CameraComponent>(camEnt);
            Matrix camMatrix = new Matrix();

            // Render our cube map, once for each cube face( 6 times ).
            for (int i = 0; i < 6; i++) {
                // render the scene to all cubemap faces
                CubeMapFace cubeMapFace = (CubeMapFace)i;

                switch (cubeMapFace) {
                    case CubeMapFace.NegativeX:
                        {
                            camMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Left, Vector3.Up);
                            break;
                        }
                    case CubeMapFace.NegativeY:
                        {
                            camMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Down, Vector3.Forward);
                            break;
                        }
                    case CubeMapFace.NegativeZ:
                        {
                            camMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Backward, Vector3.Up);
                            break;
                        }
                    case CubeMapFace.PositiveX:
                        {
                            camMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Right, Vector3.Up);
                            break;
                        }
                    case CubeMapFace.PositiveY:
                        {
                            camMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Up, Vector3.Backward);
                            break;
                        }
                    case CubeMapFace.PositiveZ:
                        {
                            camMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
                            break;
                        }
                }

                // Set the cubemap render target, using the selected face
                graphicsDevice.SetRenderTarget(envComp.renderTargetCube, cubeMapFace);
                useEnvironmentMapping = false;
                SystemManager.Instance.RunAllRenderSystems(graphicsDevice, spriteBatch, gameTime);
                useEnvironmentMapping = true;
            }
            envComp.environmentMap = envComp.renderTargetCube;

            graphicsDevice.SetRenderTarget(null);
        }
    }
}
