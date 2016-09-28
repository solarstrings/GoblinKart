using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace GoblinKart.Systems
{
    public class CountLapsSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            //TODO
            // Update based on tiles or waypoints?
            
            // Loop all players
            // If player has reach 1 lap, add +1 to the players lapComponent
        }
    }
}
