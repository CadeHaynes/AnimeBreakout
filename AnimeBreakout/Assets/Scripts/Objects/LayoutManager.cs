using System.Collections.Generic;
using UnityEngine;
using Game.Objects.Blocks;
using Game.Objects.Balls;

namespace Game.Objects.Layout
{
    public class LayoutManager : MonoBehaviour
    {
        [SerializeField] GameObject[] _blockPrefabs;
        [SerializeField] Layout[] _layouts;
        [SerializeField] Layout _groundLayout;

        [SerializeField] bool _resetGround;

        List<Block> _allBlocks = new List<Block>();
        List<Block> _currentGroundBlocks = new List<Block>();
        List<Block> _currentAirBlocks = new List<Block>();

        List<GameObject> _balls = new List<GameObject>();

        int _totalBlocks = 0;

        public event System.Action OnAirBlockDestroyed;
        public event System.Action OnGroundBlockDestroyed;

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
                InitialiseLayout(_resetGround);
            }
        }

        void StartManager()
        {
            InitialiseLayout(true);
        }

        void InitialiseLayout(bool resetGround = false)
        {
            if (resetGround)
            {
                // Deactivate existing ground blocks so they can be reused, then clear the list.
                for (int i = 0; i < _currentGroundBlocks.Count; i++)
                {
                    var b = _currentGroundBlocks[i];
                    if (b != null) b.gameObject.SetActive(false);
                }

                _currentGroundBlocks.Clear();
            }

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

            if (_groundLayout != null && resetGround)
            {
                var groundBlocks = _groundLayout.BlockCoords;

                foreach (var coords in groundBlocks)
                {
                    AddNewBlock(coords, true);
                }
            }
        }

        void AddNewBlock(Vector2 pos, bool isGround = false)
        {
            // Try to reuse an inactive block from the pool first.
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                var currBlock = _allBlocks[i];

                if (!currBlock.gameObject.activeSelf)
                {
                    currBlock.transform.position = pos;
                    _allBlocks[i].ActivateBlock(this, isGround);

                    if (isGround) _currentGroundBlocks.Add(currBlock);
                    else _currentAirBlocks.Add(currBlock);

                    return;
                }
            }

            // No inactive block available -> instantiate a new one.
            InstantiateNewBlock(pos, isGround);
        }

        void InstantiateNewBlock(Vector2 pos, bool isGround = false)
        {
            //logic here to randomly choose block from block prefab, with more chances for basic block.
            //for now, basic block will be set to 0, and will always be chosen

            var randBlock = 0;

            var newBlock = Instantiate(_blockPrefabs[randBlock], pos, Quaternion.identity, transform);

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

            // _totalBlocks is recalculated each Update, so don't manage it here.
        }

        public void DamageBlock(Ball ball)
        {
            var currentBlock = _allBlocks[0];

            //Loop through each block, store closest active block to ball, then damage that block.
            if (_allBlocks.Count >= 0)
            {
                foreach (var block in _allBlocks)
                {
                    var dist = Vector2.Distance(block.transform.position, ball.transform.position);

                    if (dist < Vector2.Distance(currentBlock.transform.position, ball.transform.position) &&
                        block.gameObject.activeSelf)
                    {
                        currentBlock = block;
                    }
                }

                currentBlock.TakeDamage(ball.Damage);

                if (_currentGroundBlocks.Contains(currentBlock)) OnGroundBlockDestroyed?.Invoke();
                if (_currentAirBlocks.Contains(currentBlock)) OnAirBlockDestroyed?.Invoke();
            }

        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ball")
            {
                var ball = collision.gameObject.GetComponent<Ball>();

                if (ball && ball.IsStruck)
                {
                    DamageBlock(ball);
                }
            }
        }
    }
}