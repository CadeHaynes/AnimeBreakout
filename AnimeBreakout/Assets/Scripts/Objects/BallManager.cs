using System.Collections.Generic;
using UnityEngine;

namespace Game.Objects.Ball
{
    public class BallManager : MonoBehaviour
    {
        public int BallCount { get; private set; }

        [SerializeField] GameObject _ballPrefab;
        [SerializeField] Vector2 _startBallPos;

        List<Ball> _balls = new List<Ball>();

        public void StartManager()
        {
            BallCount = 0;
            AddNewBall(_startBallPos);
        }

        //Just for testing
        void Start()
        {
            StartManager();

            Debug.Log("Start");
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

            Debug.Log("Ball Count" + BallCount);
        }

        public void DeactivateBall(Ball ball)
        {
            ball.gameObject.SetActive(false);
            BallCount--;
        }
    }
}
