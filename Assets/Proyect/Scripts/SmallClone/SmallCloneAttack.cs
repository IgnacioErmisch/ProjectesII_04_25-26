using UnityEngine;

public class SmallCloneAttack : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int jumpDamage;
    [SerializeField] private float dashKnockbackForce = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HitCollider"))
        {

            IDamageable damageable = collision.gameObject.GetComponentInParent<IDamageable>();

            if (damageable != null && !damageable.IsDead())
            {

                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                damageable.TakeDamage(jumpDamage, knockbackDirection * dashKnockbackForce);


            }

            if (transform.position.y > collision.bounds.max.y)
            {
               
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15f);
            }
        }
    }
}
