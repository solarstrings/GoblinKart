﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class TransformComponent : IComponent
    {
        public Matrix world { get; set; }
        public Vector3 position { get; set; }
        public Vector3 scale { get; set; }
        public Quaternion rotation { get; set; }

        public Vector3 vRotation { get; set; }
        public Vector3 forward { get; set; }

        // Maybe move me
        public Vector3 Velocity;

        public TransformComponent()
        {
            scale = Vector3.One;
            rotation = Quaternion.Identity;
            position = Vector3.Zero;
        }

        public void LockModelToHeight(TerrainMapComponent terComp, float offset) {
            position = new Vector3(position.X, offset + TerrainMapRenderSystem.GetTerrainHeight(terComp, position.X, Math.Abs(position.Z)), position.Z);
        }
    }
}
