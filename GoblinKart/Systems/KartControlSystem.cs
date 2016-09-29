using System.Collections.Generic;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Engine.InputDefs;
using GameEngine.Interfaces;
using GameEngine.Managers;
using GameEngine.Source.Components;
using GameEngine.Systems;
using Microsoft.Xna.Framework;
using GameEngine;
using System.Diagnostics;

namespace GoblinKart.Systems
{
    class KartControlSystem : IUpdateSystem
    {
        ECSEngine engine;

        public KartControlSystem(ECSEngine engine)
        {
            this.engine = engine;
        }
        public void Update(GameTime gameTime)
        {
            var sceneEntities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            var terrain = ComponentManager.Instance.GetEntityWithTag("Terrain", sceneEntities);
            var terrainC = ComponentManager.Instance.GetEntityComponent<TerrainMapComponent>(terrain);
            
            var winImage = ComponentManager.Instance.GetEntityWithTag("MP_Join", sceneEntities);
            var kart = ComponentManager.Instance.GetEntityWithTag("Kart", sceneEntities);
            var transformC = ComponentManager.Instance.GetEntityComponent<TransformComponent>(kart);
            var physComp = ComponentManager.Instance.GetEntityComponent<PhysicsComponent>(kart);
            var kartModel = ComponentManager.Instance.GetEntityComponent<ModelComponent>(kart);


            ModelRenderSystem.ResetMeshTransforms(ref kartModel);
            MoveKart(gameTime, sceneEntities, transformC, physComp);

            var aiKart = ComponentManager.Instance.GetEntityWithTag("AiKart", sceneEntities);
            var transformC2 = ComponentManager.Instance.GetEntityComponent<TransformComponent>(aiKart);
        }

        private static Quaternion CreateRotation(Vector3 v3)
        {
            return Quaternion.CreateFromYawPitchRoll(v3.X, v3.Y, v3.Z);
        }

        private void MoveKart(GameTime gameTime, List<Entity> sceneEntities, TransformComponent trsComp, PhysicsComponent physComp)
        {
            Entity kb = ComponentManager.Instance.GetEntityWithTag("keyboard", sceneEntities);
            Vector3 newRot = Vector3.Zero;

            if (kb != null)
            {
                KeyBoardComponent k = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(kb);

                if (k != null)
                {
                    if (Utilities.Utilities.CheckKeyboardAction("right", BUTTON_STATE.HELD, k))
                    {
                        newRot = new Vector3(-PhysicsManager.TurningAcceleration, 0f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        trsComp.Rotation *= CreateRotation(newRot);
                    }
                    else if (Utilities.Utilities.CheckKeyboardAction("left", BUTTON_STATE.HELD, k))
                    {
                        newRot = new Vector3(PhysicsManager.TurningAcceleration, 0f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        trsComp.Rotation *= CreateRotation(newRot);
                    }
                    if (Utilities.Utilities.CheckKeyboardAction("quit", BUTTON_STATE.RELEASED, k))
                    {
                        SystemManager.Instance.Category = "MainMenu";
                        SceneManager.Instance.SetActiveScene("MainMenu");
                    }

                    if (Utilities.Utilities.CheckKeyboardAction("forward", BUTTON_STATE.HELD, k))
                    {
                        if (trsComp.Velocity.X < PhysicsManager.MaxSpeed)
                        {
                            var acceleration = physComp.Force.X / physComp.Mass;
                            trsComp.Velocity.X += acceleration;
                        }
                    }
                    if (Utilities.Utilities.CheckKeyboardAction("back", BUTTON_STATE.HELD, k))
                    {
                        if (trsComp.Velocity.X > PhysicsManager.MaxReverseSpeed)
                        {
                            var acceleration = physComp.Force.X / physComp.Mass;
                            trsComp.Velocity.X -= acceleration;
                        }
                    }
                    if (Utilities.Utilities.CheckKeyboardAction("jump", BUTTON_STATE.PRESSED, k))
                    {
                        if (!physComp.InAir)
                        {
                            var acceleration = physComp.Force.Y / physComp.Mass;
                            trsComp.Velocity.Y += acceleration;
                            SoundManager.Instance.PlaySound("jump");
                        }
                    }
                }
            }
        }
    }
}
