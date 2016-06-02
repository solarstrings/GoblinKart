using System;
using System.Collections.Generic;
using GameEngine.InputDefs;
using GameEngine;
using Microsoft.Xna.Framework;

namespace GoblinKart {
    class KartControlSystem : IUpdateSystem {
        ECSEngine engine;
        bool airborne = true;

        const float KartGroundOffset = 1.7f;
        /*
        const float MaxSpeed = 100f;
        const float MaxReverseSpeed = -50f;
        const float KartAcceleration = 2f;
        const float KartTurningAcceleration = 2.8f;
        const float JumpingAcceleration = 75f;
        */

        public KartControlSystem(ECSEngine engine) {
            this.engine = engine;
        }

        public void Update(GameTime gameTime) {
            List<Entity> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            Entity kart = ComponentManager.Instance.GetEntityWithTag("Kart", sceneEntities);
            TransformComponent trsComp = ComponentManager.Instance.GetEntityComponent<TransformComponent>(kart);
            ModelComponent kartModel = ComponentManager.Instance.GetEntityComponent<ModelComponent>(kart);

            Entity terrain = ComponentManager.Instance.GetEntityWithTag("Terrain", sceneEntities);
            TerrainMapComponent terComp = ComponentManager.Instance.GetEntityComponent<TerrainMapComponent>(terrain);

            //engine.SetWindowTitle("Visible Chunks:" + terComp.numChunksInView + " Num Drawed static models: " + terComp.numModelsInView + "| Kart x: " + trsComp.position.X + " Kart y: " + trsComp.position.Y + " Kart z: " + trsComp.position.Z + " Map height: " +
            //    TerrainMapRenderSystem.GetTerrainHeight(terComp, trsComp.position.X, Math.Abs(trsComp.position.Z)));
            //engine.SetWindowTitle("xVel: " + trsComp.Velocity.X + "yVel: " + trsComp.Velocity.Y);

            ModelRenderSystem.ResetMeshTransforms(ref kartModel);
            MoveKart(gameTime, sceneEntities, trsComp, kartModel);
            CollisionSystem.TerrainMapCollision(ref trsComp, ref airborne, terComp, KartGroundOffset);
            PhysicsSystem.ApplyFriction(ref trsComp, airborne);
            PhysicsSystem.ApplyGravity(ref trsComp, gameTime, airborne);

        }

        private Quaternion CreateRotation(Vector3 v3) {
            return Quaternion.CreateFromYawPitchRoll(v3.X, v3.Y, v3.Z);
        }

        private void MoveKart(GameTime gameTime, List<Entity> sceneEntities, TransformComponent trsComp, ModelComponent kartModel) {
            Entity kb = ComponentManager.Instance.GetEntityWithTag("keyboard", sceneEntities);
            Vector3 newRot = Vector3.Zero;

            if (kb != null) {
                KeyBoardComponent k = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(kb);

                if (k != null) {
                    if (Utilities.CheckKeyboardAction("right", BUTTON_STATE.HELD, k)) {
                        newRot = new Vector3(-PhysicsManager.TurningAcceleration, 0f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        trsComp.Rotation *= CreateRotation(newRot);
                    }
                    else if (Utilities.CheckKeyboardAction("left", BUTTON_STATE.HELD, k)) {
                        newRot = new Vector3(PhysicsManager.TurningAcceleration, 0f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        trsComp.Rotation *= CreateRotation(newRot);
                    }
                    if (Utilities.CheckKeyboardAction("quit", BUTTON_STATE.RELEASED, k)) {
                        SystemManager.Instance.Category = "MainMenu";
                        SceneManager.Instance.SetActiveScene("MainMenu");
                    }

                    if (Utilities.CheckKeyboardAction("forward", BUTTON_STATE.HELD, k)) {
                        if(!airborne && trsComp.Velocity.X < PhysicsManager.MaxSpeed) {
                            trsComp.Velocity += new Vector3(PhysicsManager.Acceleration, 0, 0);
                        }
                    }
                    if (Utilities.CheckKeyboardAction("back", BUTTON_STATE.HELD, k)) {
                        if (!airborne && trsComp.Velocity.X > PhysicsManager.MaxReverseSpeed) {
                            trsComp.Velocity += new Vector3(-PhysicsManager.Acceleration, 0, 0);
                        }
                    }
                    if (Utilities.CheckKeyboardAction("jump", BUTTON_STATE.PRESSED, k)) {
                        if (!airborne) {
                            trsComp.Velocity.Y += PhysicsManager.JumpingAcceleration;
                            SoundManager.Instance.PlaySound("jump");
                        }                        
                    }
                    ModelRenderSystem.SetMeshTransform(ref kartModel, 1, Matrix.CreateRotationY(0.08f));
                    ModelRenderSystem.SetMeshTransform(ref kartModel, 3, Matrix.CreateRotationY(0.1f));
                }
            }
        }
    }
}
