using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Components;
using GameEngine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Engine
{
    /// <summary>
    /// Factory to create new entities
    /// </summary>
    public class EntityFactory
    {
        private static EntityFactory instance;
        public static EntityFactory Instance
        {
            get 
            {
                if (instance == null)
                    instance = new EntityFactory();
                return instance;
            }
        }

        private EntityFactory() { }

        /// <summary>
        /// Creates new entity (Do not use for entities that need collision handling)
        /// </summary>
        /// <returns> A new entity</returns>
        public Entity NewEntity() 
        {
            Entity newEnt = new Entity();;
            newEnt.Visible = true;
            return newEnt;
        }

        /// <summary>
        /// Creates a new entity with a tag
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public Entity NewEntityWithTag(string tagName)
        {
            Entity newEnt = new Entity();
            newEnt.Visible = true;
            ComponentManager.Instance.AddComponentToEntity(newEnt, new TagComponent(tagName));
            return newEnt;
        }

        /// <summary>
        /// Creates a new entity with a tag
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public Entity NewEntityTextString(SpriteFont font, string textString,Color color,int x,int y)
        {
            Entity newEnt = new Entity();
            newEnt.Visible = true;
            ComponentManager.Instance.AddComponentToEntity(newEnt, new TextRenderComponent(textString, color, font));
            ComponentManager.Instance.AddComponentToEntity(newEnt, new Position2DComponent(new Vector2(x, y)));
            return newEnt;
        }
        /// <summary>
        /// Creates a new non-movable game entity with a tag(chest,rock etc)
        /// Includes: a render, tag, rectangle collision and position2D component
        /// Visible: yes
        /// Updateable: no
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public Entity NewGameEntity(string tagName,Texture2D texture, int x,int y)
        {
            Render2DComponent renderComponent = new Render2DComponent(texture);
            //TODO : Redo Collision
            //RectangleCollisionComponent collisionRect = new RectangleCollisionComponent(renderComponent.DestRect);
            Entity newEnt = new Entity();
            newEnt.Visible = true;
            ComponentManager.Instance.AddComponentToEntity(newEnt, renderComponent);
            ComponentManager.Instance.AddComponentToEntity(newEnt, new TagComponent(tagName));
            ComponentManager.Instance.AddComponentToEntity(newEnt,new Position2DComponent(new Vector2(x,y)));
            //ComponentManager.Instance.AddComponentToEntity(newEnt,collisionRect);
            return newEnt;
        }

        /// <summary>
        /// This method creates an animated game entity
        /// Includes: a render, tag, rectangle collision, position2D and animation component
        /// Visible: yes
        /// Updateable: yes
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        public Entity NewAnimatedGameEntity(string tagName, Texture2D texture,int x,int y, double timePerFrame, int animationRectWidht,int animationRectHeight)
        {
            Render2DComponent renderComponent = new Render2DComponent(texture);
            // TODO: Readd Collision
            //RectangleCollisionComponent collisionRect = new RectangleCollisionComponent(renderComponent.DestRect);
            Entity newEnt = new Entity();
            newEnt.Visible = true;
            newEnt.Updateable = true;
            ComponentManager.Instance.AddComponentToEntity(newEnt, new AnimationComponent(timePerFrame,animationRectWidht,animationRectHeight,texture.Width,texture.Height));
            ComponentManager.Instance.AddComponentToEntity(newEnt, renderComponent);
            ComponentManager.Instance.AddComponentToEntity(newEnt, new TagComponent(tagName));
            ComponentManager.Instance.AddComponentToEntity(newEnt, new Position2DComponent(new Vector2(x, y)));
            //ComponentManager.Instance.AddComponentToEntity(newEnt, collisionRect);
            return newEnt;
        }

        /// <summary>
        /// Creates a new game entity with a tag(enemy, player, spear, car, box)
        /// Includes: Tag, Position2D, rectangleCollision and velocity component
        /// Visible: yes
        /// Updateable: yes
        /// </summary>
        /// <param name="tagName">Tag for the entity</param>
        /// <returns></returns>
        public Entity NewMoveableGameEntity(string tagName,Texture2D texture, float speed, int x,int y)
        {
            Render2DComponent renderComponent = new Render2DComponent(texture);
            // TODO: Readd Collision
            //RectangleCollisionComponent collisionRect = new RectangleCollisionComponent(new Rectangle(x,y,renderComponent.Width,renderComponent.Height));
            Entity newEnt = new Entity();
            newEnt.Visible = true;
            newEnt.Updateable = true;
            ComponentManager.Instance.AddComponentToEntity(newEnt, renderComponent);
            ComponentManager.Instance.AddComponentToEntity(newEnt, new TagComponent(tagName));
            ComponentManager.Instance.AddComponentToEntity(newEnt, new Position2DComponent(new Vector2(x, y)));
            ComponentManager.Instance.AddComponentToEntity(newEnt, new Velocity2DComponent(new Vector2(0, 0), speed));
            //ComponentManager.Instance.AddComponentToEntity(newEnt, collisionRect);
            return newEnt;
        }

        /// <summary>
        /// Creates a new game entity with a tag(enemy, player, spear, car, box)
        /// Includes: Tag, Position2D, rectangleCollision, velocity and animation component
        /// Visible: yes
        /// Updateable: yes
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="texture"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="speed"></param>
        /// <param name="timePerFrame"></param>
        /// <param name="animationRectWidht"></param>
        /// <param name="animationRectHeight"></param>
        /// <returns></returns>
        public Entity NewAnimatedMovableGameEntity(string tagName, Texture2D texture, int x, int y,float speed, double timePerFrame, int animationRectWidht, int animationRectHeight)
        {
            Render2DComponent renderComponent = new Render2DComponent(texture);
            // TODO: Readd Collision.
            //RectangleCollisionComponent collisionRect = new RectangleCollisionComponent(renderComponent.DestRect);
            Entity newEnt = new Entity();
            newEnt.Visible = true;
            newEnt.Updateable = true;
            ComponentManager.Instance.AddComponentToEntity(newEnt, new AnimationComponent(timePerFrame, animationRectWidht, animationRectHeight, texture.Width, texture.Height));
            ComponentManager.Instance.AddComponentToEntity(newEnt, renderComponent);
            ComponentManager.Instance.AddComponentToEntity(newEnt, new TagComponent(tagName));
            ComponentManager.Instance.AddComponentToEntity(newEnt, new Position2DComponent(new Vector2(x, y)));
            ComponentManager.Instance.AddComponentToEntity(newEnt, new Velocity2DComponent(new Vector2(0, 0), speed));
            //ComponentManager.Instance.AddComponentToEntity(newEnt, collisionRect);
            return newEnt;
        }
    }
}
