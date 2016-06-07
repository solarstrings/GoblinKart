using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Engine;
using GameEngine.Interfaces;

namespace GoblinKart.Systems
{
    public class KartCollisionSystem : ICollisionObserver
    {
        public KartCollisionSystem(ICollisionSubject subject) 
        {
            subject.Subscribe(this);
        }
        public void OnCollision(Entity entity1, Entity entity2)
        {
            // Do mesh-mesh collision detection

            // Move the models away from each other
        }
    }
}
