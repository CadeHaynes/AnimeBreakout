using UnityEngine;
using Game.Interfaces;
using System.Collections;

namespace Game.Objects.Balls
{
    public class Ball : MonoBehaviour
    {      
        Rigidbody2D _rb;

        GameObject _struckBy;

        BallManager _bm;

        TrailRenderer _trail;

        bool _isStruck = false;
        bool _isBunted = false;
        bool _buntable = true;

        float _currentSpeed;
        float _initialGravityScale = 2.5f;
        float _buntHeight = 5f;
        float _buntXModifier = 0.5f;
        float _buntCooldown = 0.2f;
        float _lastYPos = 0f;
        float _ballDropTimer;
        [SerializeField] float _ballDropTimerMax = 5f;

        int _damage = 1;

        public bool IsStruck { get { return _isStruck; } }
        public int Damage { get { return _damage; } }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _trail = GetComponent<TrailRenderer>();

            _ballDropTimer = _ballDropTimerMax;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isStruck || _isBunted) _rb.gravityScale = _initialGravityScale;
            else _rb.gravityScale = 0f;

            if (_isStruck && !_struckBy.activeSelf)
            {
                _isStruck = false;
                _struckBy = null;
            }

            var currYPos = transform.position.y;
            currYPos = Mathf.Round(currYPos * 10f) / 10f; // round to 1 decimal place
            
            if (_lastYPos != 0 && currYPos == _lastYPos)
            {
                _ballDropTimer -= Time.deltaTime;

                if (_ballDropTimer <= 0)
                {
                    _isBunted = true;
                    _ballDropTimer = _ballDropTimerMax;
                }
            }
            else
            {
                _ballDropTimer = _ballDropTimerMax;
            }

            _lastYPos = currYPos;
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
            var damageable = obj.GetComponentInParent<IDamageable>();

            Debug.Log(damageable);

            if (damageable != null)
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

            _rb.linearVelocity = _rb.linearVelocity.normalized * _currentSpeed;
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
                    //if (_bm) _bm.ResetBallSpeed();

                    if (_bm && _bm.BallCount > 1)
                    {
                        _bm.DeactivateBall(this);
                    }
                    else if (_bm.BallCount <= 1)
                    {
                        TryToDamage(_struckBy);

                        transform.position = _bm.StartBallPos;
                        _rb.linearVelocity = Vector2.zero;
                        _isStruck = false;
                        _trail.Clear();
                    }
                }

                if (collision.gameObject.tag == "Block")
                {
                    //TryToDamage(collision.gameObject);
                }
            }
        }
    }
}