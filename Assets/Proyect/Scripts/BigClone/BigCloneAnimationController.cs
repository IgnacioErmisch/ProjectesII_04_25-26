using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BigCloneAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BigCloneController bigCloneController;
    [SerializeField] private BigCloneWallDestroyer bigCloneWallDestroyer;
    [SerializeField] private BigCloneAttack bigCloneAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        Initialize();
    }

    
    void Update()
    {
        PlayWalkAnimation();
        PlayAttackAnimation();
        
    }

    private void Initialize()
    {

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
    private void PlayAttackAnimation()
    {

        if (bigCloneAttack.isDashing)
        {

            animator.SetBool("isAttacking", true);

        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }


}
