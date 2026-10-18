using System;
using System.Collections.Generic;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitions
{
    public class CatapultDefinition : BaseDefinition
    {
        public String TemplateReference { get; set; }
        public GameFrame.Core.Math.Range PossibleFiringPower { get; set; }
        public GameFrame.Core.Math.Range FiringArc { get; set; }
        public List<ProjectileTypeDefinition> AvailableProjectileTypes { get; set; }
    }
}
