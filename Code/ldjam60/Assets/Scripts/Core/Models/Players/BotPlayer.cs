using System;

namespace Assets.Scripts.Core.Models.Players
{
    public abstract class BotPlayer : Player
    {
        public override Boolean IsHuman => false;

        public Player Target { get; set; }

        public abstract void SelectTarget();
        public abstract void CalculateValues(out Single targetAngle, out Single targetPower);
    }
}
