using UnityEngine;

namespace Game.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        // Private
        Rigidbody2D _rb;

        void Start()
        {
            GetRigidBody();
        }

        public void Move(float x)
        {
            _rb.linearVelocityX = x;
        }

        public void Move(Vector2 v)
        {
            _rb.linearVelocity = v;
        }

        //public void Flip()
        //{
        //    var newScale = new Vector3(-_rb.transform.localScale.x, _rb.transform.localScale.y, _rb.transform.localScale.z);

        //    _rb.transform.localScale = newScale;
        //}

        void GetRigidBody()
        {
            if (_rb == null)
            {
                _rb = gameObject.GetComponent<Rigidbody2D>();
            }
        }
    }
}
