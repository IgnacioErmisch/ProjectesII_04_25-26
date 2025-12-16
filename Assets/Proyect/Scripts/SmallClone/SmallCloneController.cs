using UnityEngine;

public class SmallCloneController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SmallCloneStats stats;
    private SmallCloneMovment movement;
    private SmallCloneDoubleJump doubleJump;
    private PerspectiveSwitch perspectiveSwitch;
    private SpriteRenderer spriteRenderer;
    private Transform transformSmallClone;

    public GameObject energyImage;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Edge Detection")]
    [SerializeField] private Transform edgeCheckFront;
    [SerializeField] private Transform edgeCheckBack;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float jumpMultiplier = 1f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private int jumpCounter = 0;

    public SmallCloneMovment Movement => movement;
    public SmallCloneDoubleJump DoubleJump => doubleJump;

    private void Awake()
    {
        InitializeComponents();
        ApplySizeModifier();
        CinemachineSingleton.Instance.SetSmallClone(transformSmallClone);
        GameManager.Instance.SetSmallCloneEnergy(energyImage);
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        transformSmallClone = GetComponent<Transform>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        stats = new SmallCloneStats(); 
        perspectiveSwitch = FindFirstObjectByType<PerspectiveSwitch>();      
        movement = new SmallCloneMovment(rb, stats, spriteRenderer,groundCheck, groundCheckRadius, groundLayer, edgeCheckFront, edgeCheckBack);
        doubleJump = new SmallCloneDoubleJump(rb, groundCheck, groundCheckRadius, groundLayer, jumpForce, jumpMultiplier, coyoteTime, maxJumps, jumpCounter);
    }

    private void ApplySizeModifier()
    {
        transform.localScale = Vector3.one * stats.SizeMultiplier;
    }

    private void Update()
    {
        bool canControl = !perspectiveSwitch.GetControllingPlayer(); 
        movement.Update();       
        doubleJump.Update(canControl);
    }

    private void FixedUpdate()
    {
        bool canControl = !perspectiveSwitch.GetControllingPlayer();     
        movement.Move(canControl);
        doubleJump.FixedUpdate();
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
            Gizmos.DrawLine(edgeCheckFront.position, edgeCheckFront.position + Vector3.down * 0.3f);
            Gizmos.DrawLine(edgeCheckBack.position, edgeCheckBack.position + Vector3.down * 0.3f);
        }
    }
}