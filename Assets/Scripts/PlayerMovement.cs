using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;

    public int maxJumpCount = 2; // сколько прыжков можно сделать
    private int jumpCount;

    private Rigidbody2D rb;
    private bool isGrounded = true; // позже лучше заменить на Raycast/Collider проверку

    private Collider2D playerCollider;
    private bool isDropping = false;
    private Collider2D currentSurface;

    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumpCount;
    }

    void Update()
    {
        Move();

        if (TryDrop()) return;

        HandleJump();
    }

    void Move()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
            moveInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            moveInput = 1f;

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        // если нажата S — значит это попытка drop, не прыгаем
        if (Input.GetKey(KeyCode.S)) return;

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpCount -= 1;
    }

    // пример простого сброса прыжков при касании земли
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDropping) return; // ⭐ ВАЖНО
        
        if (IsFloor(collision))
        {
            jumpCount = maxJumpCount;
        }

        if (collision.gameObject.CompareTag("Surface"))
        {
            isGrounded = true;
            currentSurface = collision.collider;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            isGrounded = false;
            currentSurface = null;
        }
    }


    IEnumerator DisableCollisionTemporarily(Collider2D platform)
    {
        isDropping = true;

        Physics2D.IgnoreCollision(playerCollider, platform, true);

        // лёгкий толчок вниз чтобы выйти из коллайдера
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -3f);

        yield return new WaitForSeconds(0.4f);

        Physics2D.IgnoreCollision(playerCollider, platform, false);

        isDropping = false;
    }

    bool TryDrop()
    {
        if (!isGrounded) return false;

        if (currentSurface == null) return false;

        // ⭐ если это Ground — НЕ даём падать
        if (currentSurface.CompareTag("Ground"))
            return false;

        // ⭐ падать можно только через Surface
        if (!currentSurface.CompareTag("Surface"))
            return false;

        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DisableCollisionTemporarily(currentSurface));
            return true;
        }

        return false;
    }

    bool IsFloor(Collision2D col)
    {
        return col.gameObject.CompareTag("Surface") ||
               col.gameObject.CompareTag("Ground");
    }


}