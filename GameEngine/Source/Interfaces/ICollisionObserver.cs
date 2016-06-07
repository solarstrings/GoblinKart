using GameEngine.Engine;

namespace GameEngine.Interfaces
{
    public interface ICollisionObserver
    {
        void OnCollision(Entity entity1, Entity entity2);
    }
}
