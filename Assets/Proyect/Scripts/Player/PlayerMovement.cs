using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float pjSpeed;
    private Rigidbody2D rb2D;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb2D.linearVelocity = new Vector2(horizontal * pjSpeed, rb2D.linearVelocity.y);

    }
}
