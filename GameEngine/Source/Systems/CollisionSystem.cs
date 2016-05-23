using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine {
    public class CollisionSystem : IUpdateSystem {

        public void Update(GameTime gameTime) {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            if (entities != null) {
                ComponentManager cm = ComponentManager.Instance;
                List<CollisionComponent> c = cm.GetComponentsFromEntities<CollisionComponent>(entities);
                List<CollisionComponent> colliders = c.Where(x => x.Collider == true).ToList();
                foreach (CollisionComponent collider in colliders) {
                    UpdatePos(collider);
                    foreach (CollisionComponent collide in c) {
                        UpdatePos(collide);
                        if (collide.Active && collider.Active && collide != collider) {
                            if (collider.GetType() == collide.GetType()) {
                                bool result = collider.CollisionArea.Intersect(collide.CollisionArea);
                                if (result) {
                                    if (collide.PixelPerfect && collider.PixelPerfect) {
                                        Texture2D texCollider = cm.GetEntityComponent<Render2DComponent>(cm.GetEntityOfComponent<CollisionComponent>(collider)).Texture;
                                        Texture2D texCollide = cm.GetEntityComponent<Render2DComponent>(cm.GetEntityOfComponent<CollisionComponent>(collide)).Texture;
                                        result = collider.CollisionArea.IntersectPixel(collide.CollisionArea, texCollide, texCollider);
                                    }
                                    collider.UpdateCollision(cm.GetEntityOfComponent<CollisionComponent>(collide), (bool)result);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdatePos(CollisionComponent c) {
            ComponentManager cm = ComponentManager.Instance;
            Entity e = cm.GetEntityOfComponent<CollisionComponent>(c);
            Position2DComponent p = cm.GetEntityComponent<Position2DComponent>(e);
            if (p != null) {
                c.CollisionArea.SetX(p.Position.X);
                c.CollisionArea.SetY(p.Position.Y);
            }
        }

        /* Flagges the object as flying if it is above the ground. If it only slightly above the ground
        it will still be flagged as non flying to make such detection more responsive. */
        public static void TerrainMapCollision(ref TransformComponent trsComp, ref bool airborne, TerrainMapComponent terComp, float groundOffset) {
            float distanceToGround = -(TerrainMapRenderSystem.GetTerrainHeight(terComp, trsComp.position.X, Math.Abs(trsComp.position.Z)) - trsComp.position.Y);

            if (distanceToGround <= groundOffset) {
                trsComp.LockModelToHeight(terComp, groundOffset);
                trsComp.velocity.Y = 0;
                airborne = false;
                return;
            }
            else if (distanceToGround > 10f) {
                airborne = true;
            }
            else {
                airborne = false;
            }
        }
    }
}
