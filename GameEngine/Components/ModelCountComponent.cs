using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Interfaces;

namespace GameEngine.Components
{
    public class ModelCountComponent : IComponent
    {
        public int numModelsInView { get; set; }
    }
}
