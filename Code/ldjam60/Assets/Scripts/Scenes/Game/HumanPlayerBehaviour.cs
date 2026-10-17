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

        public override Player Player => this.humanPlayer;

        public override void EnableInput()
        {
            base.EnableInput();

            fireAction.performed += FireAction_performed;
        }

        public override void DisableInput()
        {
            base.DisableInput();

            fireAction.performed -= FireAction_performed;
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
        }

        private void FireAction_performed(InputAction.CallbackContext obj)
        {
            ShotFired.Invoke(this);
        }
    }
}
