using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Models
{
    public abstract class Projectile
    {
        public Vector2 Velocity { get; set; }
        public Vector2 Force { get; set; }
        public Vector4 Rotation { get; set; }
    }
}
