using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float airDeceleration;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Edge Detection")]
    [SerializeField] private Transform edgeCheckFront;
    [SerializeField] private Transform edgeCheckBack;
    [SerializeField] private float edgeCheckDistance = 0.3f;
    [SerializeField] private float edgeClampSpeed = 2f;

    [Header("References")]
    [SerializeField] private PerspectiveSwitch perspectiveSwitch;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform cloneSpawnerPoint;
    [SerializeField] private Transform cloneSpawnerPointSecond;
    [SerializeField] private PlayerCombatController playerCombatController;

    private SoundManager soundManager;
    public float horizontal { get; private set; }
    public bool isMoving { get; private set; }
    public bool isGrounded { get; private set; }
   
    private bool isBeingLaunched = false;
    private float launchControlDisableTime = 0f;

    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    private float currentSpeed;
    private bool wasGrounded;
    private bool isOnEdge;
    private bool wasMoving;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Awake()
    {
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    void Update()
    {
        wasGrounded = isGrounded;
        isGrounded = CheckGround();

        if (perspectiveSwitch.GetControllingPlayer() && !playerCombatController.IsDead())
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            horizontal = 0f;
        }

        bool isCurrentlyMoving = isGrounded && isMoving && Mathf.Abs(currentSpeed) > 0.1f;

        if (isCurrentlyMoving && !wasMoving)
        {
            soundManager.PlayLoop(soundManager.movementP);
        }
        else if (!isCurrentlyMoving && wasMoving)
        {
            soundManager.StopLoop();
        }

        wasMoving = isCurrentlyMoving;

        CheckEdge();
    }

    void FixedUpdate()
    {
        
        if (isBeingLaunched && Time.time < launchControlDisableTime)
            return;
        

        if (isBeingLaunched && Time.time >= launchControlDisableTime)
        {
            isBeingLaunched = false;
        }

        if (perspectiveSwitch.GetControllingPlayer() && !playerCombatController.IsDead())
        {
            ApplyMovement();
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
            rb2D.linearVelocity = new Vector2(currentSpeed, rb2D.linearVelocity.y);
        }

        if (isOnEdge && isGrounded)
        {
            ClampToEdge();
        }
    }

    private void ApplyMovement()
    {
        float targetSpeed = horizontal * maxSpeed;
        float accel = isGrounded ? acceleration : airAcceleration;
        float decel = isGrounded ? deceleration : airDeceleration;

        if (Mathf.Abs(horizontal) > 0.01f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accel * Time.fixedDeltaTime);
            isMoving = true;
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, decel * Time.fixedDeltaTime);
            isMoving = Mathf.Abs(currentSpeed) > 0.1f;
        }

        rb2D.linearVelocity = new Vector2(currentSpeed, rb2D.linearVelocity.y);

        if (currentSpeed > 0.1f && !facingRight)
        {
            Flip();
        }
        else if (currentSpeed < -0.1f && facingRight)
        {
            Flip();
        }
    }

    private bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void CheckEdge()
    {
        if (!isGrounded)
        {
            isOnEdge = false;
            return;
        }

        Vector2 frontCheck = edgeCheckFront.position;
        Vector2 backCheck = edgeCheckBack.position;
        bool frontHasGround = Physics2D.Raycast(frontCheck, Vector2.down, edgeCheckDistance, groundLayer);
        bool backHasGround = Physics2D.Raycast(backCheck, Vector2.down, edgeCheckDistance, groundLayer);
        isOnEdge = !frontHasGround || !backHasGround;
    }

    private void ClampToEdge()
    {
        if (Mathf.Abs(rb2D.linearVelocity.x) > edgeClampSpeed)
        {
            float clampedVelocity = Mathf.Sign(rb2D.linearVelocity.x) * edgeClampSpeed;
            rb2D.linearVelocity = new Vector2(clampedVelocity, rb2D.linearVelocity.y);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
        Vector3 attackScale = attackPoint.localScale;
        attackScale.x *= -1;
        attackPoint.localScale = attackScale;

        cloneSpawnerPoint.localPosition = new Vector3(-cloneSpawnerPoint.localPosition.x, cloneSpawnerPoint.localPosition.y, cloneSpawnerPoint.localPosition.z);
        cloneSpawnerPointSecond.localPosition = new Vector3(-cloneSpawnerPointSecond.localPosition.x, cloneSpawnerPointSecond.localPosition.y, cloneSpawnerPointSecond.localPosition.z);
    }

    public void DisableControlForLaunch(float duration)
    {
        isBeingLaunched = true;
        launchControlDisableTime = Time.time + duration;
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public bool IsOnEdge()
    {
        return isOnEdge;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (edgeCheckFront != null && edgeCheckBack != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(edgeCheckFront.position, edgeCheckFront.position + Vector3.down * edgeCheckDistance);
            Gizmos.DrawLine(edgeCheckBack.position, edgeCheckBack.position + Vector3.down * edgeCheckDistance);
        }
    }
}