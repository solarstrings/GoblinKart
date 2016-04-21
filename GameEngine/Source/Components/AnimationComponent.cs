using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class AnimationComponent : IComponent
    {

        public double TimePerFrame { set; get; }
        public double CurrentElapsedTime { set; get; }
        public bool visible {get;set;}

        private string currentAnimation;
        public string CurrentAnimation 
        {
            get
            {
                return currentAnimation;
            }
            set
            {
                if (animations.ContainsKey(value))
                {
                    currentAnimation = value;
                    CurrentFrame = 0;
                    visible = true;
                }
            }
        }
        
        private Dictionary<string, List<int>> animations = new Dictionary<string, List<int>>();

        public int CurrentFrame { get; set; }

        public int MaxXFrames;
        public int MaxYFrames;

        public Rectangle SourceRect = new Rectangle();

        public AnimationComponent(double timePerFrame, int animeationRectWidth, int animeationRectHeight, int textureWidth, int textureHeight)
        {
            TimePerFrame = timePerFrame;
            SourceRect.Width = animeationRectWidth;
            SourceRect.Height = animeationRectHeight;
            MaxXFrames = textureWidth / animeationRectWidth;
            MaxYFrames = textureHeight / animeationRectHeight;
        }

        public void AddFrameToAnimation(string name, int frame)
        {
            if (!animations.ContainsKey(name))
            {
                animations[name] = new List<int>();
            }
            animations[name].Add(frame);
        }

        public void AddFramesToAnimation(string name, int[] frames)
        {
            if (!animations.ContainsKey(name))
            {
                animations[name] = new List<int>();
            }
            for (int i = 0; i < frames.Length; ++i)
            {
                animations[name].Add(frames[i]);
            }
        }

        public void RemoveAnimation(string name)
        {
            if(animations.ContainsKey(name))
            {
                animations[name].Clear();
            }
        }

        public int GetCurrentFrame()
        {
            if (animations.ContainsKey(currentAnimation))
            {
                if (CurrentFrame <= animations[currentAnimation].Count())
                {
                    return animations[currentAnimation][CurrentFrame];
                }
            }
            return 0;
        }

        public void SetSourceRect(int frame)
        {
            int Y = frame / MaxXFrames;
            int X = frame - (Y * MaxXFrames);
            SourceRect.X = X * SourceRect.Width;
            SourceRect.Y = Y * SourceRect.Height;
        }

        public int GetAnimLength()
        {
            if (animations.ContainsKey(currentAnimation))
                return animations[CurrentAnimation].Count;
            else return 0;
        }

        public Rectangle GetAnimationRectangle()
        {
            return SourceRect;
        }

        public void SetAnimation(string animation)
        {
            if (animations.ContainsKey(animation))
            {
                if (currentAnimation != animation)
                {
                    currentAnimation = animation;
                    CurrentFrame = 0;
                    CurrentElapsedTime = 0;
                    SetSourceRect(GetCurrentFrame());
                }
            }
        }

    }
}
