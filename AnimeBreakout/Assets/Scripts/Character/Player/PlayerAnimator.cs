using UnityEngine;

namespace Game.Character.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        void Start()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
        }

        public void PlayAnimation(string anim, int layer)
        {
            _animator.Play(anim, layer, 0f);
        }

        public void SetBool(string name, bool value)
        {
            _animator.SetBool(name, value);
        }
    }
}