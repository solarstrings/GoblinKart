using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class ModelComponent : IComponent
    {
        public Model model {get;set;}

        public bool useBasicEffect { get; set; }
        public Dictionary<int, Matrix> MeshTransforms { get; set; }

        public ModelComponent(Model model, bool useBasicEffect)
        {
            this.model = model;
            this.useBasicEffect = useBasicEffect;
            MeshTransforms = new Dictionary<int, Matrix>();
        }

        public void AddMeshTransform(int bone,Matrix t)
        {
            MeshTransforms.Add(bone, t);
        }
        public void SetMeshTransform(int bone,Matrix t)
        {
            if(MeshTransforms.ContainsKey(bone))
            {
                MeshTransforms[bone] = t;
            }
        }

        public void RemoveMeshTransform(int bone)
        {
            if (MeshTransforms.ContainsKey(bone))
            {
                MeshTransforms.Remove(bone);
            }
        }

        public void ResetMeshTransforms() {
            SetMeshTransform(1, Matrix.CreateRotationY(0.0f));
            SetMeshTransform(3, Matrix.CreateRotationY(0.0f));
        }
    }
}
