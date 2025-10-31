using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float pjSpeed;
    private Rigidbody2D rb2D;
    [SerializeField] private BigCloneSpawner bigCloneSpawner;
    [SerializeField] private SmallCloneSpawner smallCloneSpawner;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bigCloneSpawner = FindFirstObjectByType<BigCloneSpawner>();
        smallCloneSpawner = FindFirstObjectByType<SmallCloneSpawner>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!bigCloneSpawner.cloneActive)
        {
            MovePJ();
        }
        else if (!smallCloneSpawner.cloneActive)
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
