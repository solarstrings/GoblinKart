using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class TransformSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();

            List<TransformComponent> transformComponents =
                ComponentManager.Instance.GetComponentsFromEntities<TransformComponent>(entities);

            foreach(TransformComponent t in transformComponents)
            {
                    var qRotation = Quaternion.CreateFromYawPitchRoll(t.vRotation.X, t.vRotation.Y, t.vRotation.Z);
                    t.rotation *= qRotation;
                    t.forward = Vector3.Transform(Vector3.Forward, t.rotation);
                    //Update world matrix
                    t.world = Matrix.CreateScale(t.scale)
                              * Matrix.CreateFromQuaternion(t.rotation)
                              * Matrix.CreateTranslation(t.position);
            }
        }
    }
}
