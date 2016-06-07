using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Engine;
using GameEngine.Interfaces;

namespace GameEngine.Components
{
    public class CollisionComponent : IComponent 
    {
        public CollisionArea CollisionArea { get; set; }
        public bool PixelPerfect { get; set; }
        public bool Active { get; set; }
        public bool Collider { get; set; }

        private Dictionary<Entity, bool> colliders = new Dictionary<Entity, bool>();

        public CollisionComponent() : this(null, false, false)
        {
            Active = false;
        }

        public CollisionComponent(CollisionArea area, bool pixelPerfect, bool collider)
        {
            CollisionArea = area;
            PixelPerfect = pixelPerfect;
            Collider = collider;
            Active = true;
        }

        public List<Entity> GetCollidedEntities()
        {
            return colliders.Where(x => x.Value == true).Select(x => x.Key).ToList();
        }

        public void UpdateCollision(Entity entity, bool collided)
        {
            colliders[entity] = collided;
        }

        public void RemoveCollision(Entity entity)
        {
            colliders.Remove(entity);
        }
    }
}
