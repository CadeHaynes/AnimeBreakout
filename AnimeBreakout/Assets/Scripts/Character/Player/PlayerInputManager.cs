using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Character.Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        // Private
        [SerializeField] string _actionMapName;
        [SerializeField] string _moveActionName;
        [SerializeField] string _jumpActionName;
        [SerializeField] string _strikeActionName;
        [SerializeField] string _sprintActionName;

        [SerializeField] InputActionAsset _playerControls;

        InputAction _moveAction;
        InputAction _jumpAction;
        InputAction _strikeAction;
        InputAction _sprintAction;

        // Public
        public InputAction MoveAction => _moveAction;
        public InputAction JumpAction => _jumpAction;
        public InputAction StrikeAction => _strikeAction;
        public InputAction SprintAction => _sprintAction;

        void OnEnable()
        {
            _moveAction.Enable();
            _jumpAction.Enable();
            _strikeAction.Enable();
            _sprintAction.Enable();
        }

        void OnDisable()
        {
            _moveAction.Disable();
            _jumpAction.Disable();
            _strikeAction.Disable();
            _sprintAction.Disable();
        }

        private void Awake()
        {
            _moveAction = _playerControls.FindActionMap(_actionMapName).FindAction(_moveActionName);
            _jumpAction = _playerControls.FindActionMap(_actionMapName).FindAction(_jumpActionName);
            _strikeAction = _playerControls.FindActionMap(_actionMapName).FindAction(_strikeActionName);
            _sprintAction = _playerControls.FindActionMap(_actionMapName).FindAction(_sprintActionName);
        }

        void Start()
        {
            
        }

        void Update()
        {

        }
    }
}
