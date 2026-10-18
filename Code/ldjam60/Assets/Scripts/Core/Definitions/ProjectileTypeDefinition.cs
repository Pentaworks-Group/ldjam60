using System;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitions
{
    public class ProjectileTypeDefinition : BaseDefinition
    {
        public String Name { get; set; }
        public Boolean? IsDefault { get; set; }
        public Decimal? Cost { get; set; }
        public GameFrame.Core.Math.Range Mass { get; set; }
        public Single? Damage { get; set; }
        public String TemplateReference { get; set; }
    }
}
