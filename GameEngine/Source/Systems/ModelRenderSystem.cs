using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace GameEngine
{
    public class ModelRenderSystem : IRender3DSystem
    {
        DebugRenderBoundingBox boxRenderer;
        BoundingBoxToWorldSpace boxConvert;
        bool renderBoxInitialised = false;

        private bool modelInCameraFrustrum=false;

        public void Render(GraphicsDevice graphicsDevice, GameTime gameTime) {
            if (renderBoxInitialised.Equals(false)) {
                boxConvert = new BoundingBoxToWorldSpace();
                boxRenderer = new DebugRenderBoundingBox(graphicsDevice);
                renderBoxInitialised = true;
            }

            List<List<Entity>> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllLayers();
            Entity Ecamera = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();
            CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(Ecamera);
            Entity modelCount = ComponentManager.Instance.GetFirstEntityOfType<ModelCountComponent>();
            ModelCountComponent modelsInView = ComponentManager.Instance.GetEntityComponent<ModelCountComponent>(modelCount);

            if (sceneEntities == null || c == null) {
                return;
            }
            if (modelsInView != null)
                modelsInView.numModelsInView = 0;

            for (int i = 0; i < sceneEntities.Count; ++i) {
                foreach (Entity entity in sceneEntities[i]) {
                    if (entity.Visible) {
                        ModelComponent m = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity);
                        ModelBoundingBoxComponent b = ComponentManager.Instance.GetEntityComponent<ModelBoundingBoxComponent>(entity);

                        //if the entity has a model component
                        if (m != null) {
                            //loop through all mesh transforms in the model
                            foreach (var pair in m.meshTransforms) {
                                //update the model transforms
                                ChangeBoneTransform(m, pair.Key, pair.Value);
                            }

                            TransformComponent t = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity);
                            //if there is a transform component
                            if (t != null) {
                                //if the model has bounding boxes
                                if (b != null) {
                                    modelInCameraFrustrum = false;
                                    foreach (BoundingBox bb in b.boundingBoxes) {
                                        Vector3 leftRightVector = Matrix.Transpose(c.viewMatrix).Right;
                                        boxRenderer.RenderBoundingBox(bb, t.world, c.viewMatrix, c.projectionMatrix);
                                        BoundingBox box = boxConvert.ConvertBoundingBoxToWorldCoords(b.boundingBoxes[0], Matrix.CreateTranslation(t.position));
                                        BoundingSphere s = BoundingSphere.CreateFromBoundingBox(box);

                                        if (c.cameraFrustrum.Contains(s) != ContainmentType.Disjoint) {
                                            modelInCameraFrustrum = true;
                                            break;
                                        }
                                    }
                                    if (modelInCameraFrustrum == true) {
                                        if (modelsInView != null)
                                            modelsInView.numModelsInView++;

                                        //If the model uses monogames built-in basic effects
                                        if (m.useBasicEffect) {
                                            //render the model with basic effects
                                            RenderBasicEffectModel(m, t, c);
                                        }
                                    }
                                }
                                //if the model doesn't have any bounding boxes
                                else {
                                    //If the model uses monogames built-in basic effects
                                    if (m.useBasicEffect) {
                                        //render the model with basic effects
                                        RenderBasicEffectModel(m, t, c);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RenderBasicEffectModel(ModelComponent modelComp, TransformComponent t, CameraComponent c) {
            /*
            Matrix worldMatrix = Matrix.CreateScale(t.scale)    //Scale
                * Matrix.CreateRotationY(MathHelper.Pi)         //Rotate model 180 degrees (compensate for upside-down model)
                * Matrix.CreateFromQuaternion(t.rotation)       //Rotate
                * Matrix.CreateTranslation(t.position);         //Translate
                */
            Matrix[] transforms = new Matrix[modelComp.model.Bones.Count];
            modelComp.model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in modelComp.model.Meshes) {
                foreach (BasicEffect effect in mesh.Effects) {
                    effect.EnableDefaultLighting();
                    if (modelComp.textured) {
                        effect.TextureEnabled = true;
                        effect.Texture = modelComp.texture;
                    }
                    if (modelComp.useFog) {
                        effect.FogEnabled = true;
                        effect.FogColor = Color.LightGray.ToVector3();
                        effect.FogStart = modelComp.fogStart;
                        effect.FogEnd = modelComp.fogEnd;
                    }

                    effect.World = transforms[mesh.ParentBone.Index] * t.world;
                    effect.View = c.viewMatrix;
                    effect.Projection = c.projectionMatrix;
                }
                mesh.Draw();
                //Console.WriteLine("Model X:" + t.position.X + " Y:" + t.position.Y +" Z:" + t.position.Z);
            }
        }

        /// <summary>
        /// This function rotates the given bone by the given matrix
        /// </summary>
        /// <param name="boneIndex"></param>
        /// <param name="t"></param>
        private void ChangeBoneTransform(ModelComponent modelComp,int boneIndex, Matrix t)
        {
            modelComp.model.Bones[boneIndex].Transform = t * modelComp.model.Bones[boneIndex].Transform;
        }

        public static void AddMeshTransform(ref ModelComponent model, int bone, Matrix t) {
            model.meshTransforms.Add(bone, t);
        }

        public static void SetMeshTransform(ref ModelComponent model, int bone, Matrix t) {
            if (model.meshTransforms.ContainsKey(bone)) {
                model.meshTransforms[bone] = t;
            }
        }

        public static void RemoveMeshTransform(ref ModelComponent model, int bone) {
            if (model.meshTransforms.ContainsKey(bone)) {
                model.meshTransforms.Remove(bone);
            }
        }

        public static void ResetMeshTransforms(ref ModelComponent model) {
            SetMeshTransform(ref model, 1, Matrix.CreateRotationY(0.0f));
            SetMeshTransform(ref model, 3, Matrix.CreateRotationY(0.0f));
        }
    }
}
