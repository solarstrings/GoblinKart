using System.Diagnostics;
using GameEngine.Components;
using GameEngine.Interfaces;
using GameEngine.Managers;
using Microsoft.Xna.Framework;

namespace GameEngine.Engine
{
    public class CountdownState : IState
    {
        //Should of course not use a object variable for this.
        private int _timer = 10000;

        public void DoAction(Entity entity)
        {
            //TODO: Shared countdown timer.
            if (_timer <= 0)
            {
                var aiC = ComponentManager.Instance.GetEntityComponent<AiComponent>(entity);
                Debug.WriteLine("Changing State to Race");
                aiC.SetState(new RaceState());
            }
            _timer--;
        }
    }
}
