using GameEngine.Engine;
using GameEngine.Interfaces;

namespace GameEngine.Components
{
    public class AiComponent : IComponent
    {
        public Waypoint Waypoint { get; set; }

        private IState _state;

        public void SetState(IState state)
        {
            _state = state;
        }

        public IState GetState()
        {
            return _state;
        }

        public AiComponent(Waypoint wp)
        {
            Waypoint = wp;
            wp.SetRandomTargetPosition();
            SetState(new RaceState());
        }
    }
}
