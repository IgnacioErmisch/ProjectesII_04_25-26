using UnityEngine;

public class BigCloneController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BigCloneStats stats;
    private BigCloneMovement movement;
    private BigCloneWallDestroyer wallDestroyer;
    private WallContactDetector wallContactDetector;
    private WallDestructor wallDestructor;
    [SerializeField] private PerspectiveSwitch perspectiveSwitch;

    private void Awake()
    {
        InitializeComponents();
        ApplySizeModifier();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = new BigCloneStats();
        movement = new BigCloneMovement(rb, stats);
        wallContactDetector = new WallContactDetector();
        wallDestructor = new WallDestructor();
        wallDestroyer = new BigCloneWallDestroyer( wallContactDetector, wallDestructor);
        perspectiveSwitch = FindFirstObjectByType<PerspectiveSwitch>();

    }

    private void ApplySizeModifier()
    {
        transform.localScale = Vector3.one * stats.SizeMultiplier;
    }
    private void Update()
    {
        wallDestroyer.CheckAndDestroyWall();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallContactDetector.SetWallContact(collision.gameObject,true);
        }
    }
}
