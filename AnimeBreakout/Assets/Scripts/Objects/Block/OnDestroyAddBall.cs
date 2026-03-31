using UnityEngine;
using Game.Interfaces;
using Game.Objects.Ball;

public class OnDestroyAddBall : MonoBehaviour, IOnDestroy
{
    [SerializeField] GameObject _ball;

    BallManager _bm;

    void Start()
    {
        _bm = FindFirstObjectByType<BallManager>();
    }

    public void OnDestroyed(GameObject obj)
    {
        if (_ball && obj && _bm)
        {
            Debug.Log(obj.transform.position);

            _bm.AddNewBall(obj.transform.position);
        }
    }
}
