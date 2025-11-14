using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombatController combatController;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayWalkAnimation();
        PlayAttackAnimation();
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

    private void PlayAttackAnimation()
    {
        if (combatController.isAttacking)
        {
            animator.SetBool("isAttacking", true);

        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
