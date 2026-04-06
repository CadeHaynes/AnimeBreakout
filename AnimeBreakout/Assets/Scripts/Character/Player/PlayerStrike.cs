using UnityEngine;
using Game.Objects.Ball;

namespace Game.Character.Player
{
    public class PlayerStrike : MonoBehaviour
    {
        // Private
        [SerializeField] BoxCollider2D _collider;

        [SerializeField] float maxAngle = 60f;

        public void Strike(bool isAuto = false)
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
                        // Sends the ball in a direction based on its offset from the center of the _collider.
                        var offset = ball.transform.position.x- _collider.bounds.center.x;
                        var normalizedOffset = Mathf.Clamp(offset / _collider.bounds.extents.x, -1f, 1f);
                        var newAngle = normalizedOffset * maxAngle;

                        ball.Strike(newAngle, gameObject);
                    }
                    else
                    {
                        // Manual
                        ball.Bunt(gameObject);
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