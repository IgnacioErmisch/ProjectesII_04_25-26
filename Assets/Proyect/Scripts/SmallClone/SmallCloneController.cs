using UnityEngine;

public class SmallCloneController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SmallCloneStats stats;
    private SmallCloneMovment movement;
    private SmallCloneDoubleJump doubleJump;
    private CoyoteTimer coyoteTimer;
    private GroundChecker groundChecker;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    private void Awake()
    {
        InitializeComponents();
        ApplySizeModifier();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = new SmallCloneStats();   
        groundChecker = new GroundChecker(groundCheck, groundCheckRadius, groundLayer);
        movement = new SmallCloneMovment(rb, movement.speed, stats);
        doubleJump = new SmallCloneDoubleJump(groundChecker, coyoteTimer, stats.JumpMultiplier, doubleJump.maxJumps);
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
        movement.Move();
    }

}
