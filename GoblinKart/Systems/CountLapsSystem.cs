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
using GoblinKart.Utilities;
using Microsoft.Xna.Framework;

namespace GoblinKart.Systems
{
    public class CountLapsSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            var playerEntities = ComponentManager.Instance.GetAllEntitiesWithComponentType<PlayerComponent>();
            var gameSettings = ComponentManager.Instance.GetAllComponentsOfType<GameSettingsComponent>()[0];

            foreach (var e in playerEntities)
            {
                var transformC = ComponentManager.Instance.GetEntityComponent<TransformComponent>(e);
                var lapC = ComponentManager.Instance.GetEntityComponent<LapComponent>(e);
                
                var distance = Vector2.Distance(AiHelper.V3ToV2(transformC.Position), gameSettings.Waypoints[lapC.CurrentWaypoint].WaypointPosition);
                if (distance <= gameSettings.Waypoints[lapC.CurrentWaypoint].Radius)
                {
                    Debug.WriteLine("Player reached waypoint: " + lapC.CurrentWaypoint);
                    lapC.CurrentWaypoint++;
                }
                if (lapC.CurrentWaypoint == gameSettings.Waypoints.Count)
                {
                    lapC.Laps++;
                    lapC.CurrentWaypoint = 0;
                }
            }
        }
    }
}
