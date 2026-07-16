using System.Collections.Generic;
using UnityEngine;
using Game.Objects.Blocks;
using Game.Objects.Balls;
using UnityEngine.Rendering;
using Game.Interfaces;
using Unity.VisualScripting;

namespace Game.Objects.Layout
{
    public class LayoutManager : MonoBehaviour
    {
        [SerializeField] GameObject[] _blockPrefabs;
        [SerializeField] GameObject[] _layoutPrefabs;
        [SerializeField] GameObject _groundLayoutPrefab;

        [SerializeField] bool _resetGround;

        List<Block> _allBlocks = new List<Block>();
        List<Block> _currentGroundBlocks = new List<Block>();
        List<Block> _currentAirBlocks = new List<Block>();

        public event System.Action OnAirBlockDestroyed;
        public event System.Action OnGroundBlockDestroyed;

        // Update is called once per frame
        void Update()
        {
            if (_currentAirBlocks.Count <= 0)
            {
                Debug.Log("restarting layout");
                InitialiseLayout(_resetGround);
            }
        }

        public void StartManager()
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

            var layoutIndex = Random.Range(0, _layoutPrefabs.Length);

            if (_layoutPrefabs[layoutIndex] != null)
            {
                // maybe could use a class that stores all the blocks?
                var blocks = _layoutPrefabs[layoutIndex].GetComponentsInChildren<Block>();
                var index = 0;

                foreach (var block in blocks)
                {
                    AddNewBlock(block.transform);

                    if (block.GetComponents<IOnDestroy>().Length > 0)
                    {
                        foreach (IOnDestroy onDestroy in block.GetComponents<IOnDestroy>()) _allBlocks[index].gameObject.AddComponent(onDestroy.GetType());
                    }

                    index++;
                }
            }

            if (_groundLayoutPrefab != null && resetGround)
            {
                var groundBlocks = _groundLayoutPrefab.GetComponentsInChildren<Block>();

                foreach (var block in groundBlocks)
                {
                    AddNewBlock(block.transform, true);
                }
            }
        }

        void AddNewBlock(Transform transform, bool isGround = false)
        {
            for (int i = 0; i < _allBlocks.Count; i++)
            {
                var currBlock = _allBlocks[i];

                if (!currBlock.gameObject.activeSelf)
                {
                    currBlock.transform.position = transform.position;
                    currBlock.transform.rotation = transform.rotation;
                    currBlock.transform.localScale = transform.localScale;
                    _allBlocks[i].ActivateBlock(this, isGround);

                    if (isGround) _currentGroundBlocks.Add(currBlock);
                    else _currentAirBlocks.Add(currBlock);

                    return;
                }
            }

            InstantiateNewBlock(transform, isGround);
        }

        void InstantiateNewBlock(Transform transform, bool isGround = false)
        {
            var newBlock = Instantiate(_blockPrefabs[0], transform.position, transform.rotation, this.transform);
            newBlock.transform.localScale = transform.localScale;

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