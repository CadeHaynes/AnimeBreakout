using Game.StateMachine;
using UnityEngine;

namespace Game.Character.Player
{
    public class BasePlayerIdleState : State<PlayerStateManager>
    {
        // Private
        float _lastJumpInput;

        public override void OnEnter(PlayerStateManager sm)
        {
            _lastJumpInput = sm.InputManager.JumpAction.ReadValue<float>();
        }

        public override void OnUpdate(PlayerStateManager sm)
        {
            AirborneTransition(sm);
            JumpStateTransition(sm);
            MoveStateTransition(sm);
            StrikeTransition(sm);
        }

        public override void OnExit(PlayerStateManager sm)
        {

        }

        void AirborneTransition(PlayerStateManager sm)
        {
            if (!sm.GroundClass.IsGrounded())
            {
                sm.ChangeState(sm.AirState);
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

        void MoveStateTransition(PlayerStateManager sm)
        {
            var moveInput = sm.InputManager.MoveAction.ReadValue<Vector2>().x;

            if (moveInput != 0)
            {
                sm.ChangeState(sm.MoveState);
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
