using UnityEngine;

public class SmallCloneDoubleJump
{
    public JumpHandler jumpHandler;
    public GroundChecker groundChecker;
    public CoyoteTimer coyoteTimer;
    public float jumpForce;
    public float jumpMultiplier;
    public int maxJumps;
    public int jumpsRemaining;
    public bool wasGroundedLastFrame;

    public SmallCloneDoubleJump(JumpHandler jumpHandler, GroundChecker groundChecker, CoyoteTimer coyoteTimer, float jumpForce, float jumpMultiplier, int maxJumps = 2)
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
