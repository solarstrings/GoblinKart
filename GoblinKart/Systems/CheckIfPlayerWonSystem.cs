using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                var localPlayerComp = ComponentManager.Instance.GetEntityComponent<LocalPlayerComponent>(e);

                if (playerLapComp.Laps >= gameSettings.NrOfLaps)
                {
                    if (localPlayerComp == null)
                    {
                        Debug.WriteLine("Player lost!");
                        SceneManager.Instance.SetActiveScene("LooseScreen");
                        SystemManager.Instance.Category = "LooseScreen";
                    }
                    else
                    {
                        Debug.WriteLine("Player Won!");
                        SceneManager.Instance.SetActiveScene("WinScreen");
                        SystemManager.Instance.Category = "WinScreen";
                    }
                }
            }
        }
    }
}
                    