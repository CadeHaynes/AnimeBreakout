using Game.StateMachine;
using UnityEngine;
using UnityEngine.Windows;

namespace Game.Character.Player
{
    public class BasePlayerAirState : State<PlayerStateManager>
    {
        bool _hasJumped;

        public override void OnEnter(PlayerStateManager sm)
        {
            sm.GroundClass.SetNonGroundedVariables();

            if (sm.PreviousState == (sm.JumpState || sm.StrikeState))
            {
                _hasJumped = true;
            }
            else _hasJumped = false;
        }

        public override void OnUpdate(PlayerStateManager sm)
        {
            sm.PlayerMove();
            EndJump(sm);

            IdleTransition(sm);
            StrikeTransition(sm);
        }

        public override void OnExit(PlayerStateManager sm)
        {
            
        }

        void EndJump(PlayerStateManager sm)
        {
            if (sm.InputManager.JumpAction.ReadValue<float>() == 0 && sm.Rb.linearVelocityY >= 0 && _hasJumped)
            {
                sm.JumpClass.EndJump();
            }
        }

        void IdleTransition(PlayerStateManager sm)
        {
            if (sm.GroundClass.IsGrounded())
            {
                sm.GroundClass.SetGroundedVariables();

                sm.ChangeState(sm.IdleState);
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
