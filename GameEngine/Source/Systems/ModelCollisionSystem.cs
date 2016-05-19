using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using GameEngine.Source.Observers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Source.Systems
{
    public class ModelCollisionSystem : IUpdateSystem, ICollisionSubject
    {
        private readonly ICollection<ICollisionObserver> _observers = new List<ICollisionObserver>();

        // TODO This system should only check for collision and not for mesh-mesh collision.
        public void Update(GameTime gameTime)
        {
            // This system detects when two models collide
            // This system will be adjusted to just detect if two models collide and then
            // each system will use the level of collision they need (mesh to mesh or pixel-perfect).
            // The engine will implement an util-class that helps with these other levels of collision to avoid redundant code.

            if (_observers.Count != 0)
            {
                List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<Collision3Dcomponent>();

                foreach (var entity1 in entities)
                {
                    foreach (var entity2 in entities)
                    {
                        if (entity1 == entity2) continue;

                        ModelComponent model1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity1);
                        ModelComponent model2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity2);

                        TransformComponent transfrom1 =
                            ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity1);
                        TransformComponent transfrom2 =
                            ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity2);

                        var sphere1 = MergeModelSpheres(model1.model, transfrom1);
                        var sphere2 = MergeModelSpheres(model2.model, transfrom2);

                        if (sphere1.Intersects(sphere2))
                        {
                            Notify(entity1, entity2);
                        }
                    }
                }
            }
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

        public void Subscribe(ICollisionObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(ICollisionObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(Entity entity1, Entity entity2)
        {
            foreach (var observer in _observers)
            {
                observer.OnCollision(entity1, entity2);
            }
        }
    }
}
