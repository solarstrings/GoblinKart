using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
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
        }

        public override void Initialise()
        {
            new InitGame(this);
            new InitHouses(this);            
            SceneManager.Instance.SetActiveScene("Game");
            
            //once all models entities have been loaded, add the static ones to the chunks they stand upon.
            TerrainMapRenderSystem.AddStaticModelsToChunks();
        }
    }
}
