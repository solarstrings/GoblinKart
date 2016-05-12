using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Source.Observers;

namespace GoblinKart.Systems
{
    public class PowerupCollisionSystem :ISystem, ICollisionObserver
    {
        public PowerupCollisionSystem(ICollisionSubject subject)
        {
            subject.Subscribe(this);
        }

        public void OnCollision(Entity entity1, Entity entity2)
        {
            // Handle collision

            Debug.WriteLine("Collision!");
        }
    }
}
