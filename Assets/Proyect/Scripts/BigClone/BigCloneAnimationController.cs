using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BigCloneAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BigCloneController bigCloneController;
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

    
    void Update()
    {
        PlayWalkAnimation();
        
    }

    private void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        stats = new BigCloneStats();
        wallContactDetector = new WallContactDetector();
        wallDestructor = new WallDestructor();
        
    }

    private void PlayWalkAnimation()
    {
      
        if (bigCloneController.movement.GetMove())
        {
          
            animator.SetBool("isWalking", true);
            
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

   
}
