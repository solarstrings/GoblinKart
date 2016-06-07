using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameEngine.Systems
{
    public class SpriteRenderSystem : IRenderSystem
    {
        /// <summary>
        /// Renders a sprite on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            try
            {
                List<List<Entity>> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllLayers();
                for (int i = 0; i < sceneEntities.Count; ++i)
                {
                    foreach (Entity entity in sceneEntities[i])
                    {
                        if (entity.Visible)
                        {
                            Render2DComponent r = ComponentManager.Instance.GetEntityComponent<Render2DComponent>(entity);
                            if (r != null)
                            {
                                Position2DComponent p = ComponentManager.Instance.GetEntityComponent<Position2DComponent>(entity);
                                AnimationComponent a = ComponentManager.Instance.GetEntityComponent<AnimationComponent>(entity);

                                if (p != null)
                                {
                                    if (a != null)
                                    {
                                        if (a.visible)
                                        {
                                            r.DestRect = new Rectangle((int)p.Position.X, (int)p.Position.Y, a.SourceRect.Width, a.SourceRect.Height);
                                            spriteBatch.Draw(r.Texture, r.DestRect, a.SourceRect, Color.White);
                                        }
                                    }
                                    else
                                    {
                                        r.DestRect = new Rectangle((int)p.Position.X, (int)p.Position.Y, r.Width, r.Height);
                                        r.SourceRect = new Rectangle(0, 0, r.Width, r.Height);
                                        spriteBatch.Draw(r.Texture, r.DestRect, r.SourceRect, Color.White);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
