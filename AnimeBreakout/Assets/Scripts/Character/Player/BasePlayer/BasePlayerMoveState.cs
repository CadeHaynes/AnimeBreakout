using Game.StateMachine;
using UnityEngine;

namespace Game.Character.Player
{
    public class BasePlayerMoveState : State<PlayerStateManager>
    {
        // Private
        float _lastJumpInput;

        public override void OnEnter(PlayerStateManager sm)
        {
            _lastJumpInput = sm.InputManager.JumpAction.ReadValue<float>();
        }
        
        public override void OnUpdate(PlayerStateManager sm)
        {

            if (sm.LastMoveInput != 0)
            {
                sm.PlayerMove();
            }

            AirborneTransition(sm);
            IdleStateTransition(sm, sm.LastMoveInput);
            JumpStateTransition(sm);
            StrikeTransition(sm);
        }

        public override void OnExit(PlayerStateManager sm)
        {
            sm.MoveClass.Move(sm.LastMoveInput * (sm.Stats.Speed / 4));
        }

        void AirborneTransition(PlayerStateManager sm)
        {
            if (!sm.GroundClass.IsGrounded())
            {
                sm.ChangeState(sm.AirState);
            }
        }

        void IdleStateTransition(PlayerStateManager sm, float moveInput)
        {
            if (moveInput == 0)
            {
                sm.ChangeState(sm.IdleState);
            }
        }

        void JumpStateTransition(PlayerStateManager sm)
        {
            var jumpInput = sm.InputManager.JumpAction.ReadValue<float>();

            if (_lastJumpInput != 0)
            {
                _lastJumpInput = sm.InputManager.JumpAction.ReadValue<float>();
            }

            if (jumpInput != 0 && _lastJumpInput == 0)
            {
                sm.ChangeState(sm.JumpState);
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
