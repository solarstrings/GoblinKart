using System;
using System.Collections.Generic;
using GameEngine.InputDefs;
using GameEngine;
using Microsoft.Xna.Framework;

namespace GoblinKart {
    class KartControlSystem : IUpdateSystem {
        ECSEngine engine;
        bool airBorne = true;

        const float kartGroundOffset = 1.7f;
        const float maxSpeed = 100f;
        const float maxReverseSpeed = -50f;
        const float kartAcceleration = 2f;
        const float kartTurningAcceleration = 2.8f;
        const float gravityAcceleration = -2f;
        const float jumpingAcceleration = 75f;
        const float kartFriction = 0.95f;
        const float kartDrag = 0.999f;

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

            engine.SetWindowTitle("Visible Chunks:" + terComp.numChunksInView + " Num Drawed static models: " + terComp.numModelsInView + "| Kart x: " + trsComp.position.X + " Kart y: " + trsComp.position.Y + " Kart z: " + trsComp.position.Z + " Map height: " +
                TerrainMapRenderSystem.GetTerrainHeight(terComp, trsComp.position.X, Math.Abs(trsComp.position.Z)));

            ModelRenderSystem.ResetMeshTransforms(ref kartModel);
            MoveKart(gameTime, sceneEntities, trsComp, kartModel);
            ApplyGravity(trsComp, terComp, gameTime);
            ApplyFriction(trsComp);
        }

        /* Increases negative vertical acceleration of the kart if it is above the ground. If it only slightly above the ground
        it will still be flagged as non flying to make such detection more responsive. */
        public void ApplyGravity(TransformComponent trsComp, TerrainMapComponent terComp, GameTime gameTime) {
            float distanceToGround = -(TerrainMapRenderSystem.GetTerrainHeight(terComp, trsComp.position.X, Math.Abs(trsComp.position.Z)) - trsComp.position.Y);

            if (distanceToGround <= kartGroundOffset) {
                trsComp.LockModelToHeight(terComp, kartGroundOffset);
                trsComp.Velocity.Y = 0;
                airBorne = false;
                return;
            }
            else if(distanceToGround > 10f) {
                airBorne = true;
            }
            else {
                airBorne = false;
            }
            trsComp.Velocity.Y += gravityAcceleration;
        }

        /* Adds friction to the kart, the amount depending on if it is in the air or not. */
        public void ApplyFriction(TransformComponent trsComp) {
            if (airBorne) {
                trsComp.Velocity.X *= kartDrag;
            }
            else {
                trsComp.Velocity.X *= kartFriction;
            }

        }

        private void MoveKart(GameTime gameTime, List<Entity> sceneEntities, TransformComponent trsComp, ModelComponent kartModel) {
            Entity kb = ComponentManager.Instance.GetEntityWithTag("keyboard", sceneEntities);
            Vector3 newRot = Vector3.Zero;

            if (kb != null) {
                KeyBoardComponent k = ComponentManager.Instance.GetEntityComponent<KeyBoardComponent>(kb);

                if (k != null) {
                    if (Utilities.CheckKeyboardAction("right", BUTTON_STATE.HELD, k)) {
                        newRot = new Vector3(-kartTurningAcceleration, 0f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        trsComp.vRotation = newRot;
                    }
                    else if (Utilities.CheckKeyboardAction("left", BUTTON_STATE.HELD, k)) {
                        newRot = new Vector3(kartTurningAcceleration, 0f, 0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        trsComp.vRotation = newRot;
                    }
                    else {
                        trsComp.vRotation = Vector3.Zero;
                    }
                    if (Utilities.CheckKeyboardAction("quit", BUTTON_STATE.RELEASED, k)) {
                        System.Environment.Exit(0);
                    }

                    if (Utilities.CheckKeyboardAction("forward", BUTTON_STATE.HELD, k)) {
                        if(!airBorne && trsComp.Velocity.X < maxSpeed) {
                            trsComp.Velocity += new Vector3(kartAcceleration, 0, 0);
                        }
                    }
                    if (Utilities.CheckKeyboardAction("back", BUTTON_STATE.HELD, k)) {
                        if (!airBorne && trsComp.Velocity.X > maxReverseSpeed) {
                            trsComp.Velocity += new Vector3(-kartAcceleration, 0, 0);
                        }
                    }
                    if (Utilities.CheckKeyboardAction("jump", BUTTON_STATE.RELEASED, k)) {
                        if (!airBorne) {
                            trsComp.Velocity.Y += jumpingAcceleration;
                        }
                    }
                    ModelRenderSystem.SetMeshTransform(ref kartModel, 1, Matrix.CreateRotationY(0.08f));
                    ModelRenderSystem.SetMeshTransform(ref kartModel, 3, Matrix.CreateRotationY(0.1f));
                }
            }
        }
    }
}
