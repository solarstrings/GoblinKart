using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using Microsoft.Xna.Framework;
using GameEngine.InputDefs;

namespace GoblinKart
{
    class MainMenuSystem : IUpdateSystem
    {
        int currentSelction = 0;
        ECSEngine engine;

        private Entity single;
        private Entity multi;
        private Entity exit;
        private Entity keyboard;
        private KeyBoardComponent kbComp;

        public MainMenuSystem(ECSEngine engine)
        {
            this.engine = engine;
            List<Entity> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            single = ComponentManager.Instance.GetEntityWithTag("MM_SinglePlayerOption",sceneEntities);
            multi = ComponentManager.Instance.GetEntityWithTag("MM_MultiPlayerOption", sceneEntities);
            exit = ComponentManager.Instance.GetEntityWithTag("MM_ExitOption", sceneEntities);
            keyboard = ComponentManager.Instance.GetEntityWithTag("mainMenuKeyboard", sceneEntities);
            kbComp = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(keyboard);
        }
 
        public void Update(GameTime gameTime)
        {

            if (Utilities.CheckKeyboardAction("up", BUTTON_STATE.RELEASED, kbComp) )
            {
                currentSelction--;
                if (currentSelction < 0)
                {
                    currentSelction = 2;
                }
                SetActiveOption();
            }

            else if (Utilities.CheckKeyboardAction("down", BUTTON_STATE.RELEASED, kbComp))
            {
                currentSelction++;
                if (currentSelction > 2)
                {
                    currentSelction = 0;
                }
                SetActiveOption();
            }
            else if (Utilities.CheckKeyboardAction("apply", BUTTON_STATE.RELEASED, kbComp))
            {
                if (currentSelction == 0)
                {
                    SystemManager.Instance.Category = "Game";
                    SceneManager.Instance.SetActiveScene("Game");
                }
                else if (currentSelction == 1)
                {
                    SceneManager.Instance.SetActiveScene("MultiPlayerMenu");
                    SystemManager.Instance.Category = "MultiPlayerMenu";                    
                }
                else if (currentSelction == 2)
                {
                    engine.StopEngine();
                }
            }
        }
        private void SetActiveOption()
        {
            if (currentSelction == 0)
            {
                single.Visible = true;
                multi.Visible = false;
                exit.Visible = false;
            }
            if (currentSelction == 1)
            {
                single.Visible = false;
                multi.Visible = true;
                exit.Visible = false;
            }

            if (currentSelction == 2)
            {
                single.Visible = false;
                multi.Visible = false;
                exit.Visible = true;
            }
        }
    }
}
