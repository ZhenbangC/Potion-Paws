using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private float velocityXSmoothing;
    private Vector2 currentVelocity;
    private Vector3 initialScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

       
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        initialScale = transform.localScale;

    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }

    }

    void FixedUpdate()
    {
        float targetVelocityX = moveInput * moveSpeed;
        float smoothTime = (moveInput != 0) ? 1f / acceleration : 1f / deceleration;
        float smoothedVelocityX = Mathf.SmoothDamp(rb.velocity.x, targetVelocityX, ref velocityXSmoothing, smoothTime);

        rb.velocity = new Vector2(smoothedVelocityX, rb.velocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

