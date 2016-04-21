using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class SystemManager
    {
        public string Category { set; get; }
        private static SystemManager instance = null;
        
        private SystemManager(){}

        public static SystemManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemManager();
                    instance.Category = null;
                }
                return instance;
            }
        }

        Dictionary<string, Dictionary<Type, IUpdateSystem>> updateSystemDictionary = new Dictionary<string, Dictionary<Type, IUpdateSystem>>();
        Dictionary<string, Dictionary<Type, IRenderSystem>> renderSystemDictionary = new Dictionary<string, Dictionary<Type, IRenderSystem>>();
        Dictionary<string, Dictionary<Type, IRender3DSystem>> render3DSystemDictionary = new Dictionary<string, Dictionary<Type, IRender3DSystem>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="system"></param>
        public void RegisterSystem(string category, ISystem system)
        {
            //if the category hasn't been assigned yet
            if (Category==null)
            {
                //set it to the first category that comes in.
                Category = category;
            }

            if (system is IUpdateSystem)
            {
                if (!updateSystemDictionary.ContainsKey(category))
                {
                    updateSystemDictionary[category] = new Dictionary<Type, IUpdateSystem>();
                }
                updateSystemDictionary[category][system.GetType()] = (IUpdateSystem)system;
            }
            if (system is IRenderSystem)
            {
                if (!renderSystemDictionary.ContainsKey(category))
                {
                    renderSystemDictionary[category] = new Dictionary<Type, IRenderSystem>();
                }
                renderSystemDictionary[category][system.GetType()] = (IRenderSystem)system;
            }
            if (system is IRender3DSystem)
            {
                if (!render3DSystemDictionary.ContainsKey(category))
                {
                    render3DSystemDictionary[category] = new Dictionary<Type, IRender3DSystem>();
                }
                render3DSystemDictionary[category][system.GetType()] = (IRender3DSystem)system;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="system"></param>
        public void DeregisterSystem(string category, Type system)
        {
            if (system is IUpdateSystem)
            {
                if (updateSystemDictionary.ContainsKey(category))
                {
                    if (updateSystemDictionary[category].ContainsKey(system))
                    {
                        updateSystemDictionary[category].Remove(system);
                    }
                }
            }
            else if (system is IRenderSystem)
            {
                if (renderSystemDictionary.ContainsKey(category))
                {
                    if (renderSystemDictionary[category].ContainsKey(system))
                    {
                        renderSystemDictionary[category].Remove(system);
                    }
                }
            }
            else if (system is IRender3DSystem)
            {
                if (render3DSystemDictionary.ContainsKey(category))
                {
                    if (render3DSystemDictionary[category].ContainsKey(system))
                    {
                        render3DSystemDictionary[category].Remove(system);
                    }
                }
            }
        }

        /// <summary>
        /// This method deregisters a whole category
        /// </summary>
        /// <param name="category"></param>
        /// <param name="system"></param>
        public void DeregisterCategory(string category,Type system)
        {
            if (system is IUpdateSystem)
            {
                if (updateSystemDictionary.ContainsKey(category))
                {
                    updateSystemDictionary.Remove(category);
                }
            }
            else if (system is IRenderSystem)
            {
                if (renderSystemDictionary.ContainsKey(category))
                {
                    renderSystemDictionary.Remove(category);
                }
            }
            else if (system is IRender3DSystem)
            {
                if (render3DSystemDictionary.ContainsKey(category))
                {
                    render3DSystemDictionary.Remove(category);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        internal void RunAllRenderSystems(GraphicsDevice graphicsDevice,SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (render3DSystemDictionary.Count > 0)
            {
                if (render3DSystemDictionary.ContainsKey(Category))
                {
                    foreach (IRender3DSystem ren3Dsys in render3DSystemDictionary[Category].Values)
                    {
                        ren3Dsys.Render(graphicsDevice, gameTime);
                    }
                }
            }
            if (renderSystemDictionary.Count > 0)
            {
                if (renderSystemDictionary.ContainsKey(Category))
                {
                    foreach (IRenderSystem renSys in renderSystemDictionary[Category].Values)
                    {
                        renSys.Render(spriteBatch, gameTime);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        internal void RunAllUpdateSystems(GameTime gameTime)
        {
            if (updateSystemDictionary.Count > 0)
            {
                if (updateSystemDictionary.ContainsKey(Category))
                {
                    foreach (IUpdateSystem upSys in updateSystemDictionary[Category].Values)
                    {
                        upSys.Update(gameTime);
                    }
                }
            }
        }
    }
}
