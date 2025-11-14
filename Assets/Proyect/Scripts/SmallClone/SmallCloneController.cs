using UnityEngine;

public class SmallCloneController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SmallCloneStats stats;
    private SmallCloneMovment movement;
    private SmallCloneDoubleJump doubleJump;
    private PerspectiveSwitch perspectiveSwitch;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
  
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float jumpMultiplier = 1f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float coyoteTime = 0.1f;

    private void Awake()
    {
        InitializeComponents();
        ApplySizeModifier();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        stats = new SmallCloneStats();
        movement = new SmallCloneMovment(rb, stats, spriteRenderer);      
        GroundChecker groundChecker = new GroundChecker(groundCheck, groundCheckRadius, groundLayer);
        CoyoteTimer coyoteTimer = new CoyoteTimer(coyoteTime);
        JumpHandler jumpHandler = new JumpHandler(rb);
        perspectiveSwitch = FindFirstObjectByType<PerspectiveSwitch>();
        doubleJump = new SmallCloneDoubleJump(jumpHandler, groundChecker, coyoteTimer, jumpForce, jumpMultiplier, maxJumps);
    }

    private void ApplySizeModifier()
    {
        transform.localScale = Vector3.one * stats.SizeMultiplier;
    }

    private void Update()
    {
        doubleJump.Update();
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && doubleJump.CanJump())
        {
            doubleJump.Jump();
        }
    }

    private void FixedUpdate()
    {
       
        if (!perspectiveSwitch.GetControllingPlayer())
        {
            movement.Move();
        }
        else
        {
            rb.linearVelocity = Vector2.zero; 
        }
    }
}