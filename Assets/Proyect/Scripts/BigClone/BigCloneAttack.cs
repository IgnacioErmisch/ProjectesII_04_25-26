using UnityEngine;

public class BigCloneAttack : MonoBehaviour
{

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDamage = 999f; 
    [SerializeField] private float dashKnockbackForce = 15f;
    [SerializeField] private LayerMask enemyLayer; 
    [SerializeField] private Rigidbody2D rb;

    public bool isDashing = false;
    private bool canDash = true;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                StopDash();
            }
        }

        if (!canDash)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                canDash = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * dashSpeed, rb.linearVelocity.y);
        }
    }

    void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;
    }

    void StopDash()
    {
        isDashing = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, rb.linearVelocity.y);
    }

 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

                if (damageable != null && !damageable.IsDead())
                {
                   
                    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                    damageable.TakeDamage(dashDamage, knockbackDirection * dashKnockbackForce);

                    
                }
        }
    }
}


