using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Components;
using GameEngine.Interfaces;
using GameEngine.Managers;
using GoblinKart.Components;
using Microsoft.Xna.Framework;

namespace GoblinKart.Systems
{
    public class CheckIfPlayerWonSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            var playerEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<PlayerComponent>();
            var gameSettings = ComponentManager.Instance.GetAllComponentsOfType<GameSettingsComponent>()[0];
                
            foreach (var e in playerEntities)
            {
                var playerLapComp = ComponentManager.Instance.GetEntityComponent<LapComponent>(e);
                if (playerLapComp.Lap >= gameSettings.NrOfLaps)
                {
                    // A player has won
                    // Handle it! Exit the game?
                }
            }
        }
    }
}
