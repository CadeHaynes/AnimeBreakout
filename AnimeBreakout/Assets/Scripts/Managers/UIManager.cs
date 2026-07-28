using Game.Objects.Balls;
using UnityEngine;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        ScoreManager _sm;
        BallManager _bm;

        [SerializeField] TMPro.TextMeshProUGUI _scoreText;
        [SerializeField] TMPro.TextMeshProUGUI _healthText;

        float _displayedScore = 0f;
        float _displayedHealth = 0f;
        
        public void StartManager()
        {
            _displayedScore = 0f;
            _displayedHealth = 0f;
        }

        public void SetScoreManager(ScoreManager sm)
        {
            _sm = sm;
        }

        public void SetBallManager(BallManager bm)
        {
            _bm = bm;
        }

        // Update is called once per frame
        void Update()
        {
            if (_sm)
            {
                _displayedScore = _sm.Score;

                var mult = _bm.BallCount + _sm.ScoreMultiplier;
                _scoreText.text = _displayedScore.ToString("0") + " x" + mult.ToString("0");
            }
        }
    }
}
