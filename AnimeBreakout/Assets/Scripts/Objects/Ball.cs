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
        BallManager _bm;

        bool _isStruck = false;
        float _currentSpeed;
        int _damage = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentSpeed = _startSpeed;

            if (!_bm)
            {
                ActivateBall();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isStruck) _rb.gravityScale = _initialGravityScale;
            else _rb.gravityScale = 0f;

            if (_isStruck && !_struckBy.activeSelf)
            {
                _isStruck = false;
            }
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

        public void ActivateBall(BallManager bm)
        {
            if (!_bm) _bm = bm;

            gameObject.SetActive(true);
        }

        public void ActivateBall()
        {
            if (!_bm)
            {
                var bm = FindFirstObjectByType<BallManager>();

                _bm = bm;

                _bm.AddExistingBall(this);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isStruck)
            {
                if (collision.gameObject.tag == "Ground")
                {
                    if (_bm && _bm.BallCount > 1)
                    {
                        _bm.DeactivateBall(this);
                    }
                    else if (_bm.BallCount <= 1)
                    {
                        TryToDamage(_struckBy);
                    }
                }

                if (collision.gameObject.tag == "Block")
                {
                    TryToDamage(collision.gameObject);
                }
            }
        }
    }
}