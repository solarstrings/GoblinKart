using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Source.Observers;
using GoblinKart.Components;

namespace GoblinKart.Systems
{
    public class PowerupCollisionSystem : ISystem, IMeshCollisionObserver
    {
        public PowerupCollisionSystem(IMeshCollisionSubject subject)
        {
            subject.Subscribe(this);
        }

        public void OnCollision(Entity entity1, Entity entity2)
        {
            // Handle collision
            var powerupModelComp1 = ComponentManager.Instance.GetEntityComponent<PowerupModelComponent>(entity1);
            var powerupModelcomp2 = ComponentManager.Instance.GetEntityComponent<PowerupModelComponent>(entity2);

            Entity powerupEntity = null;
            PowerupModelComponent powerupModelComp = null;
            PowerupComponent powerupComp = null;

            if (powerupModelComp1 != null)
            {
                powerupEntity = entity1;
                powerupModelComp = powerupModelComp1;
                powerupComp = ComponentManager.Instance.GetEntityComponent<PowerupComponent>(entity2);
            }
            else if (powerupModelcomp2 != null)
            {
                powerupEntity = entity2;
                powerupModelComp = powerupModelcomp2;
                powerupComp = ComponentManager.Instance.GetEntityComponent<PowerupComponent>(entity1);
            }

            if (powerupModelComp != null)
            {
                // Remove powerupModel-entity from the game
                powerupEntity.Visible = false;
                powerupEntity.Updateable = false;

                // Give player a powerup
                Random rnd = new Random();
                powerupComp.Powerup = rnd.Next(1, 10);
            }
            else
            {
                // Debug.WriteLine("Wrong type of collision!");
            }
        }
    }
}
