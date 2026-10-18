using Assets.Scripts.Constants;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Models.Players;

using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game
{
    public class HumanPlayerBehaviour : PlayerBehaviour
    {
        private HumanPlayer humanPlayer;
        
        private InputAction fireAction;
        private InputAction aimUpAction;
        private InputAction aimDownAction;

        public override Player Player => this.humanPlayer;

        public override void EnableInput()
        {
            base.EnableInput();

            fireAction.performed += FireAction_performed;
            aimUpAction.performed += AimUpAction_performed;
            aimDownAction.performed += AimDownAction_performed;
        }

        public override void DisableInput()
        {
            base.DisableInput();

            fireAction.performed -= FireAction_performed;
            aimUpAction.performed -= AimUpAction_performed;
            aimDownAction.performed -= AimDownAction_performed;
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
            aimUpAction = InputSystem.actions.FindAction(InputActions.AimUp);
            aimDownAction = InputSystem.actions.FindAction(InputActions.AimDown);
        }

        private void FireAction_performed(InputAction.CallbackContext obj)
        {
            ShotFired.Invoke(this);
        }

        private void AimUpAction_performed(InputAction.CallbackContext obj)
        {
            Player.Catapult.FiringAngle += 10;
            OnAngleChanged(Player.Catapult.FiringAngle);
        }

        private void AimDownAction_performed(InputAction.CallbackContext obj)
        {
            Player.Catapult.FiringAngle -= 10;
            OnAngleChanged(Player.Catapult.FiringAngle);
        }
    }
}
