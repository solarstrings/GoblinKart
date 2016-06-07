using GameEngine.Engine;

namespace GameEngine.Interfaces
{
    public interface IState
    {
        void DoAction(Entity entity);
    }
}
