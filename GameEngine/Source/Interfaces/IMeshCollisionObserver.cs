using GameEngine.Engine;

namespace GameEngine.Interfaces
{
    public interface IMeshCollisionObserver
    {
        void OnCollision(Entity entity1, Entity entity2);
    }
}
