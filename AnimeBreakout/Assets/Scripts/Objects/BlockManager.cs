using System.Collections.Generic;
using UnityEngine;

namespace Game.Objects.Blocks
{
    public class BlockManager : MonoBehaviour
    {
        [SerializeField] GameObject _blockPrefab;
        
        List<Block> _blocks;
        int _blockCount;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InitialiseBlocks();
        }

        // Update is called once per frame
        void Update()
        {
            if (_blockCount <= 0)
            {
                ReactivateBlocks();
            }
        }

        public void DeactivateBlock(Block block)
        {
            block.gameObject.SetActive(false);
            _blockCount--;
        }

        void InitialiseBlocks()
        {
            _blocks = new List<Block>();
            _blockCount = 0;

            foreach (Transform block in transform)
            {
                var component = block.GetComponent<Block>();

                _blocks.Add(component);
                component.ActivateBlock(this);

                _blockCount++;
            }
        }

        void ReactivateBlocks()
        {
            _blockCount = 0;

            foreach (Block block in _blocks)
            {
                block.ActivateBlock(this);
                _blockCount++;
            }
        }
    }
}
