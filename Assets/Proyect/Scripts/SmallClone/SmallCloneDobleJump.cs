using UnityEngine;

public class SmallCloneDoubleJump
{
    private Rigidbody2D rb;
    private Transform groundCheck;
    private float groundCheckRadius;
    private LayerMask groundLayer;
    private float jumpForce;
    private float jumpMultiplier;
    private int maxJumps;
    private float normalGravityScale = 2.5f;
    private float fallGravityMultiplier = 2f;
    private float lowJumpMultiplier = 3f;
    private float maxFallSpeed = 20f;
    private float apexThreshold = 2f;
    private float apexHangTime = 0.1f;
    private float apexGravityMultiplier = 0.5f;
    private float jumpCutMultiplier = 0.5f;
    private float coyoteTime;
    private float coyoteCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    private bool isAtApex;
    private float apexHangCounter;
    private bool jumpHeld;
    private bool jumpCut;
    private bool wasGrounded;
    private int jumpCounter = 0;
    public bool isJumping { get; private set; }
    public bool isGrounded { get; private set; }
    [SerializeField] private SoundManager soundManager;

    public SmallCloneDoubleJump(Rigidbody2D rb, Transform groundCheck, float groundCheckRadius,
                                LayerMask groundLayer, float jumpForce, float jumpMultiplier,
                                float coyoteTime, int maxJumps = 2, int jumpCounter = 0)
    {
        this.rb = rb;
        this.groundCheck = groundCheck;
        this.groundCheckRadius = groundCheckRadius;
        this.groundLayer = groundLayer;
        this.jumpForce = jumpForce;
        this.jumpMultiplier = jumpMultiplier;
        this.coyoteTime = coyoteTime;
        this.maxJumps = maxJumps;
        this.jumpBufferCounter = 0f;
        this.coyoteCounter = 0f;
        this.jumpCounter = 0;
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            this.soundManager = audioObject.GetComponent<SoundManager>();
        }
    }
   

    public void Update(bool canControl)
    {
        wasGrounded = isGrounded;
        isGrounded = CheckGround();

        if (isGrounded && !wasGrounded)
        {
            OnLand();
        }


        if (isGrounded && !isJumping)
        {
            coyoteCounter = coyoteTime;
            jumpCounter = 0;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        jumpHeld = Input.GetKey(KeyCode.Space);
        

        if (canControl)
        {

            if (jumpBufferCounter > 0f && CanJump())
            {
                PerformJump();
                jumpBufferCounter = 0f;
            }

            if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f && !jumpCut)
            {
                CutJump();
            }
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
        return jumpCounter < maxJumps && (isGrounded || coyoteCounter > 0f);
    }

    public void PerformJump()
    {
        
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpMultiplier);
        jumpCounter++;
        isJumping = true;
        jumpCut = false;
        soundManager.PlaySFX(soundManager.jump);
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
        
        if (isAtApex && apexHangCounter > 0f)
        {
            rb.gravityScale = normalGravityScale * apexGravityMultiplier;
        }
        
        else if (rb.linearVelocity.y < 0f)
        {
            rb.gravityScale = normalGravityScale * fallGravityMultiplier;
        }
        
        else if (rb.linearVelocity.y > 0f && !jumpHeld)
        {
            rb.gravityScale = normalGravityScale * lowJumpMultiplier;
        }
        
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

    public bool IsAtApex()
    {
        return isAtApex;
    }

    public float GetVerticalVelocity()
    {
        return rb != null ? rb.linearVelocity.y : 0f;
    }
}