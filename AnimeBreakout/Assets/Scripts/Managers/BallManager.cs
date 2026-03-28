using System.Collections.Generic;
using UnityEngine;

namespace Game.Objects.Ball
{
    public class BallManager : MonoBehaviour
    {
        [SerializeField] GameObject _ballPrefab;
        List<Ball> _balls = new List<Ball>();
        int _ballCount = 0;

        public List<Ball> Balls { get { return _balls; } } //not sure if needed.

        public void ActivateBall(Vector2 pos)
        {
            if (_balls.Count == 0)
            {
                InstantiateNewBall();
            }
            else
            {
                for (int i = 0;  i < _balls.Count; i++)
                {
                    var currBall = _balls[i].gameObject;

                    if (!currBall.activeSelf)
                    {
                        currBall.transform.position = pos;
                        currBall.SetActive(true); //this line would probably be to call the ball's activate code (refer to block manager/block)
                        _ballCount++;

                        break;
                    }
                }
            }
        }

        public void InstantiateNewBall()
        {
            var newBall = Instantiate(_ballPrefab);

            var ballClass = newBall.GetComponent<Ball>();

            if (ballClass != null)
            {
                _balls.Add(ballClass);
                _ballCount++;
            }
        }
    }
}
