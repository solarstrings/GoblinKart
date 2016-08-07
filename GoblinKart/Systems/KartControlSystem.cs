using System.Collections.Generic;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Engine.InputDefs;
using GameEngine.Interfaces;
using GameEngine.Managers;
using GameEngine.Systems;
using Microsoft.Xna.Framework;

namespace GoblinKart.Systems {
    class KartControlSystem : IUpdateSystem {
        private bool _airborne = true;
        private bool _whateverAi = true;

        private const float KartGroundOffset = 1.7f;
        /*
        const float MaxSpeed = 100f;
        const float MaxReverseSpeed = -50f;
        const float KartAcceleration = 2f;
        const float KartTurningAcceleration = 2.8f;
        const float JumpingAcceleration = 75f;
        */

        public void Update(GameTime gameTime) {
            var sceneEntities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            var terrain = ComponentManager.Instance.GetEntityWithTag("Terrain", sceneEntities);
            var terrainC = ComponentManager.Instance.GetEntityComponent<TerrainMapComponent>(terrain);

            var kart = ComponentManager.Instance.GetEntityWithTag("Kart", sceneEntities);
            var transformC = ComponentManager.Instance.GetEntityComponent<TransformComponent>(kart);
            var kartModel = ComponentManager.Instance.GetEntityComponent<ModelComponent>(kart);

            //engine.SetWindowTitle("Visible Chunks:" + terComp.numChunksInView + " Num Drawed static models: " + terComp.numModelsInView + "| Kart x: " + trsComp.position.X + " Kart y: " + trsComp.position.Y + " Kart z: " + trsComp.position.Z + " Map height: " +
            //    TerrainMapRenderSystem.GetTerrainHeight(terComp, trsComp.position.X, Math.Abs(trsComp.position.Z)));
            //engine.SetWindowTitle("xVel: " + trsComp.Velocity.X + "yVel: " + trsComp.Velocity.Y);

            ModelRenderSystem.ResetMeshTransforms(ref kartModel);
            MoveKart(gameTime, sceneEntities, transformC);
            // Move this to its own system?
            CollisionSystem.TerrainMapCollision(ref transformC, ref _airborne, terrainC, KartGroundOffset);


            // Move these to the physicssystem? Friction/gravity components?
            //PhysicsSystem.ApplyFriction(ref transformC, _airborne);
            //PhysicsSystem.ApplyGravity(ref transformC, gameTime, _airborne);

            var aiKart = ComponentManager.Instance.GetEntityWithTag("AiKart", sceneEntities);
            var transformC2 = ComponentManager.Instance.GetEntityComponent<TransformComponent>(aiKart);
            
            CollisionSystem.TerrainMapCollision(ref transformC2, ref _whateverAi, terrainC, KartGroundOffset);

            //PhysicsSystem.ApplyFriction(ref transformC2, _whateverAi);
            //PhysicsSystem.ApplyGravity(ref transformC2, gameTime, _whateverAi);
        }

        private static Quaternion CreateRotation(Vector3 v3) {
            return Quaternion.CreateFromYawPitchRoll(v3.X, v3.Y, v3.Z);
        }

        private void MoveKart(GameTime gameTime, List<Entity> sceneEntities, TransformComponent trsComp) {
            Entity kb = ComponentManager.Instance.GetEntityWithTag("keyboard", sceneEntities);
            Vector3 newRot = Vector3.Zero;

            if (kb != null) {
                KeyBoardComponent k = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(kb);

                if (k != null) {
                    if (Utilities.Utilities.CheckKeyboardAction("right", BUTTON_STATE.HELD, k)) {
                        newRot = new Vector3(-PhysicsManager.TurningAcceleration, 0f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        trsComp.Rotation *= CreateRotation(newRot);
                    }
                    else if (Utilities.Utilities.CheckKeyboardAction("left", BUTTON_STATE.HELD, k)) {
                        newRot = new Vector3(PhysicsManager.TurningAcceleration, 0f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        trsComp.Rotation *= CreateRotation(newRot);
                    }
                    if (Utilities.Utilities.CheckKeyboardAction("quit", BUTTON_STATE.RELEASED, k)) {
                        SystemManager.Instance.Category = "MainMenu";
                        SceneManager.Instance.SetActiveScene("MainMenu");
                    }

                    if (Utilities.Utilities.CheckKeyboardAction("forward", BUTTON_STATE.HELD, k)) {
                        if(!_airborne && trsComp.Velocity.X < PhysicsManager.MaxSpeed) {
                            trsComp.Velocity += new Vector3(PhysicsManager.Acceleration, 0, 0);
                        }
                    }
                    if (Utilities.Utilities.CheckKeyboardAction("back", BUTTON_STATE.HELD, k)) {
                        if (!_airborne && trsComp.Velocity.X > PhysicsManager.MaxReverseSpeed) {
                            trsComp.Velocity += new Vector3(-PhysicsManager.Acceleration, 0, 0);
                        }
                    }
                    if (Utilities.Utilities.CheckKeyboardAction("jump", BUTTON_STATE.PRESSED, k)) {
                        if (!_airborne) {
                            trsComp.Velocity.Y += PhysicsManager.JumpingAcceleration;
                            SoundManager.Instance.PlaySound("jump");
                        }                        
                    }
                }
            }
        }
    }
}
