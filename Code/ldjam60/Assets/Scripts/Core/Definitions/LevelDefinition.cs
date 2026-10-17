using System;
using System.Collections.Generic;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitions
{
    public class LevelDefinition : BaseDefinition
    {
        public List<String> BackgroundReferences { get; set; }
        public MapDefinition Map { get; set; } 
    }
}
