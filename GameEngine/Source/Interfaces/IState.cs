using GameEngine.Engine;

namespace GameEngine.Interfaces
{
    public interface IState
    {
        //This is our interface.
        //http://www.tutorialspoint.com/design_pattern/images/state_pattern_uml_diagram.jpg

        void DoAction(Entity entity);
    }
}
