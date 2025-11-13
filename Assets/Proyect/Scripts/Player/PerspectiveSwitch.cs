using UnityEngine;

public class PerspectiveSwitch : MonoBehaviour
{
    [SerializeField] private CloneSpawner bigCloneSpawner;
    [SerializeField] private CloneSpawner smallCloneSpawner;
    public GameObject player;
    public Camera playerCamera;
    public bool controllingPlayer = true;
    private Rigidbody2D playerRb;

    private void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
        playerRb.bodyType = RigidbodyType2D.Dynamic;

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        CloneSpawner activeSpawner = GetActiveSpawner();

        if (activeSpawner == null)
            return;

        GameObject currentClone = activeSpawner.GetCurrentClone();
        Rigidbody2D cloneRb = currentClone.GetComponent<Rigidbody2D>();

        if (controllingPlayer)
        {
            playerCamera.transform.SetParent(currentClone.transform);
            playerCamera.transform.localPosition = new Vector3(2, 1, -5);
            playerRb.bodyType = RigidbodyType2D.Static;
            cloneRb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            playerCamera.transform.SetParent(player.transform);
            playerCamera.transform.localPosition = new Vector3(2, 2, -5);
            playerRb.bodyType = RigidbodyType2D.Dynamic;
            cloneRb.bodyType = RigidbodyType2D.Static;
        }



        controllingPlayer = !controllingPlayer;
    }

    private CloneSpawner GetActiveSpawner()
    {
        if (bigCloneSpawner.cloneActive)
        {
            return bigCloneSpawner;
        }
        else if (smallCloneSpawner.cloneActive)
        {
            return smallCloneSpawner;
        }

        return null; 
    }

    public void SwitchToClone()
    {
        controllingPlayer = false;
    }

   
    public void SwitchToPlayer()
    {
        controllingPlayer = true;
    }

    public bool GetControllingPlayer()
    {
        return controllingPlayer;
    }
}
