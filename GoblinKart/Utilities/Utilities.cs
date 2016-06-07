using GameEngine.Components;
using GameEngine.Engine.InputDefs;

namespace GoblinKart.Utilities
{
    class Utilities
    {
        public static bool CheckKeyboardAction(string action, BUTTON_STATE state, KeyBoardComponent kbc)
        {
            if (kbc != null)
            {
                if (kbc.GetActionState(action) == state)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
