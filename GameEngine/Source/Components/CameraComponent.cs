using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class CameraComponent : IComponent
    {
        public float nearClipPlane = 1f;
        public float farClipPlane = 500f;
        public float aspectRatio { get; set; }

        public float fieldOfView { get; set; }

        public Vector3 upDirection { get; set; }
        public Vector3 position { get; set; }
        public Vector3 target { get; set; }
        public string targetEntity { get; set; }    //target entity to chase / look at

        public Matrix projectionMatrix;
        public Matrix viewMatrix;

        public Vector3 camChasePosition { get; set; }


        public void SetChaseCameraPosition(Vector3 position)
        {
            camChasePosition = position;
        }

        public int cameraMode { get; set; }
        
        public CameraComponent(GraphicsDeviceManager graphics, Vector3 position, Vector3 direction)
        {
            aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            fieldOfView = MathHelper.PiOver4;
            upDirection = Vector3.Up;
            viewMatrix = Matrix.CreateLookAt(position, direction, upDirection);
            Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane, out projectionMatrix);
            targetEntity = null;
            cameraMode = 0;
            camChasePosition = Vector3.Zero;
        }

        public CameraComponent(GraphicsDeviceManager graphics)
        {
            aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            fieldOfView = MathHelper.PiOver4;
            upDirection = Vector3.Up;
            target =  Vector3.Zero;
            position = new Vector3(10, 20, 200);
            viewMatrix = Matrix.CreateLookAt(position, target, upDirection);
            Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane, out projectionMatrix);
            targetEntity = null;
            cameraMode = 0;
            camChasePosition = Vector3.Zero;
        }

        public void SetTargetEntity(string EntityTag)
        {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();

            Entity camEnt = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();
            CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(camEnt);

            foreach (Entity e in entities)
            {
                TagComponent t = ComponentManager.Instance.GetEntityComponent<TagComponent>(e);
                if (t!=null && c!=null)
                {
                    if (t.tagName.Equals(EntityTag))
                    {
                        c.targetEntity = t.tagName;
                    }
                }
            }
        }

        public void SetCameraLookAt(Vector3 target)
        {
            //get the camera entity
            Entity camera = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();

            //get the camera component
            CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(camera);

            //set the camera to look at the target
            c.target = target;
        }

        public void SetCameraPosition(Vector3 cameraPosition)
        {
            //get the camera entity
            Entity camera = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();

            //get the camera component
            CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(camera);

            //set the camera position
            c.position = cameraPosition;
        }

        public void SetCameraFieldOfView(int degrees)
        {
            //get the camera entity
            Entity camera = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();

            //get the camera component
            CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(camera);

            //set field of view
            c.fieldOfView = MathHelper.ToRadians(degrees);
        }


        public void SetNearClipPlane(float value)
        {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<CameraComponent>();
            foreach (Entity enitity in entities)
            {
                CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(enitity);

                if (value <= 0)
                {
                    c.nearClipPlane = 1f;
                }

                c.nearClipPlane = value;
            }
        }

        public void SetFarClipPlane(float value)
        {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<CameraComponent>();
            foreach (Entity enitity in entities)
            {
                CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(enitity);

                if (value >= 10000)
                {
                    c.farClipPlane = 10000;
                }
                else if (value < 50)
                {
                    c.farClipPlane = 50;
                }
                else
                {
                    c.farClipPlane = value;
                }
            }
        }

    }
}
