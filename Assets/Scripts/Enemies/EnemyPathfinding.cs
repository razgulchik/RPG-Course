using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Knockback knockback;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        if (knockback.GettingKnockedBack) { return; }
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

        if (moveDirection.x < 0) {
            spriteRenderer.flipX = true;
        } else if (moveDirection.x > 0) {
            spriteRenderer.flipX = false;
        }
    }

    public void MoveTo (Vector2 targetPosition) {
        moveDirection = targetPosition;
    }

    public void StopMoving() {
        moveDirection = Vector2.zero;
    }
}
