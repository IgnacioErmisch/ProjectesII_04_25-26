using UnityEngine;

public class SmallCloneAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SmallCloneController smallCloneController;
    [SerializeField] private SmallCloneDoubleJump smallCloneDoubleJump;


    void Start()
    {
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        PlayWalkAnimation();
        PlayJumpAnimation();

    }


    private void PlayWalkAnimation()
    {

        if (smallCloneController.movement.GetMove())
        {

            animator.SetBool("isWalking", true);

        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void PlayJumpAnimation()
    {

        if (smallCloneDoubleJump.isJumping)
        {

            animator.SetBool("isJumping", true);

        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

}
