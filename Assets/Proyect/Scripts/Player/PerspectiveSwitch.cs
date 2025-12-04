using UnityEngine;

public class PerspectiveSwitch : MonoBehaviour
{
    [SerializeField] private CloneSpawner bigCloneSpawner;
    [SerializeField] private CloneSpawner smallCloneSpawner;
    public GameObject player;
    public Camera playerCamera;
    public bool controllingPlayer = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
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
           
        }
        else
        {
            playerCamera.transform.SetParent(player.transform);
            playerCamera.transform.localPosition = new Vector3(2, 2, -5);
       
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
