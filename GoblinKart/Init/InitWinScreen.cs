using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Managers;
using GameEngine.Systems;
using GoblinKart.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GoblinKart.Init
{
    class InitWinScreen
    {
        private SystemManager sm = SystemManager.Instance;

        public InitWinScreen(ECSEngine engine)
        {
            SystemManager.Instance.RegisterSystem("WinScreen", new SpriteRenderSystem());

            sm.RegisterSystem("WinScreen", new KeyBoardSystem());
            InitKeyboard();
            AddMenuAndOptions(engine);

            SystemManager.Instance.RegisterSystem("WinScreen", new WinScreenSystem(engine));
            SceneManager.Instance.SetActiveScene("WinScreen");
            SystemManager.Instance.Category = "WinScreen";
        }

        private void AddMenuAndOptions(ECSEngine engine)
        {
            Entity winScreen = EntityFactory.Instance.NewEntityWithTag("WinScreenImage");

            Render2DComponent Comp = new Render2DComponent(engine.LoadContent<Texture2D>("youwin"));
            Position2DComponent Pos = new Position2DComponent(new Vector2(400, 210));
            ComponentManager.Instance.AddComponentToEntity(winScreen, Comp);
            ComponentManager.Instance.AddComponentToEntity(winScreen, Pos);
            SceneManager.Instance.AddEntityToSceneOnLayer("WinScreen", 0, winScreen);
        }

        private void InitKeyboard()
        {
            Entity keyboardControl = EntityFactory.Instance.NewEntityWithTag("WinScreenKeyboard");
            ComponentManager.Instance.AddComponentToEntity(keyboardControl, new KeyBoardComponent());
            KeyBoardComponent k = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(keyboardControl);

            KeyBoardSystem.AddKeyToAction(ref k, "quit", Keys.Escape);

            SceneManager.Instance.AddEntityToSceneOnLayer("WinScreen", 0, keyboardControl);
        }

    }

}
