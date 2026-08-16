using Game.Objects.Balls;
using Game.Objects.Layout;

using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;

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

        [SerializeField] GameObject _menuPanel;
        [SerializeField] GameObject _gameOverPanel;

        [SerializeField] InputActionAsset _inputActions;
        [SerializeField] string _uiActionMap = "UI";
        [SerializeField] string _pauseActionName = "Pause";

        InputAction _pause;

        float _displayedScore = 0f;
        // float _displayedHealth = 0f;

        float _scorePopupMaxTime = 2f;
        float _scorePopupCurrentTime = 2f;

        bool _pausePressed = false;
        
        public void StartManager()
        {
            _displayedScore = 0f;
            // _displayedHealth = 0f;

            _pause = _inputActions.FindActionMap(_uiActionMap).FindAction(_pauseActionName);

            _pause.Enable();
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

        public void ToggleMenu(bool isActive)
        {
            if (_menuPanel)
            {
                _menuPanel.SetActive(isActive);

                if (isActive) Time.timeScale = 0f;
                else Time.timeScale = 1f;
            }
        }

        public void ToggleGameOver(bool isActive)
        {
            if (_gameOverPanel)
            {
                _gameOverPanel.SetActive(isActive);

                /*
                if (isActive) Time.timeScale = 0f;
                else Time.timeScale = 1f;
                */
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

            if (_menuPanel)
            {
                if (_pause.ReadValue<float>() > 0f && !_pausePressed)
                {
                    _pausePressed = true;

                    if (_menuPanel.activeSelf) ToggleMenu(false);
                    else ToggleMenu(true);
                }
                else if (_pause.ReadValue<float>() == 0f) _pausePressed = false;
            }
        }
    }
}
