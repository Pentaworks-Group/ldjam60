using System;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitions
{
    public class GameMode : BaseDefinition
    {
        public Boolean? IsDefault { get; set; }
        public String Name { get; set; }
        public LevelDefinition Level { get; set; }
        public Double? PlayerStartCredits { get; set; }
    }
}
