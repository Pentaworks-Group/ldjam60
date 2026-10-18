using System;

namespace Assets.Scripts.Core.Models.Players
{
    public abstract class BotPlayer : Player
    {
        public override Boolean IsHuman => false;

        public Player LastTarget { get; set; }
    }
}
