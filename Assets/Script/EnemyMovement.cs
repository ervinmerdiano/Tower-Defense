using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 1.25f;
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private int moveDirection = 1; // 1 = kanan, -1 = kiri

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Gerak terus sesuai arah
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        // Flip sprite sesuai arah
        sprite.flipX = (moveDirection < 0);

        // Auto anim: Jump kalau vertikal berubah
        bool isAir = Mathf.Abs(rb.velocity.y) > 0.1f;
        anim.SetBool("isJumping", isAir);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JumpPointRight"))
        {
            moveDirection = 1;
            Jump();
        }
        if (other.CompareTag("JumpPointLeft"))
        {
            moveDirection = -1;
            Jump();
        }
    }
}
