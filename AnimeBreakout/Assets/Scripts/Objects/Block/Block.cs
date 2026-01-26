using UnityEngine;
using Game.Interfaces;
using System.Collections;

namespace Game.Objects.Block
{
    public class Block : MonoBehaviour, IDamageable
    {
        BlockManager _bm;
        Collider2D _collider;

        IOnDestroy[] _onDestroy;

        int _maxHealth = 2;
        int _currentHealth;

        public int MaxHealth { get; set; }
        public int CurrentHealth { get; private set; }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ActivateBlock(BlockManager bm)
        {
            if (!_bm) _bm = bm;

            _onDestroy = GetComponents<IOnDestroy>();
            _collider = GetComponent<Collider2D>();

            _currentHealth = _maxHealth;

            if (_collider) _collider.enabled = true;
            
            gameObject.SetActive(true);
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                if (_collider) _collider.enabled = false;

                foreach (IOnDestroy onDestroy in _onDestroy)
                {
                    onDestroy.OnDestroyed(gameObject);
                }

                _bm.DeactivateBlock(this);
            }
            else
            {
                StartCoroutine(Blink());
            }
        }

        IEnumerator Blink()
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            // make sure that _sprites[0] and _sprites[1] are the Player's Lower and Upper body sprites
            for (int i = 0; i < 2; i++)
            {
                sprite.enabled = false;
                yield return new WaitForSeconds(0.05f);
                sprite.enabled = true;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
