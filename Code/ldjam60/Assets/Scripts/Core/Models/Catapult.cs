using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Core.Definitions;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Models
{
    public class Catapult
    {
        public CatapultDefinition Definition { get; set; }
        public Double FiringAngle { get; set; }
        public Double FiringStrength { get; set; }
    }
}
