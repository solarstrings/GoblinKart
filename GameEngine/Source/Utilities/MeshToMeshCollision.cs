using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Source.Components;
using Microsoft.Xna.Framework;

namespace GameEngine.Source.Utilities
{
    public class MeshToMeshCollision
    {
        public void Collision()
        {
            List<Entity> entities = ComponentManager.Instance.GetAllEntitiesWithComponentType<Collision3Dcomponent>();

            foreach (var entity1 in entities)
            {
                foreach (var entity2 in entities)
                {
                    if (entity1 != entity2)
                    {
                        ModelComponent model1 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity1);
                        ModelComponent model2 = ComponentManager.Instance.GetEntityComponent<ModelComponent>(entity2);

                        TransformComponent transfrom1 =
                            ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity1);
                        TransformComponent transfrom2 =
                            ComponentManager.Instance.GetEntityComponent<TransformComponent>(entity2);

                        foreach (var mesh1 in model1.model.Meshes)
                        {
                            BoundingSphere sphere1 = mesh1.BoundingSphere;
                            sphere1 = sphere1.Transform(transfrom1.world);

                            foreach (var mesh2 in model2.model.Meshes)
                            {
                                BoundingSphere sphere2 = mesh2.BoundingSphere;
                                sphere2 = sphere2.Transform(transfrom2.world);

                                if (sphere1.Intersects(sphere2))
                                {
                                    // Notify all observers
                                    //Notify(entity1, entity2);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
