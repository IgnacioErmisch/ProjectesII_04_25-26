using UnityEngine;

public class SmallCloneAttack : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BasicGuardHitCollider"))
        {
            if (transform.position.y > collision.bounds.max.y)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15f);
            }
        }
    }
}
