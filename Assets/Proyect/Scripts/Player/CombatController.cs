using System.Collections;
using UnityEditor;
using UnityEngine;


public class PlayerCombatController : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float knockbackForce = 10f;

    [Header("Defense Settings")]
    [SerializeField] private float playerKnockbackForce = 5f;
    [SerializeField] private float playerKnockbackDuration = 0.3f;
    [SerializeField] private float invulnerabilityDuration = 1f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string attackAnimationTrigger = "Attack";

    
    private HealthSystem healthSystem;
    private PlayerMeleeAttack meleeAttack;
    private KnockbackSystem knockbackSystem;
    private Rigidbody2D rb;
   
    private float lastDamageTime;
    private bool isInvulnerable;
    public bool isAttacking;
    [SerializeField] private bool canRegenerate = true;
    [SerializeField] private float regenerationRate = 5f; 
    [SerializeField] private float regenerationDelay = 3f; 

    
    public event System.Action OnPlayerDeath;
    public event System.Action<float, float> OnHealthChanged; 

    private void Awake()
    {
        InitializeSystems();
    }

    private void Start()
    {
       
        OnHealthChanged?.Invoke(healthSystem.GetCurrentHealth(), healthSystem.GetMaxHealth());
    }

    private void InitializeSystems()
    {
        rb = GetComponent<Rigidbody2D>();

        
        healthSystem = new HealthSystem(maxHealth);
        healthSystem.OnHealthChanged += (currentHealth) =>
        {
            OnHealthChanged?.Invoke(currentHealth, healthSystem.GetMaxHealth());
        };
        healthSystem.OnDeath += HandleDeath;

       
        meleeAttack = new PlayerMeleeAttack(
            attackPoint,
            attackDamage,
            attackRange,
            attackCooldown,
            enemyLayer,
            knockbackForce
        );
       

       
        knockbackSystem = new KnockbackSystem(rb, playerKnockbackForce, playerKnockbackDuration);
    }

    private void Update()
    {
        knockbackSystem.Update();
        UpdateInvulnerability();
        UpdateRegeneration();
        
        if (Input.GetMouseButtonDown(0) && !knockbackSystem.IsKnockedBack())
        {
            if (!isAttacking)
            {
                PerformAttack();

            }

        }
       
    }

  
    public void PerformAttack()
    {
        meleeAttack.Attack();
        StartCoroutine(AttackAnimation());
    }

    public IEnumerator AttackAnimation()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.45f);
        isAttacking = false;
    }

    private void UpdateRegeneration()
    {
        if (!canRegenerate || healthSystem.IsDead()) return;

        lastDamageTime += Time.deltaTime;

        if (lastDamageTime >= regenerationDelay && healthSystem.GetCurrentHealth() < healthSystem.GetMaxHealth())
        {
            float regenAmount = regenerationRate * Time.deltaTime;
            healthSystem.Heal(regenAmount);
        }
    }

    public void ResetRegenerationTimer()
    {
        lastDamageTime = 0f;
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        if (isInvulnerable || healthSystem.IsDead()) return;

        healthSystem.TakeDamage(damage, knockbackDirection);
        knockbackSystem.ApplyKnockback(knockbackDirection);

        lastDamageTime = Time.time;
        isInvulnerable = true;

        ResetRegenerationTimer();
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }
    public float GetCurrentHealth()
    {
        return healthSystem.GetCurrentHealth();
    }
    public float GetMaxHealth()
    {
        return healthSystem.GetMaxHealth();
    }

    private void UpdateInvulnerability()
    {
        if (isInvulnerable && Time.time - lastDamageTime >= invulnerabilityDuration)
        {
            isInvulnerable = false;
        }
    }

    private void HandleDeath()
    {
        OnPlayerDeath?.Invoke();

       
    }
    public Transform GetAttackPoint()
    {
        return attackPoint;
    }

    public void Heal(float amount)
    {
        healthSystem.Heal(amount);
    }

   

    private void OnDrawGizmosSelected()
    {
     
        if (meleeAttack != null)
        {
            meleeAttack.DrawGizmos();
        }
        else if (attackPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
