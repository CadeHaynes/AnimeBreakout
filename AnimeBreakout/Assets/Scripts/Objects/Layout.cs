using UnityEngine;

namespace Game.Objects.Layout
{
    public class Layout : ScriptableObject
    {
        [SerializeField] Vector2[] _blockCoords;
        [SerializeField] Vector2[] _enemyCoords;

        public Vector2[] BlockCoords
        {
            get { return _blockCoords; }
        }

        public Vector2[] EnemyCoords
        {
            get { return _enemyCoords; }
        }
    }
}