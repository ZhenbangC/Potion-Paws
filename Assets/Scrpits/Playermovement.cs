using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class Playermovement : MonoBehaviour
{
    [Header("ÒÆ¶¯²ÎÊý")]
    public float moveSpeed = 5f;

    [Header("ÌøÔ¾²ÎÊý")]
    public float jumpForce = 10f;
    public int maxJumps = 2;
    private int availableJumps;

    [Header("µØÃæ¼ì²â")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Ç½±Ú¼ì²â")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;
    private bool isTouchingWall;
    public LayerMask wallLayer;

    [Header("Ç½»¬²ÎÊý")]
    public float wallSlideSpeed = 2f;
    private bool isWallSliding;

    [Header("ÌøÔ¾ÈÝ´í")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Ç½ÌøÏÞÖÆ")]
    public float wallJumpCooldown = 0.3f;
    private float wallJumpCooldownTimer = 0f;
    private int lastWallJumpDirection = 0;

    [Header("»÷ÍË")]
    public float knockbackForce = 10f;
    public float knockbackUpwardForce = 2f;
    public float knockbackDuration = 0.2f;
    private bool isKnockback = false;
    private float knockbackTimer = 0f;
    private int knockbackDirection = 0;

    [Header("½Å²½Éù")]
    public float footstepInterval = 0.4f;
    private float footstepTimer = 0f;

    [Header("×é¼þ")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 respawnPoint;
    private int facingDirection = 1;
    private float moveInput;
    private bool isDead = false;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        availableJumps = maxJumps;
        respawnPoint = transform.position;
    }

    void Update()
    {
        if (isDead || !canMove) return;

        moveInput = Input.GetAxisRaw("Horizontal");

        if (wallJumpCooldownTimer > 0f)
            wallJumpCooldownTimer -= Time.deltaTime;

        animator.SetFloat("yVelocity", rb.velocity.y);

        CheckGrounded();
        HandleCoyoteTime();
        CheckWall();
        HandleWallSlide();
        HandleJump();
        FlipCharacter();
        UpdateAnimator();
        PlayFootstepSound();

        if (isKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockback = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (isDead || !canMove) return;

        if (isKnockback)
        {
            rb.velocity = new Vector2(knockbackDirection * knockbackForce, rb.velocity.y);
            return;
        }

        float speedFactor = isWallSliding ? 0.5f : 1f;
        rb.velocity = new Vector2(moveInput * moveSpeed * speedFactor, rb.velocity.y);
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded) availableJumps = maxJumps;

        animator.SetBool("Jump", !isGrounded);
    }

    void CheckWall()
    {
        bool wasTouchingWall = isTouchingWall;
        isTouchingWall = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, wallLayer);
        if (wasTouchingWall && !isTouchingWall)
            lastWallJumpDirection = 0;
    }

    void HandleCoyoteTime()
    {
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    void HandleWallSlide()
    {
        isWallSliding = false;

        if (isTouchingWall && !isGrounded && moveInput == facingDirection)
        {
            isWallSliding = true;
            if (rb.velocity.y < -wallSlideSpeed)
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWallSliding && wallJumpCooldownTimer <= 0f)
            {
                int currentWallDir = -facingDirection;
                if (currentWallDir != lastWallJumpDirection)
                {
                    rb.velocity = new Vector2(-facingDirection * moveSpeed, jumpForce);
                    wallJumpCooldownTimer = wallJumpCooldown;
                    isWallSliding = false;
                    lastWallJumpDirection = currentWallDir;
                    AudioManager.instance.PlaySFX("ÌøÔ¾");
                }
            }
            else if ((coyoteTimeCounter > 0f || availableJumps > 0) && wallJumpCooldownTimer <= 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                availableJumps--;
                coyoteTimeCounter = 0f;
                lastWallJumpDirection = 0;
                AudioManager.instance.PlaySFX("ÌøÔ¾");
            }
        }
    }

    void FlipCharacter()
    {
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
            facingDirection = 1;
            wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y);
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
            facingDirection = -1;
            wallCheck.localPosition = new Vector3(-Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y);
        }
    }

    void UpdateAnimator()
    {
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("WallSlide", isWallSliding);
    }

    void PlayFootstepSound()
    {
        bool isMoving = Mathf.Abs(moveInput) > 0.1f;
        bool isAbleToStep = isMoving && isGrounded && !isWallSliding && !isKnockback && !isDead;

        if (isAbleToStep)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                AudioManager.instance.PlaySFX("½Å²½");
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            SetRespawnPoint(transform.position);
        }
    }

    public void TakeDamage()
    {
        isKnockback = true;
        knockbackTimer = knockbackDuration;
        knockbackDirection = -facingDirection;

        AudioManager.instance.PlaySFX("ÊÜÉË");
        if (animator != null)
            animator.SetTrigger("Hurt");
    }

    public void Die()
    {
        isDead = true;
        canMove = false;
        animator.SetTrigger("Die");
        FindObjectOfType<LevelManager>().Restart();
    }

    public void Respawn()
    {
        transform.position = respawnPoint;
        rb.velocity = Vector2.zero;
        isKnockback = false;
        knockbackTimer = 0f;
        knockbackDirection = 0;
        isDead = false;
        canMove = true;
        animator.Rebind();
        animator.Update(0f);
    }

    public void SetRespawnPoint(Vector2 point)
    {
        respawnPoint = point;
    }

    public void ResetPlayer()
    {
        isDead = false;
        canMove = true;
    }
}