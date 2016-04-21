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
        public void Render(GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            List<List<Entity>> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllLayers();
            Entity Ecamera = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();
            CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(Ecamera);

            if (sceneEntities == null)
            {
                return;
            }
            if(c == null)
            {
                return;
            }

            for (int i = 0; i < sceneEntities.Count; ++i)
            {
                foreach (Entity entity in sceneEntities[i])
                {
                    if (entity.Visible)
                    {
                        ModelComponent m = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity);

                        //if the entity has a model component
                        if (m != null)
                        {
                            //loop through all mesh transforms in the model
                            foreach (var pair in m.MeshTransforms)
                            {
                                //update the model transforms
                                ChangeBoneTransform(m, pair.Key, pair.Value);
                            }

                            TransformComponent t = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity);
                            //if there is a transform component
                            if (t != null)
                            {
                                //If the model uses monogames built-in basic effects
                                if (m.useBasicEffect)
                                {
                                    //render the model with basic effects
                                    RenderBasicEffectModel(m, t, c);
                                }
                            }

                        }
                    }
                }
            }
        }
        private void RenderBasicEffectModel(ModelComponent modelComp,TransformComponent t, CameraComponent c)
        {
            /*
            Matrix worldMatrix = Matrix.CreateScale(t.scale)    //Scale
                * Matrix.CreateRotationY(MathHelper.Pi)         //Rotate model 180 degrees (compensate for upside-down model)
                * Matrix.CreateFromQuaternion(t.rotation)       //Rotate
                * Matrix.CreateTranslation(t.position);         //Translate
                */
            Matrix[] transforms = new Matrix[modelComp.model.Bones.Count];
            modelComp.model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in modelComp.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

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
    }
}
