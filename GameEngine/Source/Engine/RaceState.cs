using GameEngine.Components;
using GameEngine.Interfaces;
using GameEngine.Managers;
using Microsoft.Xna.Framework;

namespace GameEngine.Engine
{
    public class IdleState : IState
    {
        public void DoAction(Entity entity)
        {
            var transformC = ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity);
            transformC.Velocity *= new Vector3(0.97f, 0, 0);
        }
    }
}
