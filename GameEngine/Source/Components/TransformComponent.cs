using Microsoft.Xna.Framework;
using System;

namespace GameEngine {
    public class TransformComponent : IComponent {
        public Matrix world { get; set; }
        public Vector3 position { get; set; }
        public Vector3 scale { get; set; }
        public Quaternion rotation { get; set; }

        public Vector3 vRotation { get; set; }
        public Vector3 forward { get; set; }
        public Vector3 velocity;

        public float gravity { get; } = -2f;
        public float friction = 0.95f;
        public float drag = 0.999f;

        public TransformComponent() {
            scale = Vector3.One;
            rotation = Quaternion.Identity;
            position = Vector3.Zero;
        }

        public void LockModelToHeight(TerrainMapComponent terComp, float offset) {
            position = new Vector3(position.X, offset + TerrainMapRenderSystem.GetTerrainHeight(terComp, position.X, Math.Abs(position.Z)), position.Z);
        }
    }
}
