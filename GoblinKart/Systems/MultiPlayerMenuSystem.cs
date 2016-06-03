using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using Microsoft.Xna.Framework;
using GameEngine.InputDefs;
using GameEngine.Source.Managers;
using GameEngine.Source.Systems;
using GoblinKart.Systems;

namespace GoblinKart
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

            if (Utilities.CheckKeyboardAction("up", BUTTON_STATE.RELEASED, kbComp))
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
                    Join();
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
            NetworkManager.Instance.InitNetworkServer();
            //SystemManager.Instance.RegisterSystem("Game", new NetworkServerSystem());
            SystemManager.Instance.RegisterSystem("Game", new NetworkServerRecieveMessage());
            SystemManager.Instance.Category = "Game";
            SceneManager.Instance.SetActiveScene("Game");
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

