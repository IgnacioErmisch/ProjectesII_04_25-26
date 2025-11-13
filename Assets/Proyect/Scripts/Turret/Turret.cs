using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Rendering;

public class Turret : MonoBehaviour
{
    [SerializeField] private RadiusDetectionSystem detectionSystem;
    [SerializeField] private Transform detectionPoint;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private GameObject turretBullet;
    [SerializeField] private Transform spawnBullets;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private LayerMask playerLayer;
    private float lastShootTime;
    [SerializeField] private float shootCooldown;

    private void Awake()
    {
        Initialize();
    }
    void Update()
    {
        if (detectionSystem.DetectTarget())
        {
            playerTarget = detectionSystem.GetTarget();

            if (Time.time >= lastShootTime + shootCooldown)
            {
                Attack();
                lastShootTime = Time.time;
            }
        }

    }

    private void Initialize()
    {
        detectionPoint = transform;
        detectionSystem = new RadiusDetectionSystem(detectionPoint, detectionRadius, playerLayer);
    }
    private void Attack()
    {
        Vector3 spawnPos = spawnBullets.transform.position;
        GameObject bullet = Instantiate(turretBullet, spawnPos, Quaternion.identity);
        TurretBullet bulletScript = bullet.GetComponent<TurretBullet>();
        bulletScript.SetTarget(playerTarget);

    }

    private void OnDrawGizmosSelected()
    {
      
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, detectionRadius);
    }
}
