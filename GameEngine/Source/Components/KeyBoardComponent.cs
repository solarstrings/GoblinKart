using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using GameEngine.InputDefs;

namespace GameEngine {
    public class KeyBoardComponent : IComponent {
        public Dictionary<string, List<Keys>> Actions { get; set; }
        public Dictionary<string, BUTTON_STATE> ActionStates { get; set; }
        public KeyboardState NewState { get; set; }
        public KeyboardState OldState { get; set; }

        public KeyBoardComponent() {
            Actions = new Dictionary<string, List<Keys>>();
            ActionStates = new Dictionary<string, BUTTON_STATE>();
        }

        public BUTTON_STATE? GetActionState(string action) {
            if (ActionStates.ContainsKey(action)) {
                return ActionStates[action];
            }
            return null;
        }

        public void SetAction(string action, BUTTON_STATE state) {
            ActionStates[action] = state;
        }

    }
}
