using UnityEngine;

#pragma warning disable CS0618

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 9f;
    public int maxJumps = 2;

    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundMask;

    private Rigidbody2D rb;
    private int jumpsLeft;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        CheckGround();
        Move();
        Jump();
    }

    void Move()
    {
        float x = 0f;
        if (Input.GetKey(KeyCode.A)) x = -1f;
        if (Input.GetKey(KeyCode.D)) x = 1f;

        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        if (jumpsLeft <= 0) return;

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpsLeft--;
    }

    void CheckGround()
    {
        bool groundedNow = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundMask
        );

        if (groundedNow && !isGrounded)
            jumpsLeft = maxJumps;

        isGrounded = groundedNow;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
#endif
}

#pragma warning restore CS0618
