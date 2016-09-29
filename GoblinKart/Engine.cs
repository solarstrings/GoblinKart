using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Managers;
using GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GoblinKart.Init;

namespace GoblinKart
{
    class Engine : ECSEngine
    {
        public override void InitialiseContent()
        {
            Debug.WriteLine(Quaternion.Identity);
            GraphicsDeviceManager gdm = this.GetGraphicsDeviceManager();
            gdm.PreferredBackBufferWidth = 1280;
            gdm.PreferredBackBufferHeight = 720;
            gdm.ApplyChanges();
        }

        public override void Initialise()
        {
            new InitMainMenu(this);
            new InitMultiplayerMenu(this);
            new InitGame(this);
            new InitHouses(this);
            new InitWinScreen(this);
            new InitYouLoose(this);  
           
            //once all models entities have been loaded, add the static ones to the chunks they stand upon.
            //added static models will now rendered by the terrain map render system.
            TerrainMapRenderSystem.AddStaticModelsToChunks();
            SceneManager.Instance.SetActiveScene("LooseScreen");
            SystemManager.Instance.Category = "LooseScreen";
        }
    }
}
