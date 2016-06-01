using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using GameEngine.InputDefs;

namespace GameEngine
{
    public class GamePadSystem : IUpdateSystem
    {
        

        public void Update(GameTime gameTime)
        {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            List<GamePadComponent> gamePadComps = ComponentManager.Instance.GetComponentsFromEntities<GamePadComponent>(entities);
            if (gamePadComps == null) return;
            foreach (GamePadComponent gamePadComp in gamePadComps)
            {
                UpdateStates(gamePadComp);
                UpdateActionStates(gamePadComp);
            }
        }

        private void UpdateStates(GamePadComponent gamePadComp)
        {
            gamePadComp.OldState = gamePadComp.NewState;
            gamePadComp.NewState = GamePad.GetState(gamePadComp.PlayerIndex);
        }

        private void UpdateActionStates(GamePadComponent gamePadComp)
        {
            foreach(string key in gamePadComp.Actions.Keys)
            {
                foreach (Buttons button in gamePadComp.Actions[key])
                {
                    bool newState = gamePadComp.NewState.IsButtonDown(button);
                    bool oldState = gamePadComp.OldState.IsButtonDown(button);
  
                    if (newState && !oldState)
                    {
                        gamePadComp.ActionStates[key] = BUTTON_STATE.PRESSED;
                        break;
                    }
                    else if (newState && oldState)
                    {
                        gamePadComp.ActionStates[key] = BUTTON_STATE.HELD;
                        break;
                    }
                    else if (!newState && oldState)
                    {
                        gamePadComp.ActionStates[key] = BUTTON_STATE.RELEASED;
                        break;
                    }
                    else
                    {
                        gamePadComp.ActionStates[key] = BUTTON_STATE.NOT_PRESSED;
                    }
                }
            }
        }

    }
}
