using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Observers
{
    public interface IMeshCollisionObserver
    {
        void OnCollision(Entity entity1, Entity entity2);
    }
}
