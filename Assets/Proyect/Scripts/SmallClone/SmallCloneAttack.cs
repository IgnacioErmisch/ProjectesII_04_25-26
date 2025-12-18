using UnityEngine;

public class SmallCloneAttack : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int jumpDamage;
    [SerializeField] private int jumpForce;
    [SerializeField] private float dashKnockbackForce = 0f;
    private SoundManager soundManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HitCollider"))
        {
            soundManager.PlaySFX(soundManager.jumpOnEnemies);
            IDamageableBlue damageable = collision.gameObject.GetComponentInParent<IDamageableBlue>();

            if (damageable != null && !damageable.IsDead())
            {

                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                damageable.TakeDamage(jumpDamage, knockbackDirection * dashKnockbackForce);


            }

            if (transform.position.y > collision.bounds.max.y)
            {
               
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }
}
