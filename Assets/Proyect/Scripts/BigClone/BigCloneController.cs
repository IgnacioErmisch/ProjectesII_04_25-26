using UnityEngine;

public class BigCloneController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BigCloneStats stats;
    private BigCloneMovement movement;


    private void Awake()
    {
        InitializeComponents();
        ApplySizeModifier();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = new BigCloneStats();
        movement = new BigCloneMovement(rb, stats);
        
    }

    private void ApplySizeModifier()
    {
        transform.localScale = Vector3.one * stats.SizeMultiplier;
    }
    private void Update()
    {
      
    

    }
    
    private void FixedUpdate()
    {
        movement.Move();
    }
}
