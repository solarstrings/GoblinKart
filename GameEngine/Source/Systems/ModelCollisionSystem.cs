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

        public void Update(GameTime gameTime)
        {
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

                        var sphere1 = model1.Sphere;
                        var sphere2 = model2.Sphere;

                        if (sphere1.Intersects(sphere2))
                        {
                            Notify(entity1, entity2);
                        }
                    }
                }
            }
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
