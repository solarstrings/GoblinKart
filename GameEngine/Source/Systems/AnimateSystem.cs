using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class AnimateSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime)
        {
            List<Entity> entities = SceneManager.Instance.GetActiveScene().GetAllEntities();

            foreach (Entity enitity in entities)
            {
                AnimationComponent anim = ComponentManager.Instance.GetEntityComponent<AnimationComponent>(enitity);
                if (anim == null)
                    continue;

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
