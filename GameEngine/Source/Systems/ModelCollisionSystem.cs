using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using GameEngine.Source.Observers;
using Microsoft.Xna.Framework;

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
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<Collision3Dcomponent>();

            foreach (var entity1 in entities)
            {
                foreach (var entity2 in entities)
                {
                    ModelComponent model1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity1);
                    ModelComponent model2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity2);

                }
            }
        }

        //    foreach (var entity1 in entities)
        //    {
        //        foreach (var entity2 in entities)
        //        {
        //            if (entity1 != entity2)
        //            {
        //                ModelComponent model1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity1);
        //                ModelComponent model2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity2);

        //                TransformComponent transfrom1 =
        //                    ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity1);
        //                TransformComponent transfrom2 =
        //                    ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity2);

        //                foreach (var mesh1 in model1.model.Meshes)
        //                {
        //                    BoundingSphere sphere1 = mesh1.BoundingSphere;
        //                    sphere1 = sphere1.Transform(transfrom1.world);

        //                    foreach (var mesh2 in model2.model.Meshes)
        //                    {
        //                        BoundingSphere sphere2 = mesh2.BoundingSphere;
        //                        sphere2 = sphere2.Transform(transfrom2.world);

        //                        if (sphere1.Intersects(sphere2))
        //                        {
        //                            // Notify all observers
        //                            Notify(entity1, entity2);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

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
