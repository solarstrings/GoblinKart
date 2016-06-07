using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Components;
using GameEngine.Engine;
using GameEngine.Interfaces;
using GameEngine.Managers;
using Microsoft.Xna.Framework;

namespace GameEngine.Systems
{
    public class TimerSystem : IUpdateSystem
    {
        /// <summary>
        /// Handles timers, update time and timer status
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            List<Entity> sceneEntities = SceneManager.Instance.GetActiveScene().GetAllEntities();

            if (sceneEntities == null)
            {
                //ErrorLogger.Instance.LogErrorToDisk("TimerSystem - no entities with TimerComponent", "TimerSystemLog.Txt");
                return;
            }
            foreach (Entity e in sceneEntities)
            {
                TimerComponent t = ComponentManager.Instance.GetEntityComponent<TimerComponent>(e);
                if (t != null)
                {
                    if (e.Updateable)
                    {
                        TimerComponent timer = ComponentManager.Instance.GetEntityComponent<TimerComponent>(e);
                        timer.CurrentTime += gameTime.ElapsedGameTime.TotalSeconds;
                        if (timer.CurrentTime > timer.TargetTime)
                            timer.TimerDone = true;
                    }
                }
            }
        }
    }
}
