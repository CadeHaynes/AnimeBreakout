using UnityEngine;
using Game.Objects.Ball;

namespace Game.Character.Player
{
    public class PlayerStrike : MonoBehaviour
    {
        // Private
        [SerializeField] BoxCollider2D _collider;

        public void Strike(float angle, bool isAuto = false)
        {
            var targets = Physics2D.OverlapBoxAll(_collider.bounds.center, _collider.bounds.size, 0);
            var ballMask = LayerMask.NameToLayer("Ball");

            foreach (var target in targets)
            {
                if (target.gameObject.layer == ballMask)
                {
                    var ball = target.GetComponent<Ball>();

                    if (ball == null) continue;
                    if (ball.GetBallVelocity().y > 0) continue;

                    if (isAuto)
                    {
                        // Auto
                        ball.Strike(angle, this.gameObject);
                    }
                    else
                    {
                        // Manual
                    }
                }
            }
        }

        public bool CheckForTargets()
        {
            var ballMask = 1 << LayerMask.NameToLayer("Ball");
            
            var bound = Physics2D.OverlapBox(_collider.bounds.center, _collider.bounds.size, 0, ballMask);
            
            return bound;
        }    
    }
}