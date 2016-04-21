using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public interface CollisionArea
    {

        bool Intersect(CollisionArea target);

        bool IntersectPixel(CollisionArea target, Texture2D TargetTex, Texture2D sourceTex);

        void SetX(float x);

        void SetY(float y);
    }
}
