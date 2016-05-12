using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine {
    public class EnvironmentmappingComponent : IComponent{
        public RenderTarget2D renderTarget { get; set; }
        public RenderTargetCube renderTargetCube { get; set; }
        public TextureCube environmentMap { get; set; }

        public EnvironmentmappingComponent(RenderTarget2D target, RenderTargetCube targetCube) {
            renderTarget = target;
            renderTargetCube = targetCube;
        }

        public EnvironmentmappingComponent(RenderTarget2D target, RenderTargetCube targetCube, TextureCube map)
            : this(target, targetCube){
            environmentMap = map;
        }
    }
}
