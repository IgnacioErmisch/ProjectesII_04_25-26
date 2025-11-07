using UnityEngine;

public class PerspectiveSwitch : MonoBehaviour
{
    [SerializeField] private CloneSpawner cloneSpawner;
    public GameObject player;
    public Camera playerCamera;

    public bool controllingPlayer = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        if (!cloneSpawner.GetActiveClone())
            return;

        if (controllingPlayer)
        {
 
            GameObject currentClone = cloneSpawner.GetCurrentClone();
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

    public bool GetControllingPlayer()
    {
        return controllingPlayer;
    }
}
