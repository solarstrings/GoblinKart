using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Interfaces;

namespace GameEngine.Components
{
    public class TagComponent : IComponent
    {
        public string tagName;
        
        /// <summary>
        /// Constructor for a tag
        /// Tags are used to categorize an entity. This is used in collision handling.
        /// </summary>
        /// <param name="tagName">the name of the tag</param>
        public TagComponent(string tagName)
        {
            this.tagName = tagName;
        }
    }
}
