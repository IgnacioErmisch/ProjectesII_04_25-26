using UnityEngine;

public class BigCloneMovement
{
    public Rigidbody2D rb;
    public float speed;
    public BigCloneStats stat;
    public float horizontal;

    public BigCloneMovement(Rigidbody2D rb, BigCloneStats stat)
    {
        this.rb = rb;
        this.speed = 5f * stat.SpeedMultiplier;
       
    }

    public void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

    }
}
