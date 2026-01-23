using UnityEngine;

namespace Game.Character
{
    public class CharacterStats : MonoBehaviour
    {
        // Private
        CharacterBaseStats _baseStats;

        // Public
        public int Health { get; private set; }
        public float JumpHeight { get; private set; }
        public float Speed { get; private set; }

        private void Awake()
        {
            _baseStats = ScriptableObject.CreateInstance<CharacterBaseStats>();

            Health = _baseStats.health;
            JumpHeight = _baseStats.jumpHeight;
            Speed = _baseStats.speed;
        }

        public void ModifyHealth(int i)
        {
            Health += i;
        }

        public void ModifyJumpHeight(int i)
        {
            JumpHeight += i;
        }

        public void ModifySpeed(int i)
        {
            Speed += i;
        }
    }
}