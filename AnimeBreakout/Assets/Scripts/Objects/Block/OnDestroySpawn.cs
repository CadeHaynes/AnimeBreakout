using UnityEngine;
using Game.Interfaces;

namespace Game.Objects.Blocks
{
    public class OnDestroySpawn : MonoBehaviour, IOnDestroy
    {
        [SerializeField] GameObject _spawnObj;

        public void OnDestroyed(GameObject obj)
        {
            if (_spawnObj && obj)
            {
                Instantiate(_spawnObj, obj.transform.position, obj.transform.rotation);
            }
        }
    }
}