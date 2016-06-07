using GameEngine.Engine;

namespace GameEngine.Interfaces
{
    public interface IMeshCollisionSubject
    {
        void Subscribe(IMeshCollisionObserver observer);
        void Unsubscribe(IMeshCollisionObserver observer);
        void Notify(Entity entity1, Entity entity2);
    }
}
