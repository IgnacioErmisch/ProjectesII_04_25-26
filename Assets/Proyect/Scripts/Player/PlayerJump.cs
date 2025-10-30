using UnityEngine;


public class GroundChecker
{
    private Transform groundCheck;
    private float checkRadius;
    private LayerMask groundLayer;

    public GroundChecker(Transform groundCheck, float checkRadius, LayerMask groundLayer)
    {
        this.groundCheck = groundCheck;
        this.checkRadius = checkRadius;
        this.groundLayer = groundLayer;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }
}


public class CoyoteTimer
{
    private float duration;
    private float counter;

    public CoyoteTimer(float duration)
    {
        this.duration = duration;
    }

    public void Update(bool isGrounded)
    {
        if (isGrounded)
        {
            counter = duration;
        }
        else
        {
            counter = counter - Time.deltaTime;
        }
    }

    public bool CanJump()
    {
        return counter > 0f;
    }
   
}


public class JumpHandler
{
    private Rigidbody2D rb;

    public JumpHandler(Rigidbody2D rb)
    {
        this.rb = rb;
    }

    public void Jump(float force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
    }
}


public class PlayerJump : MonoBehaviour
{
    
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private int maxJumps = 1;
    [SerializeField] private int jumpCounter = 0;


    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private GroundChecker groundChecker;
    [SerializeField] private CoyoteTimer coyoteTimer;
    [SerializeField] private JumpHandler jumpHandler;

    private void Awake()
    {
        groundChecker = new GroundChecker(groundCheck, groundCheckRadius, groundLayer);
        coyoteTimer = new CoyoteTimer(coyoteTime);
        jumpHandler = new JumpHandler(GetComponent<Rigidbody2D>());
    }

    private void Update()
    {
        bool isGrounded = groundChecker.IsGrounded();
        coyoteTimer.Update(isGrounded);

        if (isGrounded)
        {
            jumpCounter = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (coyoteTimer.CanJump())
            {
                jumpHandler.Jump(jumpForce);
                jumpCounter++;
            }
            
            else if (jumpCounter < maxJumps)
            {
                jumpHandler.Jump(jumpForce);
                jumpCounter++;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

}
