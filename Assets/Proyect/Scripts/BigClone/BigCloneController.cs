using UnityEngine;

public class BigCloneController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private BigCloneStats stats;
    private Transform transformBigClone;
    public GameObject energyImage;
    public BigCloneMovement movement { get; private set; }
    private BigCloneWallDestroyer wallDestroyer;
    private WallContactDetector wallContactDetector;
    private WallDestructor wallDestructor;
    [SerializeField] private PerspectiveSwitch perspectiveSwitch;
    [SerializeField] private BigCloneAttack bigCloneAttack;


    private void Awake()
    {
        InitializeComponents();
        ApplySizeModifier();
        CinemachineSingleton.Instance.SetBigClone(transformBigClone);
        GameManager.Instance.SetBigCloneEnergy(energyImage);

        
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        transformBigClone = GetComponent<Transform>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        stats = new BigCloneStats();
        movement = new BigCloneMovement(rb, stats, spriteRenderer);
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
        if (!wallDestroyer.isAttacking)
        {
            wallDestroyer.CheckAndDestroyWall();

            if (wallDestroyer.wallContact.IsTouchingWall() && Input.GetKeyDown(KeyCode.Mouse0))
            {
                wallDestroyer.StartAttack(this); 
            }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallContactDetector.SetWallContact(collision.gameObject,true);
        }
    }


}
