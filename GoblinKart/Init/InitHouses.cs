using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;
using GameEngine.Source.Components;

namespace GoblinKart.Init
{
    class InitHouses
    {
        public InitHouses(ECSEngine engine)
        {
            Random rnd = new Random();
            ModelComponent house = new ModelComponent(engine.LoadContent<Model>("basichouse"), true, true,true);
            ModelComponent house2 = new ModelComponent(engine.LoadContent<Model>("basichouse"), true, true,true);
            List<Entity> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            Entity terrain = ComponentManager.Instance.GetEntityWithTag("Terrain", sceneEntities);
            TerrainMapComponent tcomp = ComponentManager.Instance.GetEntityComponent<TerrainMapComponent>(terrain);

            for (int i = 0; i < 5; ++i)
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
                t.position = new Vector3(minx, 0.0f, -minz);
                t.position = new Vector3(t.position.X, TerrainMapRenderSystem.GetTerrainHeight(tcomp, t.position.X, Math.Abs(t.position.Z)), t.position.Z);
                t.vRotation = new Vector3(0, 0, 0);
                t.scale = new Vector3(0.08f, houseHeight, 0.08f);
                t.world = Matrix.CreateTranslation(t.position);

                //house and house2 are identical, so it's ok to use either of them
                ModelBoundingSphereComponent sphereComp = new ModelBoundingSphereComponent(house, t.position);

                ComponentManager.Instance.AddComponentToEntity(e, t);
                ComponentManager.Instance.AddComponentToEntity(e, sphereComp);
                ComponentManager.Instance.AddComponentToEntity(e, new Collision3Dcomponent());

                SceneManager.Instance.AddEntityToSceneOnLayer("Game", 1, e);

            }
        }
    }
}
