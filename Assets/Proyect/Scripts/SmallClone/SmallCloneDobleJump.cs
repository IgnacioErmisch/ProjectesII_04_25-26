using UnityEngine;

public class SmallCloneDoubleJump
{
    private Rigidbody2D rb;

    // Ground detection
    private Transform groundCheck;
    private float groundCheckRadius;
    private LayerMask groundLayer;

    // Jump settings
    private float jumpForce;
    private float jumpMultiplier;
    private int maxJumps;

    // Gravity controls
    private float normalGravityScale = 2.5f;
    private float fallGravityMultiplier = 2f;
    private float lowJumpMultiplier = 3f;
    private float maxFallSpeed = 20f;

    // Apex settings
    private float apexThreshold = 2f;
    private float apexHangTime = 0.1f;
    private float apexGravityMultiplier = 0.5f;

    // Jump cut
    private float jumpCutMultiplier = 0.5f;

    // Coyote time
    private float coyoteTime;
    private float coyoteCounter;

    // Jump buffer
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    // State
    private int jumpsRemaining;
    private bool wasGroundedLastFrame;
    private bool isAtApex;
    private float apexHangCounter;
    private bool jumpHeld;
    private bool jumpCut;
    public bool isJumping { get; private set; }
    public bool isGrounded { get; private set; }

    public SmallCloneDoubleJump(Rigidbody2D rb, Transform groundCheck, float groundCheckRadius,
                                LayerMask groundLayer, float jumpForce, float jumpMultiplier,
                                float coyoteTime, int maxJumps = 2)
    {
        this.rb = rb;
        this.groundCheck = groundCheck;
        this.groundCheckRadius = groundCheckRadius;
        this.groundLayer = groundLayer;
        this.jumpForce = jumpForce;
        this.jumpMultiplier = jumpMultiplier;
        this.coyoteTime = coyoteTime;
        this.maxJumps = maxJumps;
        this.jumpsRemaining = maxJumps;
        this.wasGroundedLastFrame = false;
        this.jumpBufferCounter = 0f;
        this.coyoteCounter = 0f;
    }

    public void Update(bool canControl)
    {
        isGrounded = CheckGround();

        // Coyote time
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            jumpsRemaining = maxJumps;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        // Reset jumps when landing
        if (isGrounded && !wasGroundedLastFrame)
        {
            OnLand();
        }

        wasGroundedLastFrame = isGrounded;

        // Jump buffer
        if (canControl)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            jumpHeld = Input.GetKey(KeyCode.Space);

            // Handle buffered jump
            if (jumpBufferCounter > 0f && CanJump())
            {
                Jump();
                jumpBufferCounter = 0f;
            }

            // Jump cut
            if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f && !jumpCut)
            {
                CutJump();
            }
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
            jumpHeld = false;
        }

        CheckApex();
    }

    public void FixedUpdate()
    {
        ApplyGravityModifiers();
        ClampFallSpeed();
    }

    private bool CheckGround()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public bool CanJump()
    {
        // First jump uses coyote time, additional jumps don't need it
        if (jumpsRemaining == maxJumps)
        {
            return coyoteCounter > 0f;
        }
        else
        {
            return jumpsRemaining > 0;
        }
    }

    public void Jump()
    {
        // Reset vertical velocity before jumping
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpMultiplier);

        jumpsRemaining--;
        coyoteCounter = 0f;
        isJumping = true;
        jumpCut = false;
    }

    private void CutJump()
    {
        jumpCut = true;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
    }

    private void CheckApex()
    {
        if (Mathf.Abs(rb.linearVelocity.y) < apexThreshold && !isGrounded)
        {
            if (!isAtApex)
            {
                isAtApex = true;
                apexHangCounter = apexHangTime;
            }
        }
        else
        {
            isAtApex = false;
        }

        if (isAtApex && apexHangCounter > 0f)
        {
            apexHangCounter -= Time.deltaTime;
        }
    }

    private void ApplyGravityModifiers()
    {
        // Apex hang
        if (isAtApex && apexHangCounter > 0f)
        {
            rb.gravityScale = normalGravityScale * apexGravityMultiplier;
        }
        // Falling
        else if (rb.linearVelocity.y < 0f)
        {
            rb.gravityScale = normalGravityScale * fallGravityMultiplier;
        }
        // Rising but not holding jump (short jump)
        else if (rb.linearVelocity.y > 0f && !jumpHeld)
        {
            rb.gravityScale = normalGravityScale * lowJumpMultiplier;
        }
        // Normal gravity
        else
        {
            rb.gravityScale = normalGravityScale;
        }
    }

    private void ClampFallSpeed()
    {
        if (rb.linearVelocity.y < -maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -maxFallSpeed);
        }
    }

    private void OnLand()
    {
        isJumping = false;
        jumpCut = false;
    }

    public int GetJumpsRemaining()
    {
        return jumpsRemaining;
    }

    public bool IsAtApex()
    {
        return isAtApex;
    }

    public float GetVerticalVelocity()
    {
        return rb != null ? rb.linearVelocity.y : 0f;
    }
}