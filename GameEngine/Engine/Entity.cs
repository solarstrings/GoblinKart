using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.Engine
{
    public class Entity
    {
        public bool Visible { set; get; }
        public bool Updateable { set; get; }

        public Entity()
        {
            Visible = true;
            Updateable = true;
        }
    }
}
