using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    public class ParentComponent : IComponent
    {
        public Entity Parent { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }

        public ParentComponent(Entity parentEntity, float offsetX, float offsetY)
        {
            Parent = parentEntity;
            OffsetX = offsetX;
            OffsetY = OffsetY;
        }
    }
}
