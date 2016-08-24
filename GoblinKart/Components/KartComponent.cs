using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Interfaces;

namespace GoblinKart.Components
{
    public class KartComponent : IComponent
    {
        public float KartGroundOffset { get; set; } = 1.7f;
    }
}
