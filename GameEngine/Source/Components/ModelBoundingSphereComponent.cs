using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class ModelBoundingSphereComponent : IComponent
    {
        public BoundingSphere sphere { get; set; }

        public ModelBoundingSphereComponent(ModelComponent modelComp, Vector3 position)
        {
            this.sphere = CreateBoundingSphere(modelComp, position);
        }

        private BoundingSphere CreateBoundingSphere(ModelComponent modelComp, Vector3 position)
        {
            BoundingSphere sphere = new BoundingSphere(position, 0);

            //build a boundingshpere from the model
            foreach (ModelMesh mesh in modelComp.model.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.
                             CreateMerged(sphere, mesh.BoundingSphere);
            }
            return sphere;
        }
    }
}
