using UnityEngine;

namespace Game.Objects.Layout
{
    [CreateAssetMenu(menuName = "Layout")]
    public class Layout : ScriptableObject
    {
        [SerializeField] Vector2[] _blockCoords;

        public Vector2[] BlockCoords
        {
            get { return _blockCoords; }
        }
    }
}