using GameEngine.Engine;

namespace GameEngine.Interfaces
{
    public interface ICollisionSubject
    {
        void Subscribe(ICollisionObserver observer);
        void Unsubscribe(ICollisionObserver observer);
        void Notify(Entity entity1, Entity entity2);
    }
}
