namespace GameEngine.Source.Observers
{
    public interface ICollisionObserver
    {
        void OnCollision(Entity entity1, Entity entity2);
    }
}
