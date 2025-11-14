using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BigCloneAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BigCloneMovement bigCloneMovement;
    [SerializeField] private BigCloneWallDestroyer bigCloneWallDestroyer;
    private BigCloneWallDestroyer wallDestroyer;
    private WallContactDetector wallContactDetector;
    private WallDestructor wallDestructor;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private BigCloneStats stats;
    void Start()
    {
        animator = GetComponent<Animator>();
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        PlayWalkAnimation();
        PlayAttackAnimation();
    }

    private void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        stats = new BigCloneStats();
        bigCloneMovement = new BigCloneMovement(rb, stats, spriteRenderer);
        wallContactDetector = new WallContactDetector();
        wallDestructor = new WallDestructor();
        bigCloneWallDestroyer = new BigCloneWallDestroyer(wallContactDetector, wallDestructor);
    }

    private void PlayWalkAnimation()
    {
        if (bigCloneMovement.GetMove())
        {
            animator.SetBool("isWalking", true);
            
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void PlayAttackAnimation()
    {
        if (bigCloneWallDestroyer.isAttacking)
        {
            animator.SetBool("isAttacking", true);

        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
