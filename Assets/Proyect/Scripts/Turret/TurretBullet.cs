using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    private Transform target;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
