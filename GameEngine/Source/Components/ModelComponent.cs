using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameEngine {
    public class ModelComponent : IComponent {
        public Model model { get; set; }
        public BoundingSphere Sphere { get; set; }
        public bool textured { get; set; }
        public Texture2D texture { get; set; }
        public Effect effect { get; set; }
        public bool useBasicEffect { get; set; }
        public bool useFog { get; set; }
        public float fogStart { get; private set; }
        public float fogEnd { get; private set; }

        public bool staticModel { get; set; }

        public Dictionary<int, Matrix> meshTransforms { get; set; }

        public ModelComponent(Model model, bool useBasicEffect, bool useFog,bool isStatic) {
            this.model = model;
            textured = false;
            this.useBasicEffect = useBasicEffect;
            this.useFog = useFog;
            fogStart = 300f;
            fogEnd = 400f; 
            meshTransforms = new Dictionary<int, Matrix>();
            staticModel = isStatic;
        }

        public void SetTexture(Texture2D texture) {
            this.texture = texture;
        }

        public void SetFog(float start, float end) {
            fogStart = start;
            fogEnd = end;
        }
    }
}
