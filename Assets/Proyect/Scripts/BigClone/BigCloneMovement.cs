using UnityEngine;

public class BigCloneMovement
{
    public Rigidbody2D rb;
    public float speed;
    public BigCloneStats stat;
    public float horizontal;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    public bool isMoving;
    public BigCloneMovement(Rigidbody2D rb, BigCloneStats stat, SpriteRenderer spriteRenderer)
    {
        this.rb = rb;
        this.speed = 7f * stat.SpeedMultiplier;
        this.spriteRenderer = spriteRenderer;
    }

    public void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        isMoving = true;
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        if (horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && facingRight)
        {
            Flip();
        }

        if (horizontal == 0)
        {
            isMoving = false;
        }

    }
    private void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;

    }

    public bool GetMove()
    {
        return isMoving;
    }
}
