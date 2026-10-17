using System;
using System.Collections.Generic;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitions
{
    public class CatapultDefinition : BaseDefinition
    {
        public String TemplateReference { get; set; }
        public GameFrame.Core.Math.Range PossibleFiringStrength { get; set; }
        public GameFrame.Core.Math.Range PossibleFiringAngle { get; set; }
        public List<ProjectileDefinition> AvailableProjectiles { get; set; }
    }
}
