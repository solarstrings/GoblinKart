using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameEngine.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Managers
{
    /// <summary>
    /// Thread safe singleton without using locks
    /// See link: "http://csharpindepth.com/Articles/General/Singleton.aspx#nested-cctor"/// 
    /// </summary>
    public sealed class SystemManager
    {
        private bool updateThreadStarted = false;

        public bool exitGame = false;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static SystemManager() { }
        private SystemManager(){ }

        public static SystemManager Instance { get; } = new SystemManager();

        public string Category { set; get; }
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
            if (renderSystemDictionary.Count > 0)
            {
                if (renderSystemDictionary.ContainsKey(Category))
                {
                    spriteBatch.Begin();
                    foreach (IRenderSystem renSys in renderSystemDictionary[Category].Values)
                    {
                        renSys.Render(spriteBatch, gameTime);
                    }
                    spriteBatch.End();
                }
            }
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

        }

        /// <summary>
        /// Create a thread and run this method inside it
        /// </summary>
        /// <param name="gameTime"></param>
        internal void RunAllUpdateSystems(GameTime gameTime)
        {
            Stopwatch stopWatch = new Stopwatch();

            while (true)
            {
                stopWatch.Start();

                //1 seconds = 1000 milliseconds.
                //we want update to go fairly fast, 160 times / second.
                //
                if (stopWatch.Elapsed.Milliseconds > 16.66)
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
                    stopWatch.Reset();
                }
            }
        }
    }
}
