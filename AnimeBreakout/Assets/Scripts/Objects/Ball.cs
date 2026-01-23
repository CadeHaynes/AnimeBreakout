using UnityEngine;
using Game.Interfaces;

namespace Game.Objects.Ball
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] float _startSpeed = 8f;
        [SerializeField] float _speedStep = 0.5f;
        [SerializeField] float _initialGravityScale = 2.5f;

        Rigidbody2D _rb;
        GameObject _struckBy;

        bool _isStruck = false;
        float _currentSpeed;
        int _damage = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentSpeed = _startSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isStruck) _rb.gravityScale = _initialGravityScale;
            else _rb.gravityScale = 0f;
        }

        public void Strike(float angle, GameObject striker)
        {
            if (!_isStruck) _isStruck = true;

            _struckBy = striker;

            _currentSpeed += _speedStep;
            _rb.linearVelocity = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) * _currentSpeed;
        }

        public Vector2 GetBallVelocity()
        {
            return _rb.linearVelocity;
        }

        void TryToDamage(GameObject obj)
        {
            if (obj.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isStruck)
            {
                if (collision.gameObject.tag == "Ground")
                {
                    TryToDamage(_struckBy);
                }

                if (collision.gameObject.tag == "Block")
                {
                    TryToDamage(collision.gameObject);
                }
            }
        }
    }
}