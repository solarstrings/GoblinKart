using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Managers;
using GameEngine.Systems;
using GoblinKart.Components;
using GoblinKart.Systems;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GoblinKart.Init {
    internal class InitGame {
        private readonly SystemManager _sm = SystemManager.Instance;

        public InitGame(ECSEngine engine)
        {
            _sm.RegisterSystem("Game", new PhysicsSystem());
            _sm.RegisterSystem("Game", new TransformSystem());
            _sm.RegisterSystem("Game", new CalcModelSphereSystem());
            _sm.RegisterSystem("Game", new ModelRenderSystem(true));

            var modelCollisionSystem = new ModelCollisionSystem();
            _sm.RegisterSystem("Game", modelCollisionSystem);

            var meshToMeshCollisionSystem = new MeshToMeshCollision(modelCollisionSystem);
            _sm.RegisterSystem("Game", meshToMeshCollisionSystem);

            _sm.RegisterSystem("Game", new PowerupCollisionSystem(meshToMeshCollisionSystem));
            InitKeyboard();
            InitKart(engine);
            InitAi(engine);
            InitCamera(engine);
            InitTerrain(engine);
            InitSkybox(engine);
            InitParticles(engine);
            InitSound(engine);

            SceneManager.Instance.SetActiveScene("Game");
            SystemManager.Instance.Category = "Game";
        }

        #region AI

        private void InitAi(ECSEngine engine)
        {
            _sm.RegisterSystem("Game", new AiSystem());

            var entity = EntityFactory.Instance.NewEntityWithTag("AiKart");
            var modelC = new ModelComponent(engine.LoadContent<Model>("Chopper"), true, false, false)
            {
                staticModel = false
            };
            ModelRenderSystem.AddMeshTransform(ref modelC, 1, Matrix.CreateRotationY(0.2f));
            ModelRenderSystem.AddMeshTransform(ref modelC, 3, Matrix.CreateRotationY(0.5f));
            ComponentManager.Instance.AddComponentToEntity(entity, modelC);

            //Create waypoints and add the AIComponent.
            var waypoints = CreateWaypoints();
            AiSystem.Waypoints = waypoints;
            var aiC = new AiComponent(waypoints[0]);
            ComponentManager.Instance.AddComponentToEntity(entity, aiC);

            ComponentManager.Instance.AddComponentToEntity(entity, new Collision3Dcomponent());

            var aiKartTransform = new TransformComponent {Position = new Vector3(0.0f, 0.0f, 0.0f)};
            aiKartTransform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, AiSystem.GetRotation(aiKartTransform.Position, aiC.Waypoint.TargetPosition));
            aiKartTransform.Scale = new Vector3(2.5f, 2.5f, 2.5f);
            ComponentManager.Instance.AddComponentToEntity(entity, aiKartTransform);

            SceneManager.Instance.AddEntityToSceneOnLayer("Game", 3, entity);
        }

        private static List<Waypoint> CreateWaypoints()
        {
            var rand = new Random(32463);
            var waypoints = new List<Waypoint>();
            for (var i = 0; i < 10; i++)
            {
                var wp = new Waypoint
                {
                    Id = i,
                    WaypointPosition = new Vector2(rand.Next(15, 500), rand.Next(-500, -15)),
                    Radius = 20
                };
                waypoints.Add(wp);
            }

            return waypoints;
        }

        #endregion

        private void InitKeyboard() {
            _sm.RegisterSystem("Game", new KeyBoardSystem());

            var keyboardControl = EntityFactory.Instance.NewEntityWithTag("keyboard");
            ComponentManager.Instance.AddComponentToEntity(keyboardControl, new KeyBoardComponent());
            var k = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(keyboardControl);

            KeyBoardSystem.AddKeyToAction(ref k, "forward", Keys.Up);
            KeyBoardSystem.AddKeyToAction(ref k, "back", Keys.Down);
            KeyBoardSystem.AddKeyToAction(ref k, "left", Keys.Left);
            KeyBoardSystem.AddKeyToAction(ref k, "right", Keys.Right);
            KeyBoardSystem.AddKeyToAction(ref k, "down", Keys.X);
            KeyBoardSystem.AddKeyToAction(ref k, "up", Keys.C);
            KeyBoardSystem.AddKeyToAction(ref k, "jump", Keys.Space);
            KeyBoardSystem.AddKeyToAction(ref k, "quit", Keys.Escape);

            SceneManager.Instance.AddEntityToSceneOnLayer("Game", 0, keyboardControl);
        }

        private void InitKart(ECSEngine engine) {
            _sm.RegisterSystem("Game", new KartControlSystem());

            Entity kart = EntityFactory.Instance.NewEntityWithTag("Kart");
            ModelComponent modelComp = new ModelComponent(engine.LoadContent<Model>("kart"), true, false,false);
            modelComp.staticModel = false;
            //ModelRenderSystem.AddMeshTransform(ref modelComp, 1, Matrix.CreateRotationY(0.2f));
            //ModelRenderSystem.AddMeshTransform(ref modelComp, 3, Matrix.CreateRotationY(0.5f));
            ComponentManager.Instance.AddComponentToEntity(kart, modelComp);
            ComponentManager.Instance.AddComponentToEntity(kart, new NetworkComponent());

            ComponentManager.Instance.AddComponentToEntity(kart, new Collision3Dcomponent());
            ComponentManager.Instance.AddComponentToEntity(kart, new PowerupComponent());
            
            // Create player comp
            ComponentManager.Instance.AddComponentToEntity(kart, new PlayerComponent {Name = "Player", Id = 1});

            TransformComponent kartTransform = new TransformComponent();
            kartTransform.Position = new Vector3(0.0f, 0.0f, 0.0f);
            kartTransform.Scale = new Vector3(2.5f, 2.5f, 2.5f);
            ComponentManager.Instance.AddComponentToEntity(kart, kartTransform);

            SceneManager.Instance.AddEntityToSceneOnLayer("Game", 3, kart);
        }

        private void InitCamera(ECSEngine engine) {
            _sm.RegisterSystem("Game", new CameraSystem());

            Entity camera = EntityFactory.Instance.NewEntityWithTag("3DCamera");
            CameraComponent cc = new CameraComponent(engine.GetGraphicsDeviceManager());
            cc.position = new Vector3(0, 20, 60);
            cc.camChasePosition = new Vector3(0f, 30f, 70f);

            ComponentManager.Instance.AddComponentToEntity(camera, cc);
            ComponentManager.Instance.AddComponentToEntity(camera, new TransformComponent());
            ComponentManager.Instance.AddComponentToEntity(camera, new PlayerComponent());
            ComponentManager.Instance.RemoveComponentFromEntity<PlayerComponent>(camera);

            CameraSystem.SetTargetEntity("Kart");
            SceneManager.Instance.AddEntityToSceneOnLayer("Game", 6, camera);
            CameraSystem.SetCameraFrustrum();
            CameraSystem.SetFarClipPlane(1000);
        }

        private void InitSkybox(ECSEngine engine)
        {
            _sm.RegisterSystem("Game", new SkyboxRenderSystem());

            Entity skyboxEnt = EntityFactory.Instance.NewEntityWithTag("Skybox");
            SkyboxComponent skyboxComp = new SkyboxComponent(engine.LoadContent<Model>("skyboxes/cube"),
                engine.LoadContent<TextureCube>("skyboxes/Sunset"),
                engine.LoadContent<Effect>("skyboxes/skybox"), 570);

            ComponentManager.Instance.AddComponentToEntity(skyboxEnt, skyboxComp);
            SceneManager.Instance.AddEntityToSceneOnLayer("Game", 0, skyboxEnt);
        }

        private void InitTerrain(ECSEngine engine)
        {
            _sm.RegisterSystem("Game", new TerrainMapRenderSystem());

            Texture2D terrainTex = engine.LoadContent<Texture2D>("Canyon");
            Texture2D defaultTex = engine.LoadContent<Texture2D>("grasstile");

            Entity terrain = EntityFactory.Instance.NewEntityWithTag("Terrain");
            TerrainMapComponent t = new TerrainMapComponent(engine.GetGraphicsDevice(), terrainTex, defaultTex, 10);
            TransformComponent tf = new TransformComponent();

            TerrainMapRenderSystem.LoadHeightMap(ref t, terrainTex, defaultTex, engine.GetGraphicsDevice());

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

            tf.World = Matrix.CreateTranslation(0, 0, 0);
            tf.Position = Vector3.Zero;
            ComponentManager.Instance.AddComponentToEntity(terrain, t);
            ComponentManager.Instance.AddComponentToEntity(terrain, tf);

            SceneManager.Instance.AddEntityToSceneOnLayer("Game", 2, terrain);
        }

        private void InitParticles(ECSEngine engine)
        {
            SystemManager.Instance.RegisterSystem("Game", new ParticleRenderSystem(engine.GetGraphicsDevice()));
            SystemManager.Instance.RegisterSystem("Game", new ParticleUpdateSystem());
            Entity SmokehParticle = EntityFactory.Instance.NewEntityWithTag("smokeh");
            SmokeParticleComponent pComp = new SmokeParticleComponent();
            ComponentManager.Instance.AddComponentToEntity(SmokehParticle, pComp);
            ParticleRenderSystem.LoadParticleEffect(engine.GetGraphicsDevice(),engine.LoadContent<Effect>("Effects/ParticleEffect"), engine.LoadContent<Texture2D>("smoke"), ref pComp);
            ParticleRenderSystem.setParticleOffsetPosition(ref pComp, new Vector3(0, 0, 10f));
            SceneManager.Instance.AddEntityToSceneOnLayer("Game", 2, SmokehParticle);
        }
        private void InitSound(ECSEngine engine)
        {
            SoundManager.Instance.SetMusicVolume(0.1f);
            var song = engine.LoadContent<Song>("Sounds/song");
            SoundManager.Instance.AddSong("song", song);
            SoundManager.Instance.PlaySong("song");

            var jump = engine.LoadContent<SoundEffect>("Sounds/jump");
            SoundManager.Instance.AddSoundEffect("jump", jump);
        }
    }
}
