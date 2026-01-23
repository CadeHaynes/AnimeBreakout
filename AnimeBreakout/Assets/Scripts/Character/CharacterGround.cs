using UnityEngine;

namespace Game.Character
{
    public class CharacterGround : MonoBehaviour
    {
        // Private
        [SerializeField] LayerMask _groundLayer;

        [SerializeField] int _airborneGravityScale = 5;

        [SerializeField] PhysicsMaterial2D _baseMaterial;
        [SerializeField] PhysicsMaterial2D _airborneMaterial;

        Rigidbody2D _rb;

        public bool IsGrounded()
        {
            bool isGrounded = Physics2D.BoxCast(transform.position, transform.localScale, 0f, Vector2.down, 0.2f, _groundLayer);

            return isGrounded;
        }

        public void SetGroundedVariables()
        {
            if (_rb == null)
            {
                _rb = GetComponent<Rigidbody2D>();
            }

            _rb.gravityScale = 1;
            _rb.sharedMaterial = _baseMaterial;
        }

        public void SetNonGroundedVariables()
        {
            if (_rb == null)
            {
                _rb = GetComponent<Rigidbody2D>();
            }

            _rb.gravityScale = _airborneGravityScale;
            _rb.sharedMaterial = _airborneMaterial;
        }
    }
}