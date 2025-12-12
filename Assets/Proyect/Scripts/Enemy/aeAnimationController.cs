using UnityEngine;

public class aeAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AerialSentinelEnemy aerial;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        aerial = GetComponent<AerialSentinelEnemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        if (aerial.IsDead())
        {
            animator.SetTrigger("Die");
            animator.SetBool("IsWalking", false);
            enabled = false;
            return;
        }


        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        animator.SetBool("IsWalking", isMoving);
    }

    public void PlayAttackAnimation()
    {
        if (!aerial.IsDead())
        {
            animator.SetTrigger("Attack");
        }
    }

    public void PlayHitAnimation()
    {
        if (!aerial.IsDead())
        {
            animator.SetTrigger("TakeDamage");
        }
    }
}
