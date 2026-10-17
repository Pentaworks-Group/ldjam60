using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Models
{
    public class Level
    {
        public String BackgroundReference { get; set; }
        public List<Player> Players { get; set; }
        public List<Projectile> Projectiles { get; set; }
    }
}
