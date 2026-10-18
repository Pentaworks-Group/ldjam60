using System;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Models
{
    public abstract class Player
    {
        public Guid ID { get; set; }
        public String Name { get; set; }
        public Vector2 Position { get; set; }
        public Catapult Catapult { get; set; }
        public Decimal Credits { get; set; }
        public Double Health { get; set; }
        public Boolean IsDestroyed { get; set; }
    }
}
