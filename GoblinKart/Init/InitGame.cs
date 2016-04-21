using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameEngine;
using Microsoft.Xna.Framework.Input;

namespace GoblinKart.Init
{
    class InitGame
    {
        private SystemManager sm = SystemManager.Instance;

        public InitGame(ECSEngine engine)
        {

            sm.RegisterSystem("Game", new TransformSystem());
            sm.RegisterSystem("Game", new ModelRenderSystem());
            sm.RegisterSystem("Game", new TerrainMapRenderSystem());
            sm.RegisterSystem("Game", new KeyBoardSystem());
            sm.RegisterSystem("Game", new CameraSystem());
            sm.RegisterSystem("Game", new ChopperControlSystem(engine));

            Entity keyboardControl = EntityFactory.Instance.NewEntityWithTag("keyboard");
            ComponentManager.Instance.AddComponentToEntity(keyboardControl, new KeyBoardComponent());
            KeyBoardComponent k = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(keyboardControl);

            k.AddKeyToAction("forward", Keys.Up);
            k.AddKeyToAction("back", Keys.Down);
            k.AddKeyToAction("left", Keys.Left);
            k.AddKeyToAction("right", Keys.Right);
            k.AddKeyToAction("down", Keys.Z);
            k.AddKeyToAction("up", Keys.A);
            k.AddKeyToAction("quit", Keys.Escape);

            Entity chopper = EntityFactory.Instance.NewEntityWithTag("Chopper");
            ModelComponent cm = new ModelComponent(engine.LoadContent<Model>("Chopper"), true);
            cm.AddMeshTransform(1, Matrix.CreateRotationY(0.2f));
            cm.AddMeshTransform(3, Matrix.CreateRotationY(0.5f));
            ComponentManager.Instance.AddComponentToEntity(chopper, cm);
            TransformComponent chopperTransform = new TransformComponent();
            chopperTransform.position = new Vector3(0.0f, 0.0f, 0.0f);
            chopperTransform.vRotation = new Vector3(0,0,0);
            chopperTransform.scale = new Vector3(2.5f, 2.5f, 2.5f);
            ComponentManager.Instance.AddComponentToEntity(chopper, chopperTransform);

            Entity Camera = EntityFactory.Instance.NewEntityWithTag("3DCamera");
            CameraComponent cc = new CameraComponent(engine.GetGraphicsDeviceManager());
            cc.position = new Vector3(0, 20, 60);
            
            //Use this line instead to see the back rotor rotate, hard to see from behind :)
            //cc.SetChaseCameraPosition(new Vector3(10f, 20f, 40f));
            cc.SetChaseCameraPosition(new Vector3(0f, 30f, 70f));
                        
            ComponentManager.Instance.AddComponentToEntity(Camera, cc);
            ComponentManager.Instance.AddComponentToEntity(Camera, new TransformComponent());
            cc.SetTargetEntity("Chopper");

            Entity Terrain = EntityFactory.Instance.NewEntityWithTag("Terrain");
            TerrainMapComponent t = new TerrainMapComponent(engine.GetGraphicsDevice(), engine.LoadContent<Texture2D>("Canyon"), engine.LoadContent<Texture2D>("grasstile"),10);
            TransformComponent tf = new TransformComponent();
            t.SetTextureToChunk(0, engine.LoadContent<Texture2D>("LTCornerroad"));
            t.SetTextureToChunk(1, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(2, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(3, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(4, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(5, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(6, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(7, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(8, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(9, engine.LoadContent<Texture2D>("LBCornerroad"));
            t.SetTextureToChunk(10, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(19, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(20, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(29, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(30, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(39, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(40, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(49, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(50, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(59, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(60, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(69, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(70, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(79, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(80, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(89, engine.LoadContent<Texture2D>("horizontalroad"));
            t.SetTextureToChunk(90, engine.LoadContent<Texture2D>("RTCornerroad"));
            t.SetTextureToChunk(99, engine.LoadContent<Texture2D>("RBCornerroad"));
            t.SetTextureToChunk(98, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(97, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(96, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(95, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(94, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(93, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(92, engine.LoadContent<Texture2D>("verticalroad"));
            t.SetTextureToChunk(91, engine.LoadContent<Texture2D>("verticalroad"));


            tf.world = Matrix.CreateTranslation(0, 0, 0);
            tf.position = Vector3.Zero;
            ComponentManager.Instance.AddComponentToEntity(Terrain, t);
            ComponentManager.Instance.AddComponentToEntity(Terrain, tf);

            SceneManager.Instance.AddEntityToSceneOnLayer("Game",6, Camera);
            SceneManager.Instance.AddEntityToSceneOnLayer("Game",3, chopper);
            SceneManager.Instance.AddEntityToSceneOnLayer("Game",2, Terrain);
            SceneManager.Instance.SetActiveScene("Game");
            SceneManager.Instance.AddEntityToSceneOnLayer("Game", 0, keyboardControl);
            SystemManager.Instance.Category = "Game";
        }
    }
}
