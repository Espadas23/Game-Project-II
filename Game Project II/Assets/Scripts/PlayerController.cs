
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    [Header("Animation")]
    public Animator animator;

    [Header("Flashlight Reference")]
    public Flashlight flashlight; // ссылка на фонарик

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private Vector3 initialScale;
    public ArmController armController;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        initialScale = transform.localScale;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // --- Управление
        moveInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) moveInput -= 1f;
            if (Keyboard.current.dKey.isPressed) moveInput += 1f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                Jump();
            }
        }
        else
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
        }
        
        // Блокировка движения
        if (flashlight != null && !flashlight.hasActivatedOnce)
            moveInput = 0f;

        // --- Проверка земли
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        if (isGrounded && !wasGrounded)
        {
            animator.ResetTrigger("JumpStart");
        }
        
        // --- Поворот персонажа
        if (moveInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        else if (moveInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);

        // --- Звуки шагов
        if (Mathf.Abs(moveInput) > 0.01f && isGrounded && (flashlight == null || flashlight.hasActivatedOnce))
        {
            SoundManager.Instance?.PlayFootsteps();
        }
        else
        {
            SoundManager.Instance?.StopFootsteps();
        }
        
        ArmControllerLogic();

        // --- Параметры для аниматора
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        animator.SetTrigger("JumpStart");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        SoundManager.Instance?.PlayJump();
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void ArmControllerLogic()
    {
        if (armController == null)
            armController = GetComponentInChildren<ArmController>(true);

        if (armController == null)
            return;

        // Передаём направление (true = вправо, false = влево)
        armController.isFacingRight = transform.localScale.x > 0f;
    }
}



