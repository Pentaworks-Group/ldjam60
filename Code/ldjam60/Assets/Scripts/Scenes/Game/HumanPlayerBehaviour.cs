using Assets.Scripts.Constants;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Models.Players;

using UnityEngine.InputSystem;
using GameFrame.Core.Extensions;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class HumanPlayerBehaviour : PlayerBehaviour
    {
        private HumanPlayer humanPlayer;
        
        private InputAction fireAction;

        private InputAction aimRightAction;
        private InputAction aimLeftAction;
        private InputAction increaseStrengthAction;
        private InputAction decreaseStrengthAction;

        private Boolean resetAimAngle;
        private IEnumerator aimLeft;
        private IEnumerator aimRight;

        private IEnumerator increasePower;
        private IEnumerator decreasePower;

        public override Player Player => this.humanPlayer;

        public override void EnableInput()
        {
            base.EnableInput();

            fireAction.performed += FireAction_performed;

            aimLeftAction.Hook(onStarted: AimLeftAction_Started, onCancelled: AimLeftAction_Cancelled);
            aimRightAction.Hook(onStarted: AimRightAction_Started, onCancelled: AimRightAction_Cancelled);

            increaseStrengthAction.UnHook(onStarted: IncreasePowerAction_Started, onCancelled: IncreasePowerAction_Cancelled);
            decreaseStrengthAction.UnHook(onStarted: DecreasePowerAction_Started, onCancelled: DecreasePowerAction_Cancelled);
        }

        public override void DisableInput()
        {
            base.DisableInput();

            fireAction.performed -= FireAction_performed;

            aimLeftAction.UnHook(onStarted: AimLeftAction_Started, onCancelled: AimLeftAction_Cancelled);
            aimRightAction.UnHook(onStarted: AimRightAction_Started, onCancelled: AimRightAction_Cancelled);

            increaseStrengthAction.UnHook(onStarted: IncreasePowerAction_Started, onCancelled: IncreasePowerAction_Cancelled);
            decreaseStrengthAction.UnHook(onStarted: DecreasePowerAction_Started, onCancelled: DecreasePowerAction_Cancelled);
        }

        protected override void InnerInit(Player player)
        {
            if (player is HumanPlayer humanPlayer)
            {
                this.humanPlayer = humanPlayer;
            }
        }

        protected override void InnerAwake()
        {
            base.InnerAwake();

            fireAction = InputSystem.actions.FindAction(InputActions.Fire);

            aimLeftAction = InputSystem.actions.FindAction(InputActions.AimLeft);
            aimRightAction = InputSystem.actions.FindAction(InputActions.AimRight);

            increaseStrengthAction = InputSystem.actions.FindAction(InputActions.IncreaseStrength);
            decreaseStrengthAction = InputSystem.actions.FindAction(InputActions.DecreaseStrength);
        }

        protected override void InnerStart()
        {
            base.InnerStart();

            aimLeft = ChangeAim(-1);
            aimRight = ChangeAim(1);

            increasePower = ChangePower(1);
            decreasePower = ChangePower(-1);
        }

        protected override void InnerOnDestroy()
        {
            base.InnerOnDestroy();

            fireAction.UnHook(onPerformed: FireAction_performed);

            aimLeftAction.UnHook(onStarted: AimLeftAction_Started, onCancelled: AimLeftAction_Cancelled);
            aimRightAction.UnHook(onStarted: AimRightAction_Started, onCancelled: AimRightAction_Cancelled);

            increaseStrengthAction.UnHook(onStarted: IncreasePowerAction_Started, onCancelled: IncreasePowerAction_Cancelled);
            decreaseStrengthAction.UnHook(onStarted: DecreasePowerAction_Started, onCancelled: DecreasePowerAction_Cancelled);
        }

        private void FireAction_performed(InputAction.CallbackContext obj)
        {
            ShotFired.Invoke(this);
        }

        private void AimLeftAction_Started(InputAction.CallbackContext context)
        {
            _ = StartCoroutine(aimLeft);
        }

        private void AimLeftAction_Cancelled(InputAction.CallbackContext context)
        {
            StopCoroutine(aimLeft);
            resetAimAngle = true;
        }

        private void AimRightAction_Started(InputAction.CallbackContext context)
        {
            _ = StartCoroutine(aimRight);
        }

        private void AimRightAction_Cancelled(InputAction.CallbackContext context)
        {
            StopCoroutine(aimRight);
            resetAimAngle = true;
        }

        private void IncreasePowerAction_Started(InputAction.CallbackContext context)
        {
            _ = StartCoroutine(increasePower);
        }

        private void IncreasePowerAction_Cancelled(InputAction.CallbackContext context)
        {
            StopCoroutine(increasePower);
        }

        private void DecreasePowerAction_Started(InputAction.CallbackContext context)
        {
            _ = StartCoroutine(decreasePower);
        }

        private void DecreasePowerAction_Cancelled(InputAction.CallbackContext context)
        {
            StopCoroutine(decreasePower);
        }

        private IEnumerator ChangeAim(Single angleStep)
        {
            var actualAngle = angleStep * Time.deltaTime;

            var minValue = Player.Catapult.FiringArc.Min.Value;
            var maxValue = Player.Catapult.FiringArc.Max.Value;

            var elapsedTime = 0f;

            while (true)
            {
                if (resetAimAngle)
                {
                    actualAngle = angleStep * Time.deltaTime;
                    resetAimAngle = false;
                }

                var currentAngle = Player.Catapult.FiringAngle;

                var newAngle = currentAngle - actualAngle;

                newAngle = Mathf.Clamp(newAngle, minValue, maxValue);

                if (currentAngle != newAngle)
                {
                    Player.Catapult.FiringAngle = newAngle;
                    OnAngleChanged(newAngle);
                }

                elapsedTime += Time.deltaTime;

                if (elapsedTime > 0.5f)
                {
                    elapsedTime = 0f;
                    actualAngle *= 1.5f;
                }

                Debug.Log(actualAngle);

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator ChangePower(Single powerStep)
        {
            var strenth = Player.Catapult.FiringStrength;

            var actualPowerChange = powerStep * Time.deltaTime;

            var minValue = Player.Catapult.PossibleFiringStrength.Min.Value;
            var maxValue = Player.Catapult.PossibleFiringStrength.Max.Value;

            while (true)
            {
                strenth -= actualPowerChange;

                var newPower = Mathf.Clamp(strenth, minValue, maxValue);

                Player.Catapult.FiringStrength = newPower;
                OnPowerChanged(newPower);

                yield return new WaitForFixedUpdate();
            }
        }

        private void AimRightAction_performed(InputAction.CallbackContext obj)
        {
            var newAngle = Player.Catapult.FiringAngle + 1;

            if (newAngle < Player.Catapult.FiringArc.Max.Value)
            {
                Player.Catapult.FiringAngle = newAngle;
                OnAngleChanged(newAngle);
            }
        }

        private void AimLeftAction_performed(InputAction.CallbackContext obj)
        {
            var newAngle = Player.Catapult.FiringAngle - 1;

            if (newAngle > Player.Catapult.FiringArc.Min.Value)
            {
                Player.Catapult.FiringAngle = newAngle;
                OnAngleChanged(newAngle);
            }
        }
    }
}
