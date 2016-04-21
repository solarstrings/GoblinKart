using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class CameraSystem : IUpdateSystem
    {
        private Vector3 origo = new Vector3(0f, 0f, 0f);
        private Vector3 staticCameraPos = new Vector3(30.0f, 30.0f, 30f);
        
        public void Update(GameTime gameTime)
        {
            //get the camera entity
            Entity camera = ComponentManager.Instance.GetFirstEntityOfType<CameraComponent>();

            //get the camera component
            CameraComponent c = ComponentManager.Instance.GetEntityComponent<CameraComponent>(camera);

            if (c.targetEntity!=null)
            {
                List<Entity> elist = ComponentManager.Instance.GetAllEntitiesWithComponentType<ModelComponent>();
                Entity e = ComponentManager.Instance.GetEntityWithTag(c.targetEntity,elist);

                //set the camera behind the target object
                Vector3 pos = c.camChasePosition;

                //get transform component from the entity the camera i following
                TransformComponent t = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);

                //get the rotation
                pos = Vector3.Transform(pos, Matrix.CreateFromQuaternion(t.rotation));

                //move the camera to the object position
                pos += t.position;

                //update the camera up position
                Vector3 cameraUp = new Vector3(0, 1, 0);
                cameraUp = Vector3.Transform(cameraUp, Matrix.CreateFromQuaternion(t.rotation));

                //update the view
                c.viewMatrix = Matrix.CreateLookAt(pos, t.position, cameraUp);

                //update the projection
               // c.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, c.aspectRatio, c.nearClipPlane, c.farClipPlane);
            }
            //Else, a static camera that looks at origo is set up.
            else
            {
                c.viewMatrix = Matrix.CreateLookAt(c.position, c.target, c.upDirection);
            }
        }
    }
}
