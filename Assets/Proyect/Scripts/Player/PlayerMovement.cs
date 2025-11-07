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


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
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


    // Update is called once per frame
    void FixedUpdate()
    {

        MovePJ(); 
       

    }

    private void MovePJ()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb2D.linearVelocity = new Vector2(horizontal * pjSpeed, rb2D.linearVelocity.y);

    }

    public bool IsAnyCloneActive()
    {
        return bigCloneSpawner.cloneActive || smallCloneSpawner.cloneActive;
    }
}
