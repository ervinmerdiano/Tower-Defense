using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2.5f;
    public float jumpForce = 6.5f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private float moveX;
    private bool isGrounded;
    private bool wasGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        // Gerak kiri/kanan
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Flip sprite
        if (moveX > 0) sprite.flipX = false;
        else if (moveX < 0) sprite.flipX = true;

        // Cek tanah
        CheckGround();

        // --- LOMPAT ---
        if (Input.GetButtonDown("Jump") && isGrounded && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetBool("isJumping", true);
        }
        if (isGrounded && anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
        anim.SetBool("isJumping", false);
        }

        UpdateAnimationStates();
    }

    void CheckGround()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void UpdateAnimationStates()
    {
    anim.SetBool("isRunning", Mathf.Abs(moveX) > 0.1f);
    anim.SetFloat("yVelocity", rb.velocity.y);

    // Kalau baru nyentuh tanah
    if (isGrounded && !wasGrounded)
    {
        anim.SetBool("isGrounded", true);
        anim.SetBool("isJumping", false);

        // 🟩 FORCE keluar dari jump animation
        anim.Play("Idle");
    }

    // Tanah -> Idle ketika sudah benar benar stop jatuh
    if (isGrounded && Mathf.Abs(rb.velocity.y) < 0.05f)
    {
        anim.SetBool("isJumping", false);
        anim.SetBool("isGrounded", true);

        // 🟩 Pastikan kembali ke idle jika masih stuck
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            anim.Play("Idle");
        }
    }

    // Kalau lagi di udara
    if (!isGrounded)
    {
        anim.SetBool("isGrounded", false);
    }
}


    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
