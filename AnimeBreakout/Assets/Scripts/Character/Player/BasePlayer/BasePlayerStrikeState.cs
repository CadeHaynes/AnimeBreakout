using Game.StateMachine;
using UnityEngine;

namespace Game.Character.Player
{
    public class BasePlayerStrikeState : State<PlayerStateManager>
    {
        // Private
        bool _hasJumped;

        public override void OnEnter(PlayerStateManager sm)
        {
            _hasJumped = false;

            HandleStrike(sm);

            if (sm.InputManager.JumpAction.ReadValue<float>() != 0)
            {
                _hasJumped = true;
            }
        }

        public override void OnUpdate(PlayerStateManager sm)
        {
            HandleStrike(sm);

            sm.PlayerMove();
            Jump(sm);

            CheckGround(sm);

            IdleTransition(sm);
            MoveTransition(sm);
            AirborneTransition(sm);
        }

        public override void OnExit(PlayerStateManager sm)
        {
            
        }

        void Jump(PlayerStateManager sm)
        {
            var jumpInput = sm.InputManager.JumpAction.ReadValue<float>();

            if (jumpInput != 0)
            {
                if (sm.GroundClass.IsGrounded() && !_hasJumped)
                {
                    sm.GroundClass.SetNonGroundedVariables();
                    sm.JumpClass.Jump(sm.Stats.JumpHeight);
                    _hasJumped = true;
                }
            }
            else
            {
                if (_hasJumped)
                {
                    sm.JumpClass.EndJump();
                    _hasJumped = false;
                }
            }
        }

        void HandleStrike(PlayerStateManager sm)
        {
            var strikeInput = sm.InputManager.StrikeAction.ReadValue<float>();
            var sprintInput = sm.InputManager.SprintAction.ReadValue<float>();
            var moveInput = sm.InputManager.MoveAction.ReadValue<Vector2>().x;
            
            var strikeAngle = 0f;

            if (moveInput != 0) strikeAngle += 30f;
            else strikeAngle += 15f;

            if (sprintInput != 0 && moveInput != 0) strikeAngle += 15f;

            if (!sm.GroundClass.IsGrounded()) strikeAngle *= 0.5f;

            strikeAngle *= sm.LastMoveInput;

            if (sm.InputManager.StrikeAction.ReadValue<float>() != 0)
            {
                // Manual
                sm.StrikeClass.Strike(strikeAngle);
            }
            else
            {
                // Auto
                sm.StrikeClass.Strike(strikeAngle, true);
            }
        }

        void CheckGround(PlayerStateManager sm)
        {
            if (sm.GroundClass.IsGrounded())
            {
                sm.GroundClass.SetGroundedVariables();
            }
            else
            {
                sm.GroundClass.SetNonGroundedVariables();
            }
        }

        void AirborneTransition(PlayerStateManager sm)
        {
            if (!sm.GroundClass.IsGrounded() && !sm.StrikeClass.CheckForTargets())
            {
                sm.ChangeState(sm.AirState);
            }
        }

        void IdleTransition(PlayerStateManager sm)
        {
            if (!sm.StrikeClass.CheckForTargets())
            {
                sm.ChangeState(sm.IdleState);
            }
        }

        void MoveTransition(PlayerStateManager sm)
        {
            if (!sm.StrikeClass.CheckForTargets() && sm.InputManager.MoveAction.ReadValue<Vector2>().x != 0)
            {
                sm.ChangeState(sm.MoveState);
            }
        }
    }
}
