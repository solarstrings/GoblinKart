using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Engine.InputDefs;
using Microsoft.Xna.Framework;
using GameEngine.Interfaces;
using GameEngine.Managers;
using GameEngine.Systems;
using GoblinKart.Systems;

namespace GoblinKart.Systems
{
    class MultiPlayerMenuSystem : IUpdateSystem
    {
        int currentSelction = 0;
        ECSEngine engine;

        private Entity join;
        private Entity host;
        private Entity back;
        private Entity keyboard;
        private KeyBoardComponent kbComp;

        public MultiPlayerMenuSystem(ECSEngine engine)
        {
            this.engine = engine;
            List<Entity> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            join = ComponentManager.Instance.GetEntityWithTag("MP_Join", sceneEntities);
            host = ComponentManager.Instance.GetEntityWithTag("MP_Host", sceneEntities);
            back = ComponentManager.Instance.GetEntityWithTag("MP_Back", sceneEntities);
            keyboard = ComponentManager.Instance.GetEntityWithTag("MP_Keyboard", sceneEntities);
            kbComp = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(keyboard);
        }

        public void Update(GameTime gameTime)
        {

            if (Utilities.Utilities.CheckKeyboardAction("up", BUTTON_STATE.RELEASED, kbComp))
            {
                currentSelction--;
                if (currentSelction < 0)
                {
                    currentSelction = 2;
                }
                SetActiveOption();
            }

            else if (Utilities.Utilities.CheckKeyboardAction("down", BUTTON_STATE.RELEASED, kbComp))
            {
                currentSelction++;
                if (currentSelction > 2)
                {
                    currentSelction = 0;
                }
                SetActiveOption();
            }
            else if (Utilities.Utilities.CheckKeyboardAction("apply", BUTTON_STATE.RELEASED, kbComp))
            {
                if (currentSelction == 0)
                {
                    Join();
                    var ai = ComponentManager.Instance.GetAllEntitiesWithComponentType<AiComponent>()[0];
                    ai.Updateable = false;
                    ai.Visible = false;
                }
                if (currentSelction == 1)
                {
                    Host();
                }
                if (currentSelction == 2)
                {
                    SystemManager.Instance.Category = "MainMenu";
                    SceneManager.Instance.SetActiveScene("MainMenu");
                }
            }
        }

        private void Host()
        {
            // Init server
            NetworkManager.Instance.InitNetworkServer();
            SystemManager.Instance.RegisterSystem("MultiplayerMenu", new NetworkServerRecieveMessage());

            //SystemManager.Instance.Category = "MultiplayerMenu";
            //SceneManager.Instance.SetActiveScene("MultiplayerMenu");
        }

        private void Join()
        {
            if (NetworkManager.Instance.InitClientConnection())
            {
                Console.WriteLine("Connection successfull!");
                SystemManager.Instance.RegisterSystem("Game", new NetworkClientRecieveMessage(engine));
                SystemManager.Instance.RegisterSystem("Game", new NetworkClientSendInfo());
                SystemManager.Instance.Category = "Game";
                SceneManager.Instance.SetActiveScene("Game");

            }
            else
            {
                Console.WriteLine("Failed to connect/find to/a server");
            }
            
        }

        private void SetActiveOption()
        {
            if (currentSelction == 0)
            {
                join.Visible = true;
                host.Visible = false;
                back.Visible = false;
            }
            if (currentSelction == 1)
            {
                join.Visible = false;
                host.Visible = true;
                back.Visible = false;
            }

            if (currentSelction == 2)
            {
                join.Visible = false;
                host.Visible = false;
                back.Visible = true;
            }
        }
    }
}

