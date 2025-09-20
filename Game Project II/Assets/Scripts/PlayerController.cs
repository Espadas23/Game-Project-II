using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;           // скорость бега
    public float jumpForce = 12f;          // сила прыжка

    [Header("Ground Check")]
    public Transform groundCheck;          // пустой объект чуть под ногами
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    [Header("Jump Helpers")]
    public float coyoteTime = 0.12f;       // время после отрыва от земли, когда ещё можно прыгнуть
    public float jumpBufferTime = 0.12f;   // буфер нажатия перед касанием земли

    [Header("Animation")]
    public Animator animator;

    // внутренние
    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private Vector3 initialScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        initialScale = transform.localScale;

        rb.freezeRotation = true; // предотвращаем падение на бок
    }

    void Update()
    {
        // --- Чтение ввода
        moveInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) moveInput -= 1f;
            if (Keyboard.current.dKey.isPressed) moveInput += 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space)) jumpBufferTimer = jumpBufferTime;
        }

        // --- Ground Check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.deltaTime;

        jumpBufferTimer -= Time.deltaTime;

        // --- Прыжок
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // --- Флип персонажа
        if (moveInput > 0.01f) transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        else if (moveInput < -0.01f) transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);

        // --- Анимации
        // Blend Tree должен использовать параметр "Speed"
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x)); 
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        // движение
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
