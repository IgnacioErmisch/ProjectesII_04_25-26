using UnityEngine;


public class PlayerCombatController : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float debugCurrentHealth;

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
        debugCurrentHealth = healthSystem.GetCurrentHealth();
        knockbackSystem.Update();
        UpdateInvulnerability();

        
        if (Input.GetMouseButtonDown(0) && !knockbackSystem.IsKnockedBack())
        {
            PerformAttack();
        }
    }

  
    public void PerformAttack()
    {
        meleeAttack.Attack();
    }

   
    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        if (isInvulnerable || healthSystem.IsDead()) return;

        healthSystem.TakeDamage(damage, knockbackDirection);
        knockbackSystem.ApplyKnockback(knockbackDirection);

        lastDamageTime = Time.time;
        isInvulnerable = true;

        
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }
    public float GetCurrentHealth() 
    { 
        return healthSystem.GetCurrentHealth();
    }
    public float CurrentHealth() 
    {
        return healthSystem.GetCurrentHealth();
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
        Debug.Log("muerte");
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
