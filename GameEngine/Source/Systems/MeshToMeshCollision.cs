using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;
using Microsoft.Xna.Framework;

namespace GameEngine.Systems
{
    public class MeshToMeshCollision : ISystem, ICollisionObserver, IMeshCollisionSubject
    {
        public MeshToMeshCollision(ICollisionSubject subject)
        {
            subject.Subscribe(this);
        }

        private readonly List<IMeshCollisionObserver> _observers = new List<IMeshCollisionObserver>();
        public void OnCollision(Entity entity1, Entity entity2)
        {
            if (_observers.Count != 0)
            {
                var model1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity1);
                var model2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity2);

                var transfrom1 = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity1);
                var transfrom2 = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity2);

                foreach (var mesh1 in model1.model.Meshes)
                {
                    var sphere1 = mesh1.BoundingSphere;
                    sphere1 = sphere1.Transform(transfrom1.World);

                    foreach (var mesh2 in model2.model.Meshes)
                    {
                        var sphere2 = mesh2.BoundingSphere;
                        sphere2 = sphere2.Transform(transfrom2.World);

                        if (sphere1.Intersects(sphere2))
                        {
                            // Maybe send something else, like which mesh it collided with?
                            Notify(entity1, entity2);
                        }
                    }
                }
            }
        }

        public void Subscribe(IMeshCollisionObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IMeshCollisionObserver observer)
        {
            _observers.Add(observer);
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
