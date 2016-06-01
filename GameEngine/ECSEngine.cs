using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public abstract class ECSEngine
    {
        private Game game = null;
        private GraphicsDeviceManager gdm = null;

        public abstract void Initialise();
        public abstract void InitialiseContent();

        public T LoadContent<T>(string name)
        {
            return game.Content.Load<T>(name);
        }

        public void StartEngine()
        {
            if (game == null)
            {
                using (game = new Game1(this, out gdm))
                {  
                    game.Run();
                }
            }            
        }

        public void SetWindowTitle(string title)
        {
            game.Window.Title = title;
        }

        /// <summary>
        /// This is not thread safe. If you are running update in separate thread,
        /// use: SystemManager.Instance.exitGame = true Instead.
        /// </summary>
        public void StopEngine()
        {            
            if (game != null)
            {
                game.Exit();
            }
        }

        public GraphicsDevice GetGraphicsDevice()
        {
            return game.GraphicsDevice;
        }

        public GraphicsDeviceManager GetGraphicsDeviceManager()
        {
            return gdm;
        }

    }
}
