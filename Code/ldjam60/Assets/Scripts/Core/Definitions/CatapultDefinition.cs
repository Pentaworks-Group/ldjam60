using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitions
{
    public class CatapultDefinition
    {
        public String TemplateReference { get; set; }
        public GameFrame.Core.Math.Range FiringStrength { get; set; }
        public List<ProjectileDefinition> AvailableProjectiles { get; set; }
    }
}
