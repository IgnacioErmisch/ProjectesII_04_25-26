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
    private float apexGravityMultiplier = 1.5f;
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


    private CloneGravity cloneGravity;

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

  
        this.cloneGravity = rb.GetComponent<CloneGravity>();

        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            this.soundManager = audioObject.GetComponent<SoundManager>();
        }
    }

    public void Update(bool canControl, ParticleSystem particleLand, ParticleSystem particleJump)
    {
        wasGrounded = isGrounded;
        isGrounded = CheckGround();

        if (isGrounded && !wasGrounded)
        {
            OnLand(particleLand);
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
                PerformJump(particleJump);
                jumpBufferCounter = 0f;
            }

            if (Input.GetKeyUp(KeyCode.Space) && IsMovingAwayFromGravity() && !jumpCut)
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

    public void PerformJump(ParticleSystem particleJump)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        float actualJumpForce = jumpForce * jumpMultiplier;
        if (cloneGravity != null && cloneGravity.IsInverted())
        {
            actualJumpForce = -actualJumpForce; 
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, actualJumpForce);
        jumpCounter++;
        isJumping = true;
        jumpCut = false;

        if (particleJump != null)
        {
            particleJump.Play();
        }
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
        float gravityMultiplier = cloneGravity != null ? cloneGravity.GetGravityMultiplier() : 1f;
        float baseGravity = normalGravityScale * gravityMultiplier;

     
        if (isAtApex && apexHangCounter > 0f)
        {
            rb.gravityScale = baseGravity * apexGravityMultiplier;
        }
      
        else if (IsMovingTowardGravity())
        {
            rb.gravityScale = baseGravity * fallGravityMultiplier;
        }
     
        else if (IsMovingAwayFromGravity() && !jumpHeld)
        {
            rb.gravityScale = baseGravity * lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void ClampFallSpeed()
    {
        float currentMaxSpeed = maxFallSpeed;

        
        if (cloneGravity != null && cloneGravity.IsInverted())
        {
            if (rb.linearVelocity.y > currentMaxSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, currentMaxSpeed);
            }
        }
        
        else
        {
            if (rb.linearVelocity.y < -currentMaxSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -currentMaxSpeed);
            }
        }
    }

    private bool IsMovingTowardGravity()
    {
        if (cloneGravity != null && cloneGravity.IsInverted())
        {
            return rb.linearVelocity.y > 0f; 
        }
        return rb.linearVelocity.y < 0f; 
    }
    private bool IsMovingAwayFromGravity()
    {
        if (cloneGravity != null && cloneGravity.IsInverted())
        {
            return rb.linearVelocity.y < 0f; 
        }
        return rb.linearVelocity.y > 0f; 
    }

    public void OnLand(ParticleSystem particleLand)
    {
        isJumping = false;
        jumpCut = false;
        if (particleLand != null)
        {
            particleLand.Play();
        }
    }

    public bool Landed()
    {
        return wasGrounded == false && isGrounded == true;
    }

    public bool IsJumping()
    {
        return isJumping;
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