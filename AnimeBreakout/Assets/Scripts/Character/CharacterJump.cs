using UnityEngine;

namespace Game.Character
{
    public class CharacterJump : MonoBehaviour
    {
        // Private
        Rigidbody2D _rb;

        void Start()
        {
            GetRigidBody();
        }

        public void Jump(float y)
        {
            _rb.linearVelocityY = y * Mathf.Sqrt(-(Physics2D.gravity.y * _rb.gravityScale));
        }

        public void EndJump()
        {
            _rb.linearVelocityY = 0;
        }

        void GetRigidBody()
        {
            if (_rb == null)
            {
                _rb = gameObject.GetComponent<Rigidbody2D>();
            }
        }
    }
}