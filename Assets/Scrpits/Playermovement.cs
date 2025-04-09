using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 8f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    [Header("跳跃参数")]
    public float jumpForce = 12f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    [Header("墙体互动参数")]
    public float wallSlideSpeed = 3f;
    public float wallPushForce = 2f;
    public float wallCheckDistance = 0.5f;
    public float wallStickTime = 0.1f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private float velocityXSmoothing;
    private Vector3 initialScale;
    private float wallDirection;
    private float wallStickCounter;
    private bool isWallSliding;

    private InventorySystem inventory;
    private InteractionSystem interaction;
    public bool isDead = false; // 可按需启用

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;

        inventory = FindObjectOfType<InventorySystem>();
        interaction = FindObjectOfType<InteractionSystem>();
    }

    void Update()
    {
        if (!CanMoveOrInteract())
        {
            moveInput = 0f;
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        CheckGrounded();
        CheckWall();
        HandleJump();
        FlipCharacter();
    }

    void FixedUpdate()
    {
        if (!CanMoveOrInteract()) return;

        HandleMovement();
        HandleWallSlide();
    }

    bool CanMoveOrInteract()
    {
        bool can = true;

        if (interaction != null && interaction.isExamining)
            can = false;
        if (inventory != null && inventory.isOpen)
            can = false;
        if (isDead)
            can = false;

        return can;
    }

    void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            groundCheckRadius,
            groundLayer
        );
        isGrounded = hit.collider != null && hit.normal.y > 0.7f;
    }

    void CheckWall()
    {
        Vector2 checkDirection = new Vector2(transform.localScale.x > 0 ? 1 : -1, 0);
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            checkDirection,
            wallCheckDistance,
            groundLayer
        );

        isTouchingWall = hit.collider != null && !isGrounded;
        wallDirection = checkDirection.x;

        if (isTouchingWall)
        {
            wallStickCounter = wallStickTime;
        }
        else
        {
            wallStickCounter -= Time.deltaTime;
        }

        isWallSliding = wallStickCounter > 0 && !isGrounded;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (isWallSliding)
            {
                rb.velocity = new Vector2(-wallDirection * moveSpeed, jumpForce);
                wallStickCounter = 0;
            }
        }
    }

    void FlipCharacter()
    {
        if (moveInput > 0)
        {
            transform.localScale = initialScale;
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }
    }

    void HandleMovement()
    {
        if (isWallSliding)
        {
            moveInput = 0;
        }

        float targetVelocityX = moveInput * moveSpeed;
        float smoothTime = (Mathf.Abs(moveInput) > 0.1f) ? 1f / acceleration : 1f / deceleration;
        float smoothedVelocityX = Mathf.SmoothDamp(
            rb.velocity.x,
            targetVelocityX,
            ref velocityXSmoothing,
            smoothTime
        );

        rb.velocity = new Vector2(smoothedVelocityX, rb.velocity.y);
    }

    void HandleWallSlide()
    {
        if (isWallSliding)
        {
            float currentSlideSpeed = Mathf.Max(-wallSlideSpeed, rb.velocity.y);
            rb.velocity = new Vector2(rb.velocity.x, currentSlideSpeed - wallPushForce * Time.deltaTime);

            if (Mathf.Sign(moveInput) == Mathf.Sign(wallDirection))
            {
                rb.velocity += Vector2.down * wallPushForce * Time.deltaTime;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius);

        Gizmos.color = Color.blue;
        Vector2 wallDir = new Vector2(transform.localScale.x > 0 ? 1 : -1, 0);
        Gizmos.DrawRay(transform.position, wallDir * wallCheckDistance);
    }
}