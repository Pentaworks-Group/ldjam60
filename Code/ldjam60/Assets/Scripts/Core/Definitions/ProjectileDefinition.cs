using System;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitions
{
    public class ProjectileDefinition : BaseDefinition
    {
        public String Name { get; set; }
        public Double? Cost { get; set; }
        public Double? Weight { get; set; }
        public String TemplateReference { get; set; }
    }
}
