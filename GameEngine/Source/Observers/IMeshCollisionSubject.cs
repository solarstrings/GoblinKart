using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Observers
{
    public interface IMeshCollisionSubject
    {
        void Subscribe(IMeshCollisionObserver observer);
        void Unsubscribe(IMeshCollisionObserver observer);
        void Notify(Entity entity1, Entity entity2);
    }
}
