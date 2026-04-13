using System.Collections.Generic;
using UnityEngine;

namespace Game.Objects.Ball
{
    public class BallManager : MonoBehaviour
    {
        public int BallCount { get; private set; }
        public float CurrentBallSpeed { get { return _currentBallSpeed; } }
        public float BallSpeedStep { get { return _ballSpeedStep; } }

        [SerializeField] GameObject _ballPrefab;
        [SerializeField] Vector2 _startBallPos;
        [SerializeField] float _startBallSpeed = 10f;
        [SerializeField] float _ballSpeedStep = 0.5f;

        List<Ball> _balls = new List<Ball>();
        float _currentBallSpeed;

        public void StartManager()
        {
            BallCount = 0;
            _currentBallSpeed = _startBallSpeed;
            AddNewBall(_startBallPos);
        }

        //Just for testing
        void Start()
        {
            StartManager();
        }

        public void AddNewBall(Vector2 pos)
        {
            if (BallCount == _balls.Count)
            {
                InstantiateNewBall(pos);
            }
            else
            {
                for (int i = 0;  i < _balls.Count; i++)
                {
                    var currBall = _balls[i].gameObject;

                    if (!currBall.activeSelf)
                    {
                        currBall.transform.position = pos;
                        _balls[i].ActivateBall(this);
                        BallCount++;

                        break;
                    }
                }
            }
        }

        public void AddExistingBall(Ball ball)
        {
            _balls.Add(ball);

            if (ball.gameObject.activeSelf)
            {
                BallCount++;
            }
        }

        public void InstantiateNewBall(Vector2 pos)
        {
            var newBall = Instantiate(_ballPrefab, pos, Quaternion.identity);

            var ballClass = newBall.GetComponent<Ball>();

            if (ballClass != null)
            {
                ballClass.ActivateBall(this);
                _balls.Add(ballClass);
                
                BallCount++;
            }
        }

        public void DeactivateBall(Ball ball)
        {
            ball.gameObject.SetActive(false);
            BallCount--;
        }

        public void IncreaseBallSpeed()
        {
            _currentBallSpeed += _ballSpeedStep;

            foreach (var ball in _balls)
            {
                if (ball.gameObject.activeSelf)
                {
                    ball.SetBallSpeed(_currentBallSpeed);
                }
            }
        }

        public void ResetBallSpeed()
        {
            _currentBallSpeed = _startBallSpeed;

            foreach (var ball in _balls)
            {
                if (ball.gameObject.activeSelf)
                {
                    ball.SetBallSpeed(_currentBallSpeed);
                }
            }
        }
    }
}
