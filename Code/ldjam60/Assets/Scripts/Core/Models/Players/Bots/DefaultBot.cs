using System;
using System.Collections.Generic;
using System.Linq;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core.Models.Players.Bots
{
    public class DefaultBot : BotPlayer
    {
        public override void SelectTarget()
        {
            if ((this.Target == default) || (this.Target.IsDestroyed))
            {
                var target = Base.Core.Game.State.Players.Where(p => p != this && !p.IsDestroyed).ToList().GetRandomEntry();

                this.Target = target;
            }
        }

        public override void CalculateAngleAndPower(out System.Single targetAngle, out System.Single targetPower)
        {
            var targetPosition = this.Target.Position;

            var angleRangeMax = 15f;

            if (this.Position.X > targetPosition.X)
            {
                angleRangeMax *= -1;
            }

            targetAngle = UnityEngine.Random.Range(0, angleRangeMax);
            targetPower = UnityEngine.Random.Range(this.Catapult.PossibleFiringPower.Min.Value, this.Catapult.PossibleFiringPower.Max.Value);
        }
    }
}
