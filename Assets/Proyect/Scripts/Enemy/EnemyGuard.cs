using UnityEngine;

public class BasicGuardEnemy : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 50f;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private GameObject leftLimit;
    [SerializeField] private GameObject rightLimit;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float chaseDuration = 2f;

    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private Transform attackPoint;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float knockbackDuration = 0.4f;

    [Header("Animation")]
    [SerializeField] private Animator animator;  
    private HealthSystem healthSystem;
    private RadiusDetectionSystem detectionSystem;
    private KnockbackSystem knockbackSystem;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; 
    private enum State { Patrol, Chase, Attack, Dead }
    private State currentState = State.Patrol;
    private bool movingRight = true; 
    private float chaseTimer;
    private Transform playerTarget;
    private float lastAttackTime;

    private void Awake()
    {
        InitializeSystems();
    }

   

    private void InitializeSystems()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthSystem = new HealthSystem(maxHealth);
        healthSystem.OnDeath += HandleDeath;       
        detectionSystem = new RadiusDetectionSystem(transform, detectionRadius, playerLayer);     
        knockbackSystem = new KnockbackSystem(rb, knockbackForce, knockbackDuration);
     
        if (rb != null)
        {
            rb.gravityScale = 3f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Update()
    {
        if (healthSystem.IsDead()) return;

        knockbackSystem.Update();

        if (knockbackSystem.IsKnockedBack()) return;
    
        switch (currentState)
        {
            case State.Patrol:
                UpdatePatrol();
                break;
            case State.Chase:
                UpdateChase();
                break;
            case State.Attack:
                UpdateAttack();
                break;
        }
    }

   

    private void UpdatePatrol()
    {
        
        Patrol();
    
        if (detectionSystem.DetectTarget())
        {
            playerTarget = detectionSystem.GetTarget();
            TransitionToChase();
        }
    }

    private void Patrol()
    {
        if (leftLimit == null || rightLimit == null) return;

        Vector2 velocity = rb.linearVelocity;

        if (movingRight)
        {
            velocity.x = patrolSpeed;
            FlipSprite(false);

            
            if (transform.position.x >= rightLimit.transform.position.x)
            {
                movingRight = false;
            }
        }
        else
        {
            velocity.x = -patrolSpeed;
            FlipSprite(true);

            
            if (transform.position.x <= leftLimit.transform.position.x)
            {
                movingRight = true;
            }
        }

        rb.linearVelocity = velocity;
    }

    

    private void TransitionToChase()
    {
        currentState = State.Chase;
        chaseTimer = chaseDuration;
    }

    private void UpdateChase()
    {
       
        chaseTimer -= Time.deltaTime;

        if (playerTarget == null || chaseTimer <= 0)
        {
            
            currentState = State.Patrol;
            playerTarget = null;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);
        

        if (distanceToPlayer <= attackRange)
        {
           
            TransitionToAttack();
            return;
        }

        ChasePlayer();

        if (detectionSystem.DetectTarget())
        {
            chaseTimer = chaseDuration;
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (playerTarget.position - transform.position).normalized;
        Vector2 velocity = rb.linearVelocity;
        velocity.x = direction.x * chaseSpeed;
        rb.linearVelocity = velocity;
        FlipSprite(direction.x < 0);
    }

    

    private void TransitionToAttack()
    {
        currentState = State.Attack;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); 
    }

    private void UpdateAttack()
    {
       
        if (playerTarget == null)
        {
           
            currentState = State.Patrol;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);     

        if (distanceToPlayer > attackRange * 1.2f)
        {
            
            TransitionToChase();
            return;
        }
     

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        lastAttackTime = Time.time;
       
        if (attackPoint == null) attackPoint = transform;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && !damageable.IsDead())
            {
                Vector2 knockbackDirection = (hit.transform.position - transform.position).normalized;
                damageable.TakeDamage(attackDamage, knockbackDirection);
               
            }
        }
    }


    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        if (healthSystem.IsDead()) return;

        healthSystem.TakeDamage(damage, knockbackDirection);
        knockbackSystem.ApplyKnockback(knockbackDirection);        
    }

    public bool IsDead() => healthSystem.IsDead();
    public float GetCurrentHealth() => healthSystem.GetCurrentHealth();



    private void HandleDeath()
    {
        currentState = State.Dead;
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;  
        Destroy(gameObject, 2f);
    }

   

   

    private void FlipSprite(bool flipLeft)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = flipLeft;
        }
    }

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        
        Gizmos.color = Color.yellow;
        Vector3 attackPos = attackPoint != null ? attackPoint.position : transform.position;
        Gizmos.DrawWireSphere(attackPos, attackRange);

       
        if (leftLimit != null && rightLimit != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(leftLimit.transform.position, rightLimit.transform.position);
        }
    }

    
}