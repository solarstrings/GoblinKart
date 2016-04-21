using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using GameEngine.InputDefs;

namespace GameEngine
{
    public class KeyBoardSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            List<KeyBoardComponent> keyboardComps = ComponentManager.Instance.GetComponentsFromEntities<KeyBoardComponent>(entities);

            if (keyboardComps == null) return;
            foreach (KeyBoardComponent keyboardComp in keyboardComps)
            {
                UpdateState(keyboardComp);
                UpdateActionStates(keyboardComp);
            }
        }

        public void UpdateState(KeyBoardComponent keyboardComp)
        {
            keyboardComp.OldState = keyboardComp.NewState;
            keyboardComp.NewState = Keyboard.GetState();
        }

        public void UpdateActionStates(KeyBoardComponent keyboardComp)
        {
            foreach (string action in keyboardComp.Actions.Keys)
            {
                foreach (Keys key in keyboardComp.Actions[action])
                {
                    bool newState = keyboardComp.NewState.IsKeyDown(key);
                    bool oldState = keyboardComp.OldState.IsKeyDown(key);

                    if (newState && !oldState)
                    {
                        keyboardComp.ActionStates[action] = BUTTON_STATE.PRESSED;
                        break;
                    }
                    else if (newState && oldState)
                    {
                        keyboardComp.ActionStates[action] = BUTTON_STATE.HELD;
                        break;
                    }
                    else if (!newState && oldState)
                    {
                        keyboardComp.ActionStates[action] = BUTTON_STATE.RELEASED;
                        break;
                    }
                    else
                    {
                        keyboardComp.ActionStates[action] = BUTTON_STATE.NOT_PRESSED;
                    }
                }
            }
        }

    }
}
