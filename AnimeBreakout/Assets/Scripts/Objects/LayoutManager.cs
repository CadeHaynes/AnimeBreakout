using Game.Objects.Ball;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Objects.Layout
{
    public class LayoutManager : MonoBehaviour
    {
        [SerializeField] GameObject _blockPrefabs;
        //[SerializeField] GameObject[] _enemyPrefabs;
        [SerializeField] Layout[] _layouts;

        List<GameObject> _usableBlocks = new List<GameObject>();
        //List<GameObject> _usableEnemies = new List<GameObject>();

        int _activeObjects = 0;
        int _blockCount = 0;
        //int _enemyCount = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void InitialiseLayout()
        {
            _usableBlocks.Clear();
            //_usableEnemies.Clear();
            _activeObjects = 0;

            var layoutIndex = Random.Range(0, _layouts.Length);

            if (_layouts[layoutIndex] != null) 
            {
                var blocks = _layouts[layoutIndex].BlockCoords;
                //var enemies = _layouts[layoutIndex].EnemyCoords;

                foreach (var coords in blocks)
                {
                    AddNewBlock(coords);
                }
            }
        }

        void AddNewBlock(Vector2 pos)
        {
            if (_blockCount == _usableBlocks.Count)
            {
                //InstantiateNewBlock(pos);
            }
            else
            {
                for (int i = 0; i < _usableBlocks.Count; i++)
                {
                    var currBlock = _usableBlocks[i];

                    if (!currBlock.activeSelf)
                    {
                        currBlock.transform.position = pos;
                        //_usableBlocks[i].ActivateBall(this);
                        _blockCount++;

                        break;
                    }
                }
            }
        }
    }
}