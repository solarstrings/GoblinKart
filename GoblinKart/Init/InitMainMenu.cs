using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GoblinKart.Init
{
    class InitMainMenu
    {
        private SystemManager sm = SystemManager.Instance;

        public InitMainMenu(ECSEngine engine)
        {
            SystemManager.Instance.RegisterSystem("MainMenu", new SpriteRenderSystem());
        
            sm.RegisterSystem("MainMenu", new KeyBoardSystem());
            InitKeyboard();
            AddMenuAndOptions(engine);

            SystemManager.Instance.RegisterSystem("MainMenu", new MainMenuSystem(engine));
        }

        private void AddMenuAndOptions(ECSEngine engine)
        {
            Entity MenuBackground = EntityFactory.Instance.NewEntityWithTag("MainMenuBackground");
            Entity SinglePlayer = EntityFactory.Instance.NewEntityWithTag("MM_SinglePlayerOption");
            Entity Multiplayer = EntityFactory.Instance.NewEntityWithTag("MM_MultiPlayerOption");
            Entity Exit = EntityFactory.Instance.NewEntityWithTag("MM_ExitOption");

            Render2DComponent MenuBgComp = new Render2DComponent(engine.LoadContent<Texture2D>("Menu/MainMenu"));
            Position2DComponent MenuBgPos = new Position2DComponent(new Vector2(0, 0));
            ComponentManager.Instance.AddComponentToEntity(MenuBackground, MenuBgComp);
            ComponentManager.Instance.AddComponentToEntity(MenuBackground, MenuBgPos);
            SceneManager.Instance.AddEntityToSceneOnLayer("MainMenu", 0, MenuBackground);

            Render2DComponent SinglePlayerOption = new Render2DComponent(engine.LoadContent<Texture2D>("Menu/SinglePlayerSelected"));
            Position2DComponent SinglePlayerOptionPos = new Position2DComponent(new Vector2(351, 278));
            ComponentManager.Instance.AddComponentToEntity(SinglePlayer, SinglePlayerOption);
            ComponentManager.Instance.AddComponentToEntity(SinglePlayer, SinglePlayerOptionPos);
            SceneManager.Instance.AddEntityToSceneOnLayer("MainMenu", 1, SinglePlayer);
            SinglePlayer.Visible = true;
            
            Render2DComponent MultiPlayerOption = new Render2DComponent(engine.LoadContent<Texture2D>("Menu/MultiPlayerSelected"));
            Position2DComponent MultiPlayerOptionPos = new Position2DComponent(new Vector2(371, 395));
            ComponentManager.Instance.AddComponentToEntity(Multiplayer, MultiPlayerOption);
            ComponentManager.Instance.AddComponentToEntity(Multiplayer, MultiPlayerOptionPos);
            SceneManager.Instance.AddEntityToSceneOnLayer("MainMenu", 1, Multiplayer);
            Multiplayer.Visible = false;

            Render2DComponent ExitOption = new Render2DComponent(engine.LoadContent<Texture2D>("Menu/ExitSelected"));
            Position2DComponent ExitOptionPos = new Position2DComponent(new Vector2(506, 515));
            ComponentManager.Instance.AddComponentToEntity(Exit, ExitOption);
            ComponentManager.Instance.AddComponentToEntity(Exit, ExitOptionPos);
            SceneManager.Instance.AddEntityToSceneOnLayer("MainMenu", 1, Exit);
            Exit.Visible = false;            
        }
        
        private void InitKeyboard()
        {
            Entity keyboardControl = EntityFactory.Instance.NewEntityWithTag("mainMenuKeyboard");
            ComponentManager.Instance.AddComponentToEntity(keyboardControl, new KeyBoardComponent());
            KeyBoardComponent k = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(keyboardControl);

            KeyBoardSystem.AddKeyToAction(ref k, "down", Keys.Down);
            KeyBoardSystem.AddKeyToAction(ref k, "up", Keys.Up);
            KeyBoardSystem.AddKeyToAction(ref k, "apply", Keys.Enter);
            KeyBoardSystem.AddKeyToAction(ref k, "quit", Keys.Escape);

            SceneManager.Instance.AddEntityToSceneOnLayer("MainMenu", 0, keyboardControl);
        }

    }

}
