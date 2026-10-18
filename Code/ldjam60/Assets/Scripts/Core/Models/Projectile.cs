using System;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Models
{
    public class Projectile
    {
        public ProjectileType Type { get; set; }
        public Vector3 Position { get; set; }
        public Vector3? Force { get; set; }
        public Vector4? Rotation { get; set; }
        public Player Source { get; set; }
    }
}
