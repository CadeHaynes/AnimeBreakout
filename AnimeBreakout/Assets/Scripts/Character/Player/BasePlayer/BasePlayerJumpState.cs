using Game.StateMachine;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Character.Player
{
    public class BasePlayerJumpState : State<PlayerStateManager>
    {
        public override void OnEnter(PlayerStateManager sm)
        {
            sm.GroundClass.SetNonGroundedVariables();

            sm.JumpClass.Jump(sm.Stats.JumpHeight);
        }

        public override void OnUpdate(PlayerStateManager sm)
        {
            sm.PlayerMove();
            EndJump(sm);            

            AirborneTransition(sm);
            StrikeTransition(sm);
        }

        public override void OnExit(PlayerStateManager sm)
        {

        }

        void EndJump(PlayerStateManager sm)
        {
            if (sm.InputManager.JumpAction.ReadValue<float>() == 0)
            {
                sm.JumpClass.EndJump();
            }
        }

        void AirborneTransition(PlayerStateManager sm)
        {
            var jumpInput = sm.InputManager.JumpAction.ReadValue<float>();

            if (jumpInput == 0 || sm.Rb.linearVelocityY <= 0)
            {
                sm.ChangeState(sm.AirState);
            }
        }

        void StrikeTransition(PlayerStateManager sm)
        {
            if (sm.StrikeClass.CheckForTargets() || sm.InputManager.StrikeAction.ReadValue<float>() != 0)
            {
                sm.ChangeState(sm.StrikeState);
            }
        }
    }
}
