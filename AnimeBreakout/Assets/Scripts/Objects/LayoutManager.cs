using System.Collections.Generic;
using UnityEngine;
using Game.Objects.Blocks;

namespace Game.Objects.Layout
{
    public class LayoutManager : MonoBehaviour
    {
        [SerializeField] GameObject[] _blockPrefabs;
        [SerializeField] Layout[] _layouts;

        List<Block> _allBlocks = new List<Block>();
        List<Block> _currentGroundBlocks = new List<Block>();
        List<Block> _currentAirBlocks = new List<Block>();

        int _totalBlocks = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartManager();
        }

        // Update is called once per frame
        void Update()
        {
            _totalBlocks = _currentAirBlocks.Count + _currentGroundBlocks.Count;

            if (_currentAirBlocks.Count <= 0)
            {
                Debug.Log("restarting layout");
                InitialiseLayout();
            }
        }

        void StartManager()
        {
            InitialiseLayout(true);
        }

        void InitialiseLayout(bool resetGround = false)
        {
            if (resetGround) _currentGroundBlocks.Clear();
            _currentAirBlocks.Clear();

            var layoutIndex = Random.Range(0, _layouts.Length);

            if (_layouts[layoutIndex] != null) 
            {
                var blocks = _layouts[layoutIndex].BlockCoords;

                foreach (var coords in blocks)
                {
                    AddNewBlock(coords);
                }
            }
        }

        void AddNewBlock(Vector2 pos, bool isGround = false)
        {
            if (_allBlocks.Count == _totalBlocks)
            {
                InstantiateNewBlock(pos, isGround);
                _totalBlocks++;
            }
            else
            {
                for (int i = 0; i < _allBlocks.Count; i++)
                {
                    var currBlock = _allBlocks[i];

                    if (!currBlock.gameObject.activeSelf)
                    {
                        currBlock.transform.position = pos;
                        _allBlocks[i].ActivateBlock(this, isGround);

                        if (isGround) _currentGroundBlocks.Add(currBlock);
                        else _currentAirBlocks.Add(currBlock);

                        _totalBlocks++;

                        break;
                    }
                }
            }
        }

        void InstantiateNewBlock(Vector2 pos, bool isGround = false)
        {
            //logic here to randomly choose block from block prefab, with more chances for basic block.
            //for now, basic block will be set to 0, and will always be chosen

            var randBlock = 0;

            var newBlock = Instantiate(_blockPrefabs[randBlock], pos, Quaternion.identity);

            var blockClass = newBlock.GetComponent<Block>();

            if (blockClass != null)
            {
                blockClass.ActivateBlock(this, isGround);

                if (isGround) _currentGroundBlocks.Add(blockClass);
                else _currentAirBlocks.Add(blockClass);

                _allBlocks.Add(blockClass);
            }
        }

        public void DeactivateBlock(Block block)
        {
            block.gameObject.SetActive(false);

            if (block.IsGround) _currentGroundBlocks.Remove(block);
            else _currentAirBlocks.Remove(block);

            _totalBlocks--;
        }
    }
}