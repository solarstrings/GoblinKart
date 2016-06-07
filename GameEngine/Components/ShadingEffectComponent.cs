using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameEngine.Components
{
    /// <summary>
    /// ShadingEffectComponent
    /// This component holds a shading effect
    /// </summary>
    class ShadingEffectComponent : IComponent
    {
        Effect effect { get; set; }

        public ShadingEffectComponent(Effect effect)
        {
            this.effect = effect;
        }
    }
}
