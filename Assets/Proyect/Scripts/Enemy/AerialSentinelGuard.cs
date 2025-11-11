using UnityEngine;

public class AerialSentinelEnemy : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 40f;

    [Header("Movement Settings")]
    [SerializeField] private float patrolSpeed = 1.5f;
    [SerializeField] private float patrolHeight = 5f;
    [SerializeField] private float patrolRangeX = 8f;
    [SerializeField] private Vector2 patrolCenter;

    [Header("Attack Settings")]
    [SerializeField] private GameObject electricPulsePrefab;
    [SerializeField] private float pulseInterval = 3f;
    [SerializeField] private float pulseDamage = 15f;
    [SerializeField] private float pulseRadius = 3f;
    [SerializeField] private float pulseSpeed = 8f;
    [SerializeField] private Transform pulseSpawnPoint;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.3f;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private HealthSystem healthSystem;
    private RadiusDetectionSystem detectionSystem;
    private KnockbackSystem knockbackSystem;
    private Rigidbody2D rb;
    private bool movingRight = true;
    private float minX, maxX;
    private float nextPulseTime;

    private void Awake()
    {
        InitializeSystems();
    }

    private void Start()
    {
        if (patrolCenter == Vector2.zero)
            patrolCenter = transform.position;

        minX = patrolCenter.x - patrolRangeX / 2f;
        maxX = patrolCenter.x + patrolRangeX / 2f;

        transform.position = new Vector2(transform.position.x, patrolHeight);
        nextPulseTime = Time.time + pulseInterval;
    }

    private void InitializeSystems()
    {
        rb = GetComponent<Rigidbody2D>();
        healthSystem = new HealthSystem(maxHealth);
        healthSystem.OnDeath += HandleDeath;
        detectionSystem = new RadiusDetectionSystem(transform, detectionRadius, playerLayer);
        knockbackSystem = new KnockbackSystem(rb, knockbackForce, knockbackDuration);

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }
    }

    private void Update()
    {
        if (healthSystem.IsDead()) return;
        knockbackSystem.Update();

        if (!knockbackSystem.IsKnockedBack())
            Patrol();

        if (Time.time >= nextPulseTime)
        {
            FireElectricPulse();
            nextPulseTime = Time.time + pulseInterval;
        }
    }

    private void Patrol()
    {
        Vector2 velocity = rb.linearVelocity;

        if (movingRight)
        {
            velocity.x = patrolSpeed;
            FlipSprite(false);

            if (transform.position.x >= maxX)
                movingRight = false;
        }
        else
        {
            velocity.x = -patrolSpeed;
            FlipSprite(true);

            if (transform.position.x <= minX)
                movingRight = true;
        }

        rb.linearVelocity = velocity;
    }

    private void FireElectricPulse()
    {
        if (electricPulsePrefab != null)
        {
            Vector3 spawnPos = pulseSpawnPoint != null ? pulseSpawnPoint.position : transform.position;
            GameObject pulse = Instantiate(electricPulsePrefab, spawnPos, Quaternion.identity);

            ElectricPulse pulseScript = pulse.GetComponent<ElectricPulse>();
            if (pulseScript != null)
                pulseScript.Initialize(pulseDamage, pulseSpeed, pulseRadius, playerLayer, groundLayer);
        }
        else
        {
            FirePulseDirectly();
        }
    }

    private void FirePulseDirectly()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pulseRadius, playerLayer);

        foreach (Collider2D hit in hits)
        {
            if (IsPlayerOnGround(hit.transform))
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();
                if (damageable != null && !damageable.IsDead())
                {
                    Vector2 knockbackDirection = Vector2.down;
                    damageable.TakeDamage(pulseDamage, knockbackDirection);
                }
            }
        }
    }

    private bool IsPlayerOnGround(Transform playerTransform)
    {
        RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, Vector2.down, 0.5f, groundLayer);
        return hit.collider != null;
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
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        rb.gravityScale = 2f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Destroy(gameObject, 3f);
    }

    private void FlipSprite(bool flipLeft)
    {
        if (spriteRenderer != null)
            spriteRenderer.flipX = flipLeft;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pulseRadius);

        Gizmos.color = Color.blue;
        Vector2 center = patrolCenter != Vector2.zero ? patrolCenter : (Vector2)transform.position;
        float height = Application.isPlaying ? transform.position.y : patrolHeight;
        Vector3 leftPoint = new Vector3(center.x - patrolRangeX / 2f, height, 0);
        Vector3 rightPoint = new Vector3(center.x + patrolRangeX / 2f, height, 0);
        Gizmos.DrawLine(leftPoint, rightPoint);
        Gizmos.DrawWireSphere(leftPoint, 0.3f);
        Gizmos.DrawWireSphere(rightPoint, 0.3f);
    }
}
