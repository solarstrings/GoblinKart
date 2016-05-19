using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Systems
{
    public class CalcModelSphereSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            var entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<Collision3Dcomponent>();

            Parallel.ForEach(entities, e =>
            {
                var modelComp = ComponentManager.Instance.GetEntityComponent<ModelComponent>(e);
                var transformComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);

                modelComp.Sphere = MergeModelSpheres(modelComp.model, transformComp);
            });
        }

        public BoundingSphere MergeModelSpheres(Model model, TransformComponent transform)
        {
            var sphere = new BoundingSphere();

            foreach (var mesh in model.Meshes)
            {
                if (sphere.Radius == 0)
                    sphere = mesh.BoundingSphere;
                else
                    sphere = BoundingSphere.
                             CreateMerged(sphere, mesh.BoundingSphere);
            }
            sphere.Center = transform.position;

            if (transform.scale.X > transform.scale.Y && transform.scale.X > transform.scale.Z)
                sphere.Radius *= transform.scale.X;
            else if (transform.scale.Y > transform.scale.X && transform.scale.Y > transform.scale.Z)
                sphere.Radius *= transform.scale.Y;
            else
                sphere.Radius *= transform.scale.Z;

            return sphere;
        }
    }
}
