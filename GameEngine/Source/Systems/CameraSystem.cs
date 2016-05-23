using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using Microsoft.Xna.Framework;

namespace GameEngine {
    public class CameraSystem : IUpdateSystem {
        private Vector3 origo = new Vector3(0f, 0f, 0f);
        private Vector3 staticCameraPos = new Vector3(30.0f, 30.0f, 30f);

        public void Update(GameTime gameTime) {
            //get the camera entity
            Entity camera = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();

            //get the camera component
            CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(camera);

            //update the bounding frustum
            c.cameraFrustrum = new BoundingFrustum(c.viewMatrix * c.projectionMatrix);

            if (c.targetEntity != null) {
                List<Entity> elist = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();
                Entity e = ComponentManager.Instance.GetEntityWithTag(c.targetEntity, elist);

                //set the camera behind the target object
                Vector3 pos = c.camChasePosition;

                //get transform component from the entity the camera i following
                TransformComponent t = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);

                //get the rotation
                pos = Vector3.Transform(pos, Matrix.CreateFromQuaternion(t.rotation));

                //move the camera to the object position
                pos += t.position;

                c.position = pos;

                //update the camera up position
                Vector3 cameraUp = new Vector3(0, 1, 0);
                cameraUp = Vector3.Transform(cameraUp, Matrix.CreateFromQuaternion(t.rotation));

                //update the view
                c.viewMatrix = Matrix.CreateLookAt(pos, t.position, cameraUp);

                //update the projection
                // c.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, c.aspectRatio, c.nearClipPlane, c.farClipPlane);
            }
            //Else, a static camera that looks at origo is set up.
            else {
                c.viewMatrix = Matrix.CreateLookAt(c.position, c.target, c.upDirection);
            }
        }

        /* Sets the camera to "chase" the target entity. */
        public static void SetTargetEntity(ref CameraComponent camera, Entity target) {
            TagComponent tag = ComponentManager.Instance.GetEntityComponent<TagComponent>(target);

            if (tag != null) {
                camera.targetEntity = tag.tagName;
            }
        }

        /* Sets the camera to "chase" the tagged entity. More inefficient than directly providing an entity, use: SetTargetEntity(Entity target) when possible. */
        public static void SetTargetEntity(string EntityTag) {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();

            Entity camEnt = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();
            CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(camEnt);

            foreach (Entity e in entities) {
                TagComponent t = ComponentManager.Instance.GetEntityComponent<TagComponent>(e);
                if (t != null && c != null) {
                    if (t.tagName.Equals(EntityTag)) {
                        c.targetEntity = t.tagName;
                    }
                }
            }
        }

        public static void SetNearClipPlane(float value) {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<CameraComponent>();
            foreach (Entity enitity in entities) {
                CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(enitity);

                if (value <= 0) {
                    c.nearClipPlane = 1f;
                }

                c.nearClipPlane = value;
            }
        }

        public static void SetFarClipPlane(float value) {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<CameraComponent>();
            foreach (Entity enitity in entities) {
                CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(enitity);

                if (value >= 10000) {
                    value = 10000;
                }
                else if (value < 50) {
                    value = 50;
                }

                c.farClipPlane = value;
                Matrix.CreatePerspectiveFieldOfView(c.fieldOfView, c.aspectRatio, c.nearClipPlane, c.farClipPlane, out c.projectionMatrix);
            }
        }

        public static void SetCameraFrustrum()
        {
            //get the camera entity
            Entity camera = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();
            //get the camera component
            CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(camera);
            c.cameraFrustrum = new BoundingFrustum(c.viewMatrix * c.projectionMatrix);
        }
    }
}
