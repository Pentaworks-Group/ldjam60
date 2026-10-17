using System;

namespace Assets.Scripts.Core.Models
{
    public class ProjectileType
    {
        public String Name { get; set; }
        public Boolean IsDefault { get; set; }
        public Decimal Cost { get; set; }
        public Single Mass { get; set; }
        public Double Damage { get; set; }
        public String TemplateReference { get; set; }
    }
}
