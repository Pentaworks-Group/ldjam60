using System;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Models
{
    public abstract class Player
    {
        public abstract Boolean IsHuman { get; }
        private Boolean isActive;
        public Boolean IsActive { get => isActive; }

        public Guid ID { get; set; }
        public String Name { get; set; }
        public Vector2 Position { get; set; }
        public Catapult Catapult { get; set; }
        public Decimal Credits { get; set; }
        public Single Health { get; set; }
        public Boolean IsDestroyed { get; set; }
    }
}
