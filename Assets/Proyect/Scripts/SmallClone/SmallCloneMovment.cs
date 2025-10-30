using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;


public class SmallCloneMovment : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;
    private SmallCloneStats stat;
    private float horizontal;

    SmallCloneMovment(Rigidbody2D rb, float speed, SmallCloneStats stat)
    {
        this.rb = rb;
        this.speed = speed * stat.SpeedMultiplier;
        
    }

    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

    }

}
