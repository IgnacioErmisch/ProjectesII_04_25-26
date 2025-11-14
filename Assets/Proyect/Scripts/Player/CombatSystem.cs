using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, Vector2 knockbackDirection);
    bool IsDead();
    float GetCurrentHealth();
}

public interface IAttacker
{
    void Attack();
    float GetAttackDamage();
    float GetAttackRange();
}

public interface IDetectionSystem
{
    bool DetectTarget();
    Transform GetTarget();
}

public interface IMovementSystem
{
    void Move(Vector2 direction);
    void Stop();
}

public interface IEnemyState
{
    void Enter();
    void Update();
    void Exit();
}

public class HealthSystem : IDamageable
{
    private float maxHealth;
    private float currentHealth;
    private bool isDead;

    public event System.Action<float> OnHealthChanged;
    public event System.Action OnDeath;

    public HealthSystem(float maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
        this.isDead = false;
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}

public class KnockbackSystem
{
    private Rigidbody2D rb;
    private float knockbackForce;
    private float knockbackDuration;
    private float knockbackTimer;
    private bool isKnockedBack;

    public KnockbackSystem(Rigidbody2D rb, float knockbackForce, float knockbackDuration)
    {
        this.rb = rb;
        this.knockbackForce = knockbackForce;
        this.knockbackDuration = knockbackDuration;
        this.knockbackTimer = 0f;
        this.isKnockedBack = false;
    }

    public void ApplyKnockback(Vector2 direction)
    {
        if (rb == null) return;

        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
        rb.linearVelocity = direction.normalized * knockbackForce;
    }

    public void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    public bool IsKnockedBack()
    {
        return isKnockedBack;
    }
}


public class RadiusDetectionSystem : IDetectionSystem
{
    private Transform owner;
    private float detectionRadius;
    private LayerMask targetLayer;
    private Transform currentTarget;

    public RadiusDetectionSystem(Transform owner, float detectionRadius, LayerMask targetLayer)
    {
        this.owner = owner;
        this.detectionRadius = detectionRadius;
        this.targetLayer = targetLayer;
    }

    public bool DetectTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(owner.position, detectionRadius, targetLayer);

        if (hits.Length > 0)
        {
            currentTarget = hits[0].transform;
            return true;
        }

        currentTarget = null;
        return false;
    }

    public Transform GetTarget() => currentTarget;

    public void DrawGizmos()
    {
        if (owner != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(owner.position, detectionRadius);
        }
    }
}


public class PlayerMeleeAttack : IAttacker
{
    private Transform attackPoint;
    private float attackDamage;
    private float attackRange;
    private float attackCooldown;
    private float lastAttackTime;
    private LayerMask enemyLayer;
    private float knockbackForce;

    public event System.Action OnAttackPerformed;

    public PlayerMeleeAttack(Transform attackPoint, float attackDamage, float attackRange,
                             float attackCooldown, LayerMask enemyLayer, float knockbackForce)
    {
        this.attackPoint = attackPoint;
        this.attackDamage = attackDamage;
        this.attackRange = attackRange;
        this.attackCooldown = attackCooldown;
        this.enemyLayer = enemyLayer;
        this.knockbackForce = knockbackForce;
        this.lastAttackTime = -attackCooldown;
    }

    public void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        OnAttackPerformed?.Invoke();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null && !damageable.IsDead())
            {
                Vector2 knockbackDirection = (enemy.transform.position - attackPoint.position).normalized;
                knockbackDirection.y = 0.3f; 

                damageable.TakeDamage(attackDamage, knockbackDirection * knockbackForce);
            }
        }
    }

    public float GetAttackDamage() => attackDamage;
    public float GetAttackRange() => attackRange;

    public void DrawGizmos()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}

public class EnemyStateContext
{
    public Transform transform;
    public IDetectionSystem detectionSystem;
    public IMovementSystem movementSystem;
    public IAttacker attackSystem;
    public float chaseSpeed;
    public float attackRange;
    public float attackCooldown;
    public float lastAttackTime;
}


public class PatrolState : IEnemyState
{
    private EnemyStateContext context;
    private IMovementSystem patrolMovement;

    public PatrolState(EnemyStateContext context, IMovementSystem patrolMovement)
    {
        this.context = context;
        this.patrolMovement = patrolMovement;
    }

    public void Enter()
    {
        
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        patrolMovement?.Stop();
    }
}


public class ChaseState : IEnemyState
{
    private EnemyStateContext context;

    public ChaseState(EnemyStateContext context)
    {
        this.context = context;
    }

    public void Enter()
    {
        
    }

    public void Update()
    {
        Transform target = context.detectionSystem.GetTarget();
        if (target != null)
        {
            Vector2 direction = (target.position - context.transform.position).normalized;
            context.movementSystem.Move(direction);
        }
    }

    public void Exit()
    {
        context.movementSystem.Stop();
    }
}


public class AttackState : IEnemyState
{
    private EnemyStateContext context;

    public AttackState(EnemyStateContext context)
    {
        this.context = context;
    }

    public void Enter()
    {
        context.movementSystem.Stop();
    }

    public void Update()
    {
        if (Time.time - context.lastAttackTime >= context.attackCooldown)
        {
            context.attackSystem?.Attack();
            context.lastAttackTime = Time.time;
        }
    }

    public void Exit()
    {
    
    }
}