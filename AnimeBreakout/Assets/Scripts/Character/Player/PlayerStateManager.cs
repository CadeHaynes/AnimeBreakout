using Game.Interfaces;
using Game.StateMachine;
using System.Collections;
using UnityEngine;

namespace Game.Character.Player
{
    public class PlayerStateManager : StateManager<PlayerStateManager>, IDamageable
    {
        // Public
        public Rigidbody2D Rb { get; private set; }
        public float LastMoveInput { get; private set; }

        // - Player Scripts
        public CharacterGround GroundClass { get; private set; }
        public CharacterJump JumpClass { get; private set; }
        public CharacterMovement MoveClass { get; private set; }
        public CharacterStats Stats { get; private set; }

        public PlayerStrike StrikeClass { get; private set; }
        public PlayerInputManager InputManager { get; private set; }
        public PlayerAnimator PlayerAnimator { get; private set; }

        // - States
        public State<PlayerStateManager> AirState { get; private set; }
        public State<PlayerStateManager> IdleState { get; private set; }
        public State<PlayerStateManager> JumpState { get; private set; }
        public State<PlayerStateManager> MoveState { get; private set; }
        public State<PlayerStateManager> StrikeState { get; private set; }

        // - Other
        SpriteRenderer[] _sprites;

        private void Awake()
        {
            SetStates();

            _sprites = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            Rb = GetComponent<Rigidbody2D>();

            SetClasses();

            ChangeState(IdleState);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (InputManager.MoveAction.ReadValue<Vector2>().x != 0)
            {
                LastMoveInput = InputManager.MoveAction.ReadValue<Vector2>().x;
            }
        }

        public virtual void SetStates()
        {
            AirState = ScriptableObject.CreateInstance<BasePlayerAirState>();
            IdleState = ScriptableObject.CreateInstance<BasePlayerIdleState>();
            JumpState = ScriptableObject.CreateInstance<BasePlayerJumpState>();
            MoveState = ScriptableObject.CreateInstance<BasePlayerMoveState>();
            StrikeState = ScriptableObject.CreateInstance<BasePlayerStrikeState>();
        }

        public virtual void SetClasses()
        {
            GroundClass = GetComponent<CharacterGround>();
            InputManager = GetComponent<PlayerInputManager>();
            JumpClass = GetComponent<CharacterJump>();
            MoveClass = GetComponent<CharacterMovement>();
            Stats = GetComponent<CharacterStats>();
            StrikeClass = GetComponent<PlayerStrike>();
            PlayerAnimator = GetComponent<PlayerAnimator>();
        }

        public void PlayerMove()
        {
            var moveInput = InputManager.MoveAction.ReadValue<Vector2>().x;

            var sprintInput = InputManager.SprintAction.ReadValue<float>();
            var speed = Stats.Speed;

            if (sprintInput != 0)
            {
                speed *= 2f;
            }

            MoveClass.Move(moveInput * speed);

            var isMoving = moveInput != 0;

            PlayerAnimator.SetBool("IsMoving", isMoving);

            if (LastMoveInput != moveInput && moveInput != 0)
            {
                PlayerAnimator.PlayAnimation("Run Turnaround", 1);

                foreach (SpriteRenderer sprite in _sprites)
                {
                    sprite.flipX = moveInput > 0;
                }
            }
        }

        public void TakeDamage(int damage)
        {
            Stats.ModifyHealth(-damage);

            if (Stats.Health <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(Blink());
            }
        }

        IEnumerator Blink()
        {
            var isEnabled0 = _sprites[0].enabled;
            var isEnabled1 = _sprites[1].enabled;

            // make sure that _sprites[0] and _sprites[1] are the Player's Lower and Upper body sprites
            for (int i = 0; i < 2; i++)
            {
                _sprites[0].enabled = false;
                _sprites[1].enabled = false;
                yield return new WaitForSeconds(0.05f);
                _sprites[0].enabled = isEnabled0;
                _sprites[1].enabled = isEnabled1;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
