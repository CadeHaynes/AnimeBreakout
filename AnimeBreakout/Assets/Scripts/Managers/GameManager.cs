using Game.Objects.Layout;
using Game.Objects.Balls;

using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] BallManager _bm;
        [SerializeField] LayoutManager _lm;
        //[SerializeField] UIManager _um;

        [SerializeField] Vector2 _playerSpawn;

        [SerializeField] GameObject _playerPrefab;

        void Start()
        {
            if (_playerPrefab) Instantiate(_playerPrefab, _playerSpawn, Quaternion.identity);

            if (_bm) _bm.StartManager();
            if (_lm) _lm.StartManager();
            //if (_um) _um.StartManager();
        }
    }
}
