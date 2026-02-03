using UnityEngine;

public class PositionSwapBlock : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CloneSpawner bigCloneSpawner;
    [SerializeField] private CloneSpawner smallCloneSpawner;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PerspectiveSwitch perspectiveSwitch;    
    private AudioSource audioSource;
    private float swapCooldown = 0.5f;
    private float lastSwapTime;

    private void Awake()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        bool isPlayer = collision.CompareTag("Player");
        bool isBigClone = collision.CompareTag("BigClone");
        bool isSmallClone = collision.CompareTag("SmallClone");
        bool isClone = isBigClone || isSmallClone;

        if (!isPlayer && !isClone)                 
            return;
        
        if (Time.time - lastSwapTime < swapCooldown)                  
            return;
          
        CloneSpawner activeSpawner = GetActiveSpawner();

        if (activeSpawner == null)       
            return;
        
        GameObject currentClone = activeSpawner.GetCurrentClone();

        if (currentClone != null && (isPlayer || collision.gameObject == currentClone))
        {   
            SwapPositions(activeSpawner);
        }
     
    }

    private void SwapPositions(CloneSpawner activeSpawner)
    {
        GameObject currentClone = activeSpawner.GetCurrentClone();
        Vector3 playerPosition = player.transform.position;
        Vector3 clonePosition = currentClone.transform.position;
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        Rigidbody2D cloneRb = currentClone.GetComponent<Rigidbody2D>();
        Vector2 playerVelocity = Vector2.zero;
        Vector2 cloneVelocity = Vector2.zero;

        if (playerRb != null)
            playerVelocity = playerRb.linearVelocity;

        if (cloneRb != null)
            cloneVelocity = cloneRb.linearVelocity;

        player.transform.position = clonePosition;
        currentClone.transform.position = playerPosition;

        if (playerRb != null)
            playerRb.linearVelocity = cloneVelocity;

        if (cloneRb != null)
            cloneRb.linearVelocity = playerVelocity;

        AdjustCamera(currentClone);      
        lastSwapTime = Time.time;
      
    }

    private void AdjustCamera(GameObject currentClone)
    {
        if (playerCamera == null || perspectiveSwitch == null)
            return;
  
        if (perspectiveSwitch.GetControllingPlayer())
        {
            playerCamera.transform.SetParent(player.transform);
            playerCamera.transform.localPosition = new Vector3(2, 2, -5);
        }
       
        else
        {
            playerCamera.transform.SetParent(currentClone.transform);
            playerCamera.transform.localPosition = new Vector3(2, 1, -5);
        }
    }
    private CloneSpawner GetActiveSpawner()
    {
        if (bigCloneSpawner != null && bigCloneSpawner.cloneActive)
        {
            return bigCloneSpawner;
        }
        else if (smallCloneSpawner != null && smallCloneSpawner.cloneActive)
        {
            return smallCloneSpawner;
        }
        return null;
    }

}