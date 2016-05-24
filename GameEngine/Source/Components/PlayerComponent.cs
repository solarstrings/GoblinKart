using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Source.Components
{
    public class PlayerComponent : IComponent
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
