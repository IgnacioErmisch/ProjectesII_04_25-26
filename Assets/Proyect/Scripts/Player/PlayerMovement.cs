using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float pjSpeed;
    private Rigidbody2D rb2D;

    [SerializeField] private CloneSpawner bigCloneSpawner;
    [SerializeField] private CloneSpawner smallCloneSpawner;
    [SerializeField] private PerspectiveSwitch perspectiveSwitch;
    [SerializeField] private Transform attackPoint;

    private SpriteRenderer spriteRenderer;
    private Vector3 originalAttackPointLocalPosition;
    private bool facingRight = true;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();    
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsAnyCloneActive())
            bigCloneSpawner.TrySpawnClone();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bigCloneSpawner.TryDespawnClone();
            smallCloneSpawner.TryDespawnClone();
        }
        if (Input.GetKeyDown(KeyCode.C) && !IsAnyCloneActive())
            smallCloneSpawner.TrySpawnClone();
    }

    void FixedUpdate()
    {
        if (perspectiveSwitch.GetControllingPlayer())
        {
            MovePJ();
        }
        else
        {
            rb2D.linearVelocity = Vector2.zero;
        }
    }

    private void MovePJ()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb2D.linearVelocity = new Vector2(horizontal * pjSpeed, rb2D.linearVelocity.y);

        if (horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !facingRight;
        }

      
        if (attackPoint != null && attackPoint != transform)
        {
            Vector3 newPosition = originalAttackPointLocalPosition;

            if (!facingRight)
            {
                newPosition.x = -Mathf.Abs(originalAttackPointLocalPosition.x);
            }
            else
            {
                newPosition.x = Mathf.Abs(originalAttackPointLocalPosition.x);
            }

            attackPoint.localPosition = newPosition;
        }
    }

    public bool IsAnyCloneActive()
    {
        return bigCloneSpawner.cloneActive || smallCloneSpawner.cloneActive;
    }
}