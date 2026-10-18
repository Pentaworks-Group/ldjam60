using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Models
{
    public class Catapult
    {
        public String TemplateReference { get; set; }
        public GameFrame.Core.Math.Range FiringArc { get; set; }
        public GameFrame.Core.Math.Range PossibleFiringPower { get; set; }
        public List<ProjectileType> AvailableProjectileTypes { get; set; } = new List<ProjectileType>();
        public ProjectileType SelectedProjectileType { get; set; }
        public Single FiringAngle { get; set; }
        public Single FiringPower { get; set; }
    }
}
