using UnityEngine;

public class SmallCloneAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SmallCloneController smallCloneController;

    private SmallCloneMovment movement;
    private SmallCloneDoubleJump doubleJump;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (smallCloneController == null)
        {
            smallCloneController = GetComponent<SmallCloneController>();
        }

        if (smallCloneController != null)
        {
            movement = smallCloneController.Movement;
            doubleJump = smallCloneController.DoubleJump;
        }
    }

    void Update()
    {
        PlayWalkAnimation();
        PlayJumpAnimation();
    }

    private void PlayWalkAnimation()
    {
<<<<<<< HEAD

      
=======
        if (movement != null && movement.isMoving)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
>>>>>>> feature/small-animation
    }

    private void PlayJumpAnimation()
    {
<<<<<<< HEAD

       
=======
        if (doubleJump != null && doubleJump.isJumping)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
>>>>>>> feature/small-animation
    }
}
