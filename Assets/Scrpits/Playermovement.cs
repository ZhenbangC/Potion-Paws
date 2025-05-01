
using UnityEngine;

public class Playermovement : MonoBehaviour
{

    [Header("移动参数")]
    public float moveSpeed = 5f;

    [Header("跳跃参数")]
    public float jumpForce = 10f;
    public int maxJumps = 2;
    private int availableJumps;

    [Header("地面检测")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("墙壁检测")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;
    private bool isTouchingWall;
    public LayerMask wallLayer;

    [Header("墙滑相关")]
    public float wallSlideSpeed = 2f;
    private bool isWallSliding;

    [Header("跳跃容错")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("组件引用")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("墙跳限制")]
    public float wallJumpCooldown = 0.3f;
    private float wallJumpCooldownTimer = 0f;

    private float moveInput;
    private int facingDirection = 1;
    private int lastWallJumpDirection = 0;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        availableJumps = maxJumps;
    }

    void Update()
    {
        if (wallJumpCooldownTimer > 0f)
            wallJumpCooldownTimer -= Time.deltaTime;

        if (isDead) return;

        moveInput = Input.GetAxisRaw("Horizontal");
        
        animator.SetFloat("yVelocity", rb.velocity.y);

        CheckGrounded();
        HandleCoyoteTime();
        FlipCharacter();
        CheckWall();
        HandleWallSlide();
        HandleJump();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (!isWallSliding)
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(moveInput * moveSpeed * 0.5f, rb.velocity.y);
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
            availableJumps = maxJumps;


        animator.SetBool("Jump", !isGrounded);
    }

    void CheckWall()
    {
        bool wasTouchingWall = isTouchingWall;

        isTouchingWall = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, wallLayer);

        
        if (wasTouchingWall && !isTouchingWall)
        {
            lastWallJumpDirection = 0;
        }
    }
    void HandleCoyoteTime()
    {
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           
            if (isWallSliding && wallJumpCooldownTimer <= 0f)
            {
                int currentWallDirection = -facingDirection; // 玩家面向右 => 墙在左边，跳向右 => 方向为 -1

                // 如果墙的方向和上次跳的一样，禁止跳（说明玩家还在同一面墙）
                if (currentWallDirection != lastWallJumpDirection)
                {
                    rb.velocity = new Vector2(-facingDirection * moveSpeed, jumpForce);
                    wallJumpCooldownTimer = wallJumpCooldown;
                    isWallSliding = false;

                    lastWallJumpDirection = currentWallDirection; // 记录这次跳的方向
                }
            }
            // 正常跳跃
            else if ((coyoteTimeCounter > 0f || availableJumps > 0) && wallJumpCooldownTimer <= 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                animator.SetBool("Jump", true);

                availableJumps--;
                coyoteTimeCounter = 0;
                lastWallJumpDirection = 0; // 普通跳跃不算墙跳，重置方向
            }
        }
    }
    void HandleWallSlide()
    {
        isWallSliding = false;

        if (isTouchingWall && !isGrounded && moveInput == facingDirection)
        {
            isWallSliding = true;

            if (rb.velocity.y < -wallSlideSpeed)
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);

            Debug.Log("正在墙滑！");
        }
    }

    void FlipCharacter()
    {
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
            facingDirection = 1;
            wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
            facingDirection = -1;
            wallCheck.localPosition = new Vector3(-Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
        }
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("WallSlide", isWallSliding);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance * facingDirection);
        }
    }

    public void Die()
    {
        isDead = true;
        FindObjectOfType<LevelManager>().Restart();
    }

    public void ResetPlayer()
    {
        isDead = false;
    }

}