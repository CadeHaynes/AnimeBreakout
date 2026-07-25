using UnityEngine;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        ScoreManager _sm;

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

        // Update is called once per frame
        void Update()
        {
            if (_sm)
            {
                if (_displayedScore != _sm.Score)
                {
                    _displayedScore = _sm.Score;
                    _scoreText.text = _displayedScore.ToString("0");
                }
            }
        }
    }
}
