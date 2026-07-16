using UnityEngine;
using Game.Interfaces;
using Game.Objects.Balls;

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
        Debug.Log("called onDestroyed");

        if (!_bm) _bm = FindAnyObjectByType<BallManager>();

        if (obj && _bm)
        {
            Debug.Log(obj.transform.position);

            _bm.AddNewBall(obj.transform.position);
        }
    }
}
