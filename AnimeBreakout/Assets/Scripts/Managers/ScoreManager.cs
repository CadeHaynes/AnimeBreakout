using UnityEngine;

namespace Game
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] float _score = 0f;
        [SerializeField] float _scoreMultiplier = 1f;
        [SerializeField] float _scoreMultiplierIncrement = .5f;
        [SerializeField] float _scoreBall = 100f;

        public float Score
        {
            get
            {
                return _score;
            }
        }
        public float ScoreMultiplier
        {
            get
            {
                return _scoreMultiplier;
            }
        }
        
        public void StartManager()
        {
            _score = 0f;
            _scoreMultiplier = 1f;
        }

        public void IncrementScore()
        {
            _score += _scoreBall * _scoreMultiplier;
            _scoreMultiplier += _scoreMultiplierIncrement;
        }
    }
}