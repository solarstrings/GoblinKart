using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Interfaces;
using GameEngine.Components;
using GameEngine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Systems
{
    public class CalcModelSphereSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            // Tror inte den skall skapa en ny sphere varje gametick, se om detta går att förändra
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
            sphere.Center = transform.Position;

            if (transform.Scale.X > transform.Scale.Y && transform.Scale.X > transform.Scale.Z)
                sphere.Radius *= transform.Scale.X;
            else if (transform.Scale.Y > transform.Scale.X && transform.Scale.Y > transform.Scale.Z)
                sphere.Radius *= transform.Scale.Y;
            else
                sphere.Radius *= transform.Scale.Z;

            return sphere;
        }
    }
}
