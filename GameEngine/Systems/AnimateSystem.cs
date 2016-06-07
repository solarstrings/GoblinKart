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
    public class AnimateSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();
            List<AnimationComponent> animComponents =
                ComponentManager.Instance.GetComponentsFromEntities<AnimationComponent>(entities);

            foreach (AnimationComponent anim in animComponents)
            {

                if (anim.CurrentAnimation == null)
                    continue;

                anim.CurrentElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

                if (anim.CurrentElapsedTime > anim.TimePerFrame)
                {
                    anim.CurrentElapsedTime = 0;
                    anim.CurrentFrame++;
                    int t = anim.GetAnimLength();
                    if (anim.CurrentFrame >= anim.GetAnimLength())
                    {
                        anim.CurrentFrame = 0;
                    }
                    anim.SetSourceRect(anim.GetCurrentFrame());
                }
            }
        }
    }
}
