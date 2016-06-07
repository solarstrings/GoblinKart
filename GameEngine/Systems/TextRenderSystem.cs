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
    public class TextRenderSystem : IRenderSystem
    {
        /// <summary>
        /// Render a string to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            foreach (Entity e in entities)
            {
                if (e.Visible)
                {
                    TextRenderComponent t = ComponentManager.Instance.GetEntityComponent<TextRenderComponent>(e);
                    Position2DComponent p = ComponentManager.Instance.GetEntityComponent<Position2DComponent>(e);
                    
                    if(p != null && t!=null){
                        spriteBatch.DrawString(t.Font, t.Text, p.Position, t.TextColor);
                    }
                }
            }
        }
    }
}
