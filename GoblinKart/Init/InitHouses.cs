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

namespace GoblinKart.Init
{
    class InitHouses
    {
        public InitHouses(ECSEngine engine)
        {
            Random rnd = new Random();
            ModelComponent house = new ModelComponent(engine.LoadContent<Model>("basichouse"), true, true,true);
            ModelComponent house2 = new ModelComponent(engine.LoadContent<Model>("basichouse"), true, true,true);
            ModelComponent powerupModel = new ModelComponent(engine.LoadContent<Model>("basichouse"), true, true,false);
            List<Entity> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            Entity terrain = ComponentManager.Instance.GetEntityWithTag("Terrain", sceneEntities);
            TerrainMapComponent tcomp = ComponentManager.Instance.GetEntityComponent<TerrainMapComponent>(terrain);

            // Init test-powerup

            Entity entity = EntityFactory.Instance.NewEntity();

            powerupModel.SetTexture(engine.LoadContent<Texture2D>("basichouse_texture1"));
            powerupModel.textured = true;
            ComponentManager.Instance.AddComponentToEntity(entity, powerupModel);

            TransformComponent tt = new TransformComponent();
            float hh = (float)rnd.Next(8, 12) / 100;
            tt.Position = new Vector3(500, 35, -50);
            tt.Scale = new Vector3(0.08f, hh, 0.08f);
            ComponentManager.Instance.AddComponentToEntity(entity, tt);

            ComponentManager.Instance.AddComponentToEntity(entity, new Collision3Dcomponent());
            ComponentManager.Instance.AddComponentToEntity(entity, new PowerupModelComponent());

            SceneManager.Instance.AddEntityToSceneOnLayer("Game", 1, entity);

            // Init houses
            for (int i = 0; i < 1; ++i)
            {
                Entity e = EntityFactory.Instance.NewEntity();

                if (i < 50)
                {
                    house.SetTexture(engine.LoadContent<Texture2D>("basichouse_texture1"));
                    house.textured = true;
                    ComponentManager.Instance.AddComponentToEntity(e, house);
                }
                else
                {
                    house2.SetTexture(engine.LoadContent<Texture2D>("basichouse_texture2"));
                    house2.textured = true;
                    ComponentManager.Instance.AddComponentToEntity(e, house2);
                }

                TransformComponent t = new TransformComponent();
                float minx = rnd.Next(128, 900);
                float minz = rnd.Next(128, 900);
                float houseHeight = (float)rnd.Next(8, 12) / 100;
                t.Position = new Vector3(minx, 0.0f, -minz);
                t.Position = new Vector3(t.Position.X, TerrainMapRenderSystem.GetTerrainHeight(tcomp, t.Position.X, Math.Abs(t.Position.Z)), t.Position.Z);
                t.Scale = new Vector3(0.08f, houseHeight, 0.08f);
                t.World = Matrix.CreateTranslation(t.Position);

                //house and house2 are identical, so it's ok to use either of them
                ModelBoundingSphereComponent sphereComp = new ModelBoundingSphereComponent(house, t.Position);

                ComponentManager.Instance.AddComponentToEntity(e, t);
                ComponentManager.Instance.AddComponentToEntity(e, sphereComp);
                ComponentManager.Instance.AddComponentToEntity(e, new Collision3Dcomponent());

                SceneManager.Instance.AddEntityToSceneOnLayer("Game", 1, e);

            }
        }
    }
}
