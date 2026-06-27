using Unity.VisualScripting;
using UnityEngine;

namespace Game.Character
{
    public class CharacterGround : MonoBehaviour
    {
        // Private
        [SerializeField] LayerMask _groundLayers;

        [SerializeField] int _airborneGravityScale = 5;

        [SerializeField] float _xBoundary = 0.1f;

        [SerializeField] PhysicsMaterial2D _baseMaterial;
        [SerializeField] PhysicsMaterial2D _airborneMaterial;

        Rigidbody2D _rb;

        public bool IsGrounded()
        {
            if (!_rb) return false;

            Vector2 boundScale = new Vector2(transform.localScale.x - _xBoundary, transform.localScale.y);

            bool isGrounded = Physics2D.BoxCast(transform.position, boundScale, 0f, Vector2.down, 0.2f, _groundLayers);

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