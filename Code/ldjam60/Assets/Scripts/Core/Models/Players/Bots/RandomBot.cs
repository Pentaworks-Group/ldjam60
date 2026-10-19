using System.Linq;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core.Models.Players.Bots
{
    public class RandomBot : BotPlayer
    {
        public override void SelectTarget()
        {
             this.Target = Base.Core.Game.State.Players.Where(p => p != this && !p.IsDestroyed).ToList().GetRandomEntry();
        }

        public override void CalculateAngleAndPower(out System.Single targetAngle, out System.Single targetPower)
        {
            targetAngle = UnityEngine.Random.Range(this.Catapult.FiringArc.Min.Value, this.Catapult.FiringArc.Max.Value);
            targetPower = UnityEngine.Random.Range(this.Catapult.PossibleFiringPower.Min.Value, this.Catapult.PossibleFiringPower.Max.Value);
        }
    }
}
