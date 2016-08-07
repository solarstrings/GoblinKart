using Microsoft.Xna.Framework;
using System;
using GameEngine.Interfaces;
using GameEngine.Systems;

namespace GameEngine.Components {
    public class TransformComponent : IComponent {
        public Matrix World { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Forward { get; set; }

        public Vector3 Velocity;
        public float Acceleration { get; set; }

        public float Angle { get; set; }

        public float Gravity { get; } = -2f;
        public float Friction = 0.95f;
        public float Drag = 0.999f;


        public TransformComponent() {
            Scale = Vector3.One;
            Rotation = Quaternion.Identity;
            Position = Vector3.Zero;
        }

        public void LockModelToHeight(TerrainMapComponent terComp, float offset) {
            Position = new Vector3(Position.X, offset + TerrainMapRenderSystem.GetTerrainHeight(terComp, Position.X, Math.Abs(Position.Z)), Position.Z);
        }
    }
}
