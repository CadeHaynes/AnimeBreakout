using Game.Objects.Balls;
using Game.Objects.Layout;
using UnityEngine;

namespace Game
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] float _score = 0f;
        [SerializeField] float _scoreMultiplier = 0f;
        [SerializeField] float _scoreMultiplierIncrement = .5f;
        [SerializeField] float _scoreBall = 100f;

        BallManager _bm;

        LayoutManager _lm;

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
        public float ScoreBall
        {
            get
            {
                return _scoreBall;
            }
        }
        
        public void StartManager()
        {
            _score = 0f;
            _scoreMultiplier = 0f;
        }

        public void SetBallManager(BallManager bm)
        {
            _bm = bm;
        }

        public void SetLayoutManager(LayoutManager lm)
        {
            _lm = lm;

            if (_lm) _lm.OnGroundBlockDestroyed += ResetScoreMultiplier;
        }

        public void IncrementScore()
        {
            _score += _scoreBall * (_scoreMultiplier + _bm.BallCount);
            _scoreMultiplier += _scoreMultiplierIncrement;
        }

        void ResetScoreMultiplier()
        {
            _scoreMultiplier = 0f;
        }
    }
}