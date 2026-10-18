using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Models.Players;

namespace Assets.Scripts.Scenes.Game
{
    public class BotPlayerBehaviour : PlayerBehaviour
    {
        private BotPlayer botPlayer;

        public override Player Player => this.botPlayer;

        protected override void InnerInit(Player player)
        {
            if (player is BotPlayer botPlayer)
            {
                this.botPlayer = botPlayer;
            }
        }
    }
}
