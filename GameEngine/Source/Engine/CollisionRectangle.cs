using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class CollisionRectangle : CollisionArea
    {
        Rectangle source;

        public CollisionRectangle()
        {
            source = new Rectangle();
        }

        public CollisionRectangle(Rectangle collisionRect)
        {
            source = collisionRect;
        }

        public bool Intersect(CollisionArea target)
        {
            return source.Intersects(((CollisionRectangle)target).source);
        }

        public bool IntersectPixel(CollisionArea target, Texture2D targetTex, Texture2D sourceTex)
        {
            Color[] sourceTexColor = new Color[sourceTex.Width * sourceTex.Height]; 
            Color[] targetTexColor = new Color[targetTex.Width * targetTex.Height];
            sourceTex.GetData<Color>(sourceTexColor);
            targetTex.GetData<Color>(targetTexColor);

            Rectangle targetRect = ((CollisionRectangle)target).source;
            Rectangle its = Rectangle.Intersect(source, targetRect);
            for (int y = its.Top; y < its.Bottom; ++y)
            {
                for (int x = its.Left; x < its.Right; ++x)
                {
                    Color colorSource = sourceTexColor[(x - source.Left) + (y - source.Top) * source.Width];
                    Color colorTarget = targetTexColor[(x - targetRect.Left) + (y - targetRect.Top) * targetRect.Width];
                    if (colorSource.A != 0 && colorTarget.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void SetX(float x)
        {
            source.X = (int)x;
        }
        public void SetY(float y)
        {
            source.Y = (int)y;
        }

    }
}
