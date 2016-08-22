using System.Diagnostics;
using GameEngine.Components;
using GameEngine.Interfaces;
using GameEngine.Managers;
using GameEngine.Systems;
using Microsoft.Xna.Framework;

namespace GameEngine.Engine
{
    public class CountdownState : IState
    {
        public void DoAction(Entity entity)
        {
            //TODO: Implement countdown.
            var aiC = ComponentManager.Instance.GetEntityComponent<AiComponent>(entity);
            Debug.WriteLine("Changing State to Race");
            aiC.SetState(new RaceState());
        }
    }
}
