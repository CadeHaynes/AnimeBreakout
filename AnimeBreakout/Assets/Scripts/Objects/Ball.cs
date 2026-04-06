using UnityEngine;
using Game.Interfaces;
using UnityEngine.Rendering;
using System.Collections;

namespace Game.Objects.Ball
{
    public class Ball : MonoBehaviour
    {      
        Rigidbody2D _rb;
        GameObject _struckBy;
        BallManager _bm;

        bool _isStruck = false;
        bool _isBunted = false;
        bool _buntable = true;
        float _currentSpeed;
        float _initialGravityScale = 2.5f;
        float _buntHeight = 5f;
        float _buntXModifier = 0.5f;
        float _buntCooldown = 0.2f;
        int _damage = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isStruck || _isBunted) _rb.gravityScale = _initialGravityScale;
            else _rb.gravityScale = 0f;

            if (_isStruck && !_struckBy.activeSelf)
            {
                _isStruck = false;
            }
        }

        public void Strike(float angle, GameObject striker)
        {
            if (!_isStruck) _isStruck = true;
            if (_isBunted) _isBunted = false;

            _struckBy = striker;

            if (_bm) _bm.IncreaseBallSpeed();

            _rb.linearVelocity = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) * _currentSpeed;
        }

        public void Bunt(GameObject striker)
        {
            if (!_buntable) return;

            if (!_isStruck) _isStruck = true;
            if (!_isBunted) _isBunted = true;

            _struckBy = striker;

            var x_vel = striker.GetComponent<Rigidbody2D>().linearVelocityX;

            _rb.gravityScale = _initialGravityScale;
            _rb.linearVelocity = new Vector2(x_vel * _buntXModifier, _buntHeight * _rb.gravityScale);

            StartCoroutine(BuntCooldown());
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
            _isStruck = false;
            _currentSpeed = bm.CurrentBallSpeed;
        }

        public void SetBallSpeed(float speed)
        {
            _currentSpeed = speed;
        }

        IEnumerator BuntCooldown()
        {
            _buntable = false;

            yield return new WaitForSeconds(_buntCooldown);

            _buntable = true;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (_isStruck)
            {
                if (collision.gameObject.tag == "Ground")
                {
                    if (_bm) _bm.ResetBallSpeed();

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