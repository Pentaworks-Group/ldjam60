namespace Assets.Scripts.Core.Models.Players
{
    public abstract class BotPlayer : Player
    {
        public Player LastTarget { get; set; }
    }
}
