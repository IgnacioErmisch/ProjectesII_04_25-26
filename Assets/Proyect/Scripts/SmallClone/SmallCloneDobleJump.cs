using UnityEngine;

public class CloneDoubleJump
{
    private JumpHandler jumpHandler;
    private GroundChecker groundChecker;
    private CoyoteTimer coyoteTimer;
    private float jumpForce;
    private float jumpMultiplier;
    private int maxJumps;
    private int jumpsRemaining;
    private bool wasGroundedLastFrame;

    public CloneDoubleJump(JumpHandler jumpHandler, GroundChecker groundChecker, CoyoteTimer coyoteTimer, float jumpForce, float jumpMultiplier, int maxJumps = 2)
    {
        this.jumpHandler = jumpHandler;
        this.groundChecker = groundChecker;
        this.coyoteTimer = coyoteTimer;
        this.jumpForce = jumpForce;
        this.jumpMultiplier = jumpMultiplier;
        this.maxJumps = maxJumps;
        this.jumpsRemaining = maxJumps;
        this.wasGroundedLastFrame = false;
    }

    public void Update()
    {
        bool isGrounded = groundChecker.IsGrounded();
        coyoteTimer.Update(isGrounded);

        
        if (isGrounded && !wasGroundedLastFrame)
        {
            jumpsRemaining = maxJumps;
        }

        wasGroundedLastFrame = isGrounded;
    }

    public bool CanJump()
    {
        
        if (jumpsRemaining == maxJumps)
        {
            return coyoteTimer.CanJump();
        }
        else
        {
            return jumpsRemaining > 0;
        }
    }

    public void Jump()
    {
        jumpHandler.Jump(jumpForce * jumpMultiplier);
        jumpsRemaining--;
    }

    public int GetJumpsRemaining()
    {
        return jumpsRemaining;
    }
}
