using System;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitions
{
    public class LevelDefinition : BaseDefinition
    {
        public String Name { get; set; }
        public String BackgroundReference { get; set; }
        public String Map { get; set; } // Placeholder - this should contain more information
        public Double PlayerStartCredits { get; set; }
    }
}
