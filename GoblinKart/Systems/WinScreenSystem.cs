using System.Collections.Generic;
using GameEngine;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Engine.InputDefs;
using GameEngine.Interfaces;
using GameEngine.Managers;
using Microsoft.Xna.Framework;

namespace GoblinKart.Systems
{
    class WinScreenSystem : IUpdateSystem
    {

        ECSEngine engine;
        private Entity winScreen;
        private Entity keyboard;
        private KeyBoardComponent kbComp;

        public WinScreenSystem(ECSEngine engine)
        {
            SceneManager.Instance.SetActiveScene("WinScreen");
            SystemManager.Instance.Category = "WinScreen";
            this.engine = engine;
            List<Entity> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllEntities();

            winScreen = ComponentManager.Instance.GetEntityWithTag("WinScreenImage", sceneEntities);
            keyboard = ComponentManager.Instance.GetEntityWithTag("WinScreenKeyboard", sceneEntities);
            kbComp = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(keyboard);
        }

        public void Update(GameTime gameTime)
        {

            if (Utilities.Utilities.CheckKeyboardAction("quit", BUTTON_STATE.RELEASED, kbComp))
            {
                SystemManager.Instance.Category = "MainMenu";
                SceneManager.Instance.SetActiveScene("MainMenu");
            }
        }
    }
}
