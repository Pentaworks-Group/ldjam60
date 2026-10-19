using System;
using System.Collections;

using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Models.Players;

using UnityEngine;

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

        protected override void OnActivate()
        {
            _ = StartCoroutine(DoTurn());

            base.OnActivate();
        }

        protected override void OnDeactivate()
        {
            StopAllCoroutines();

            base.OnDeactivate();
        }

        protected IEnumerator DoTurn()
        {
            yield return SelectTarget();

            yield return AimAtTarget();

            yield return Fire();

            yield return null;
        }

        protected virtual IEnumerator SelectTarget()
        {
            this.botPlayer.SelectTarget();

            yield return null;
        }

        protected IEnumerator Lerp(Single startValue, Single targetValue, Single duration, Action<Single> onUpdate)
        {
            var elapsedTime = 0f;

            var currentValue = startValue;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / duration;

                currentValue = Mathf.Lerp(currentValue, targetValue, percentageComplete);

                onUpdate(currentValue);

                yield return null;
            }
        }

        protected virtual IEnumerator AimAtTarget()
        {
            this.botPlayer.CalculateAngleAndPower(out var targetAngle, out var targetPower);

            var currentAngle = this.botPlayer.Catapult.FiringAngle;

            if (!Mathf.Approximately(currentAngle, targetAngle))
            {
                yield return Lerp(currentAngle, targetAngle, 2f, (newValue) =>
                {
                    this.botPlayer.Catapult.FiringAngle = newValue;
                    OnAngleChanged(newValue);
                });
            }

            var currentPower = this.botPlayer.Catapult.FiringPower;

            if (!Mathf.Approximately(currentPower, targetPower))
            {
                yield return Lerp(currentPower, targetPower, 2f, (newValue) =>
                {
                    this.botPlayer.Catapult.FiringPower = newValue;
                    OnPowerChanged(newValue);
                });
            }

            yield return null;
        }

        protected virtual IEnumerator Fire()
        {
            ShotFired?.Invoke(this);
            yield return null;
        }
    }
}
