using UnityEngine;
using UnityEngine.InputSystem.Composites;


public class EnemyMovement
{
    public float speed;
    public GameObject leftLimit;
    public GameObject rightLimit;
    public bool hitLeft;
    public bool hitRight;
    public Transform transform;

   
    public EnemyMovement(float speed, GameObject leftLimit, GameObject rightLimit, bool hitLeft, bool hitRight, Transform transform)
    {
        this.speed = speed;
        this.leftLimit = leftLimit;
        this.rightLimit = rightLimit;
        this.hitLeft = hitLeft;
        this.hitRight = hitRight;
        this.transform = transform;
    }

    public void EnemyMove()
    {
        if (hitLeft)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else if (hitRight)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

    }
}

public class EnemyChase
{
    public float speed;
    public GameObject player;
    public GameObject enemy;
    public float enemyRadius;

    public EnemyChase(float speed, GameObject player, float enemyRadius, GameObject enemy)
    {
        this.speed = speed;
        this.player = player;
        this.enemyRadius = enemyRadius;
        this.enemy = enemy;
    }

    public bool PlayerInTarget()
    {
       Collider2D hit =  Physics2D.OverlapCircle(enemy.transform.position, enemyRadius);
        
        if (hit.gameObject == player && hit != null)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, speed * Time.deltaTime);
            return true;
        }

        return false;
    }

}
