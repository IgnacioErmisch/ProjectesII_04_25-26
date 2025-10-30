using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;


public class SmallCloneMovment : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public SmallCloneStats stat;
    public float horizontal;

    public SmallCloneMovment(Rigidbody2D rb, float speed, SmallCloneStats stat)
    {
        this.rb = rb;
        this.speed = speed * stat.SpeedMultiplier;
        
    }

    public void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

    }

}
