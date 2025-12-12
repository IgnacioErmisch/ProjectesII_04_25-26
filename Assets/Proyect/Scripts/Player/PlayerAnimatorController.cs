using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerCombatController playerCombatController;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayWalkAnimation();
        PlayJumpAnimation();
        PlayDeathAnimation();
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

    private void PlayDeathAnimation()
    {
        if (playerCombatController.GetCurrentHealth() <= 0)
        {
            animator.SetBool("isDeath", true);

        }
        else
        {
            animator.SetBool("isDeath", false);
        }
    }

    public void ResetAnimations()
    {
        animator.SetBool("isDeath", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isJumping", false);
    }
}
