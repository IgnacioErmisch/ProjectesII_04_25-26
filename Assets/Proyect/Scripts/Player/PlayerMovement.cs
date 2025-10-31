using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float pjSpeed;
    private Rigidbody2D rb2D;
    [SerializeField] private BigCloneSpawner bigCloneSpawner;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bigCloneSpawner = FindFirstObjectByType<BigCloneSpawner>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!bigCloneSpawner.cloneActive)
        {
            MovePJ();
        }
      
    }

    private void MovePJ()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb2D.linearVelocity = new Vector2(horizontal * pjSpeed, rb2D.linearVelocity.y);

    }
}
