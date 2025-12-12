using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontal;
    public float pjSpeed;
    private Rigidbody2D rb2D;
    public bool isMoving;
    


    [SerializeField] private PerspectiveSwitch perspectiveSwitch;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform cloneSpawnerPoint;
    [SerializeField] private PlayerCombatController playerCombatController;



    private SpriteRenderer spriteRenderer;
    private Vector3 originalAttackPointLocalPosition;
    private Vector3 originalCloneSpawnerPointLocalPosition;
    private bool facingRight = true;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();    
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();


    }

    

    void FixedUpdate()
    {
        if (perspectiveSwitch.GetControllingPlayer() && !playerCombatController.IsDead() )
        {
            MovePJ();
        }
        else
        {
            rb2D.linearVelocity = Vector2.zero;
           
        }
    }

    public void MovePJ()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb2D.linearVelocity = new Vector2(horizontal * pjSpeed, rb2D.linearVelocity.y);
        isMoving = true;

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
        

      
        if (attackPoint != transform)
        {
            Vector3 newPosition = originalAttackPointLocalPosition;
            Vector3 newPositionSpawner = originalCloneSpawnerPointLocalPosition;

            if (!facingRight)
            {
                newPosition.x = -Mathf.Abs(originalAttackPointLocalPosition.x);
                newPositionSpawner.x = -Mathf.Abs(originalCloneSpawnerPointLocalPosition.x + 0.6f);
            }
            else
            {
                newPosition.x = Mathf.Abs(originalAttackPointLocalPosition.x);
                newPositionSpawner.x = Mathf.Abs(originalCloneSpawnerPointLocalPosition.x);
            }

            attackPoint.localPosition = newPosition;
            cloneSpawnerPoint.localPosition = newPositionSpawner;
        }
    }

    
}