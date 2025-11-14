using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private GameObject leftLimit;
    [SerializeField] private GameObject rightLimit;
    [SerializeField] private bool hitLeft;
    [SerializeField] private bool hitRight;



    [SerializeField] private float enemyRadius;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;

    private EnemyMovement enemyMovement;
    private EnemyChase enemyChase;

    private void Awake()
    {
        InitializeComponents();
    }

    void Start()
    {

    }

    private void InitializeComponents()
    {
        enemyMovement = new EnemyMovement(speed, leftLimit, rightLimit, hitLeft, hitRight, transform);
        enemyChase = new EnemyChase(speed, player, enemyRadius, enemy);
    }


    void Update()
    {
        bool chasing = enemyChase.PlayerInTarget();

        if (!chasing)
        {
            enemyMovement.EnemyMove();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == leftLimit)
        {
            enemyMovement.hitLeft = true;
            enemyMovement.hitRight = false;
        }

        if (collision.gameObject == rightLimit)
        {
            enemyMovement.hitRight = true;
            enemyMovement.hitLeft = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.transform.position, enemyRadius);
        
    }
}
