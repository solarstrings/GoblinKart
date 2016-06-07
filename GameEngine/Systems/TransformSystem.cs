using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;
using Microsoft.Xna.Framework;

namespace GameEngine.Systems
{
    public class TransformSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();

            List<TransformComponent> transformComponents =
                ComponentManager.Instance.GetComponentsFromEntities<TransformComponent>(entities);

            Parallel.ForEach(transformComponents, t =>
            {
                t.Forward = Vector3.Transform(Vector3.Forward, t.Rotation);
                //Update world matrix
                t.World = Matrix.CreateScale(t.Scale)
                          *Matrix.CreateFromQuaternion(t.Rotation)
                          *Matrix.CreateTranslation(t.Position);
            });
        }
    }
}
