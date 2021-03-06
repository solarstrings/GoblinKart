﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Engine
{
    class ModelRenderMethods
    {
        private Object lockMeshTransformUpdate = new Object();

        public void RenderBasicEffectModel(ModelComponent modelComp, TransformComponent t, CameraComponent c,bool renderOnlyNonStaticModels)
        {
            //if the terrain render system is rendering all static models
            if (renderOnlyNonStaticModels == true)
            {
                //if it's a static model
                if (modelComp.staticModel == true)
                {
                    //do nothing
                    return;
                }
            }
            Matrix[] transforms = new Matrix[modelComp.model.Bones.Count];
            modelComp.model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in modelComp.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    if (modelComp.textured)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = modelComp.texture;
                    }
                    if (modelComp.useFog)
                    {
                        effect.FogEnabled = true;
                        effect.FogColor = Color.LightGray.ToVector3();
                        effect.FogStart = modelComp.fogStart;
                        effect.FogEnd = modelComp.fogEnd;
                    }

                    effect.World = transforms[mesh.ParentBone.Index] * t.World;
                    effect.View = c.viewMatrix;
                    effect.Projection = c.projectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
