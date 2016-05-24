using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GoblinKart.Init
{
    class InitMultiplayerMenu
    {
        private SystemManager sm = SystemManager.Instance;

        public InitMultiplayerMenu(ECSEngine engine)
        {
            SystemManager.Instance.RegisterSystem("MultiPlayerMenu", new SpriteRenderSystem());

            sm.RegisterSystem("MultiPlayerMenu", new KeyBoardSystem());
            InitKeyboard();
            AddMenuAndOptions(engine);
            SceneManager.Instance.SetActiveScene("MultiPlayerMenu");
            SystemManager.Instance.RegisterSystem("MultiPlayerMenu", new MultiPlayerMenuSystem(engine));
            SystemManager.Instance.RegisterSystem("MultiPlayerMenu", new NetworkServerSystem());
        }

        private void AddMenuAndOptions(ECSEngine engine)
        {
            Entity Background = EntityFactory.Instance.NewEntity();
            Entity Join = EntityFactory.Instance.NewEntityWithTag("MP_Join");
            Entity Host = EntityFactory.Instance.NewEntityWithTag("MP_Host");
            Entity Back = EntityFactory.Instance.NewEntityWithTag("MP_Back");

            Render2DComponent MenuBgComp = new Render2DComponent(engine.LoadContent<Texture2D>("Menu/MultiPlayerMenu"));
            Position2DComponent MenuBgPos = new Position2DComponent(new Vector2(0, 0));
            ComponentManager.Instance.AddComponentToEntity(Background, MenuBgComp);
            ComponentManager.Instance.AddComponentToEntity(Background, MenuBgPos);
            SceneManager.Instance.AddEntityToSceneOnLayer("MultiPlayerMenu", 0, Background);

            Render2DComponent joinPlayerOption = new Render2DComponent(engine.LoadContent<Texture2D>("Menu/MPJoinGame"));
            Position2DComponent joinPlayerOptionPos = new Position2DComponent(new Vector2(381, 221));
            ComponentManager.Instance.AddComponentToEntity(Join, joinPlayerOption);
            ComponentManager.Instance.AddComponentToEntity(Join, joinPlayerOptionPos);
            SceneManager.Instance.AddEntityToSceneOnLayer("MultiPlayerMenu", 1, Join);
            Join.Visible = true;

            Render2DComponent hostOption = new Render2DComponent(engine.LoadContent<Texture2D>("Menu/MPHostGame"));
            Position2DComponent hostOptionPos = new Position2DComponent(new Vector2(367, 376));
            ComponentManager.Instance.AddComponentToEntity(Host, hostOption);
            ComponentManager.Instance.AddComponentToEntity(Host, hostOptionPos);
            SceneManager.Instance.AddEntityToSceneOnLayer("MultiPlayerMenu", 1, Host);
            Host.Visible = false;

            Render2DComponent backOption = new Render2DComponent(engine.LoadContent<Texture2D>("Menu/back"));
            Position2DComponent backOptionPos = new Position2DComponent(new Vector2(494, 527));
            ComponentManager.Instance.AddComponentToEntity(Back, backOption);
            ComponentManager.Instance.AddComponentToEntity(Back, backOptionPos);
            SceneManager.Instance.AddEntityToSceneOnLayer("MultiPlayerMenu", 1, Back);
            Back.Visible = false;
        }

        private void InitKeyboard()
        {
            Entity keyboardControl = EntityFactory.Instance.NewEntityWithTag("MP_Keyboard");
            ComponentManager.Instance.AddComponentToEntity(keyboardControl, new KeyBoardComponent());
            KeyBoardComponent k = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(keyboardControl);

            KeyBoardSystem.AddKeyToAction(ref k, "down", Keys.Down);
            KeyBoardSystem.AddKeyToAction(ref k, "up", Keys.Up);
            KeyBoardSystem.AddKeyToAction(ref k, "apply", Keys.Enter);
            KeyBoardSystem.AddKeyToAction(ref k, "quit", Keys.Escape);

            SceneManager.Instance.AddEntityToSceneOnLayer("MultiPlayerMenu", 0, keyboardControl);
        }
    }
}
