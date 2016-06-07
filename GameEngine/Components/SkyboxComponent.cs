using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameEngine.Components
{
    public class SkyboxComponent : IComponent
    {
        public Model SkyboxModel { get; set; }
        public TextureCube skyBoxTextureCube { get; set; }
        public Effect skyBoxEffect { get; set; }
        public float size { get; set; }

        public SkyboxComponent(Model SkyboxModel, TextureCube skyBoxTextureCube, Effect skyBoxEffect, float size)
        {
            this.SkyboxModel = SkyboxModel;
            this.skyBoxTextureCube = skyBoxTextureCube;
            this.skyBoxEffect = skyBoxEffect;
            this.size = size;
        }
    }
}
