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
    class YouLooseSystem : IUpdateSystem
    {
        ECSEngine engine;
        private Entity winScreen;
        private Entity keyboard;
        private KeyBoardComponent kbComp;

        public YouLooseSystem(ECSEngine engine)
        {
            SceneManager.Instance.SetActiveScene("LooseScreen");
            SystemManager.Instance.Category = "LooseScreen";
            this.engine = engine;
            List<Entity> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllEntities();

            winScreen = ComponentManager.Instance.GetEntityWithTag("LooseScreenImage", sceneEntities);
            keyboard = ComponentManager.Instance.GetEntityWithTag("LooseScreenKeyboard", sceneEntities);
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
