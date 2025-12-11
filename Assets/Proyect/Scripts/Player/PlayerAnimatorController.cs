using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerJump playerJump;
   
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayWalkAnimation();
        PlayJumpAnimation();
    }

    private void PlayWalkAnimation()
    {
        if (playerMovement.isMoving)
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
        if (playerJump.isJumping)
        {
            animator.SetBool("isJumping", true);

        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }
}
