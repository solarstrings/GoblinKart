﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Systems {
    public class CollisionSystem : IUpdateSystem {

        public void Update(GameTime gameTime) {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            if (entities == null) return;
            ComponentManager cm = ComponentManager.Instance;
            List<CollisionComponent> c = cm.GetComponentsFromEntities<CollisionComponent>(entities);
            List<CollisionComponent> colliders = c.Where(x => x.Collider).ToList();
            foreach (var collider in colliders) {
                UpdatePos(collider);
                foreach (var collide in c) {
                    UpdatePos(collide);
                    if (collide.Active && collider.Active && collide != collider) {
                        if (collider.GetType() == collide.GetType()) {
                            bool result = collider.CollisionArea.Intersect(collide.CollisionArea);
                            if (result) {
                                if (collide.PixelPerfect && collider.PixelPerfect) {
                                    var texCollider = cm.GetEntityComponent<Render2DComponent>(cm.GetEntityOfComponent<CollisionComponent>(collider)).Texture;
                                    var texCollide = cm.GetEntityComponent<Render2DComponent>(cm.GetEntityOfComponent<CollisionComponent>(collide)).Texture;
                                    result = collider.CollisionArea.IntersectPixel(collide.CollisionArea, texCollide, texCollider);
                                }
                                collider.UpdateCollision(cm.GetEntityOfComponent<CollisionComponent>(collide), result);
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
    }
}
