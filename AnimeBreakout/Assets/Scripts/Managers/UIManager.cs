using Game.Objects.Balls;
using UnityEngine;
using TMPro;
using Game.Objects.Layout;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        ScoreManager _sm;
        BallManager _bm;
        LayoutManager _lm;

        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] TextMeshProUGUI _healthText;
        [SerializeField] TextMeshProUGUI _scorePopupText;

        float _displayedScore = 0f;
        float _displayedHealth = 0f;

        float _scorePopupMaxTime = 2f;
        float _scorePopupCurrentTime = 2f;
        
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

        public void SetLayoutManager(LayoutManager lm)
        {
            _lm = lm;

            if (_lm)
            {
                _lm.OnGroundBlockDestroyed += DisplayScorePopup;
                _lm.OnAirBlockDestroyed += DisplayScorePopup;
            }
        }

        void DisplayScorePopup()
        {
            var scoreGained = _sm.ScoreBall * (_sm.ScoreMultiplier + _bm.BallCount);

            _scorePopupText.text = "+" + scoreGained.ToString("0");
            _scorePopupText.gameObject.SetActive(true);

            _scorePopupCurrentTime = _scorePopupMaxTime;
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

            if (_scorePopupText.gameObject.activeSelf)
            {
                _scorePopupCurrentTime -= Time.deltaTime;

                if (_scorePopupCurrentTime <= 0f)
                {
                    _scorePopupText.gameObject.SetActive(false);
                }
            }
        }
    }
}
