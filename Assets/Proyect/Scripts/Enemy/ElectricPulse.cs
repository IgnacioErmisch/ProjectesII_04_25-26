using UnityEngine;

public class ElectricPulse : MonoBehaviour
{
    private float damage;
    private float speed;
    private float radius;
    private LayerMask playerLayer;
    private LayerMask groundLayer;
    private bool hasHit;

    public void Initialize(float damage, float speed, float radius, LayerMask playerLayer, LayerMask groundLayer)
    {
        this.damage = damage;
        this.speed = speed;
        this.radius = radius;
        this.playerLayer = playerLayer;
        this.groundLayer = groundLayer;
        this.hasHit = false;

        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        if (hasHit) return;
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        CheckGroundImpact();
    }

    private void CheckGroundImpact()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayer);
        if (hit.collider != null)
            OnGroundHit();
    }

    private void OnGroundHit()
    {
        if (hasHit) return;
        hasHit = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null && !damageable.IsDead())
            {
                Vector2 knockbackDirection = (hit.transform.position - transform.position).normalized;
                damageable.TakeDamage(damage, knockbackDirection);
            }
        }

        Destroy(gameObject, 0.1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null && !damageable.IsDead())
        {
            Vector2 knockbackDirection = Vector2.zero;
            damageable.TakeDamage(damage, knockbackDirection);
            hasHit = true;
            Destroy(gameObject, 0.1f);
        }
    }
}