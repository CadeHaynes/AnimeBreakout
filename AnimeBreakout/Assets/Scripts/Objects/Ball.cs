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
        bool _isSoftlocked = false;
        bool _buntable = true;

        float _currentSpeed;
        float _initialGravityScale = 2.5f;
        float _buntHeight = 5f;
        float _buntXModifier = 0.5f;
        float _buntCooldown = 0.2f;
        float _stuckBallTimer;
        [SerializeField] float _stuckBallTimerMax = 5f;

        int _damage = 1;

        Vector2 _lastPos;

        public bool IsStruck { get { return _isStruck; } }
        public bool IsSoftlocked { get { return _isSoftlocked; } }
        public int Damage { get { return _damage; } }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _trail = GetComponent<TrailRenderer>();
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
        }

        private void FixedUpdate()
        {
            CheckForStuckBall(new Vector2(transform.position.x, transform.position.y));
        }

        void CheckForStuckBall(Vector2 currPos)
        {
            // round currPos to 2 decimal places
            currPos.x = Mathf.Round(currPos.x * 100f) / 100f; 
            currPos.y = Mathf.Round(currPos.y * 100f) / 100f;
            
            if (_lastPos != Vector2.zero)
            {
                if (currPos.x == _lastPos.x || currPos.y == _lastPos.y)
                {
                    _stuckBallTimer -= Time.deltaTime;

                    if (_stuckBallTimer <= 0)
                    {
                        _isSoftlocked = true;
                    }
                }
                else
                {
                    _stuckBallTimer = _stuckBallTimerMax;
                }
            }
            else
            {
                _stuckBallTimer = _stuckBallTimerMax;
            }

            _lastPos = currPos;
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

            _isSoftlocked = false;
            _stuckBallTimer = _stuckBallTimerMax;
            if (_rb) _rb.linearVelocity = Vector2.zero;
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

                    _stuckBallTimer = _stuckBallTimerMax;
                }
            }
        }
    }
}