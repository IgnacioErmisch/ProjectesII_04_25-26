using UnityEngine;

public class CloneSpawner : MonoBehaviour
{
    [SerializeField] private EnergyController energyController;
    [SerializeField] private PerspectiveSwitch perspectiveSwitch;
    [SerializeField] private CinemachineSingleton cinemachineSingleton;
    [SerializeField] private SwitchInterface switchInterface;
    [SerializeField] private GameObject cloneSmallPrefab;
    [SerializeField] private GameObject cloneBigPrefab;
    [SerializeField] private bool isSmallClone = true;
    [SerializeField] private CloneSpawner[] spawners;
    [SerializeField] private Transform cloneSpawnPointPrincipal;
    [SerializeField] private Transform cloneSpawnPointSecondary;
    public Camera playerCamera;
    private GameObject currentClone;
    public bool cloneActive = false;

    public LayerMask groundLayer;
   
    public bool TrySpawnClone()
    {
       
        foreach (var spawner in spawners)
        {
            if (spawner.cloneActive)
                return false;
        }

        if (cloneActive)
            return false;

        Vector3 spawnPosition = cloneSpawnPointPrincipal.position;
          
        if (!energyController.TryConsumeInitialCost(isSmallClone))
            return false;

        Vector3 direction = spawnPosition - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, groundLayer);
        Debug.Log(hit.rigidbody != null);
        if(hit.rigidbody == null)
        {         
            if(switchInterface.IsBigCloneSelected)
            {
                currentClone = Instantiate(cloneBigPrefab, spawnPosition + Vector3.up, Quaternion.identity);
                energyController.RegisterClone(currentClone, isSmallClone);
            }
            else if (!switchInterface.IsBigCloneSelected)
            { 

                currentClone = Instantiate(cloneSmallPrefab, spawnPosition , Quaternion.identity);
                energyController.RegisterClone(currentClone, isSmallClone);
            }
            cloneActive = true;
            playerCamera.transform.SetParent(currentClone.transform);
            playerCamera.transform.localPosition = new Vector3(2, 1, -5);
            perspectiveSwitch.SwitchToClone();

            return true;
        }
        else
        {
            Vector3 spawnPositionSecondary = cloneSpawnPointSecondary.position;
            
            if (switchInterface.IsBigCloneSelected)
            { 
                currentClone = Instantiate(cloneBigPrefab, spawnPositionSecondary + Vector3.up, Quaternion.identity);
                energyController.RegisterClone(currentClone, isSmallClone);
            }
            else if(!switchInterface.IsBigCloneSelected)
            { 
                currentClone = Instantiate(cloneSmallPrefab, spawnPositionSecondary, Quaternion.identity);
                energyController.RegisterClone(currentClone, isSmallClone);
            }
            cloneActive = true;
            playerCamera.transform.SetParent(currentClone.transform);
            playerCamera.transform.localPosition = new Vector3(2, 1, -5);
            perspectiveSwitch.SwitchToClone();
            

                return true;
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 spawnPosition = cloneSpawnPointPrincipal.position;
        Gizmos.DrawLine(transform.position, spawnPosition);
        Vector3 direction = spawnPosition - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, groundLayer);
        if (hit.rigidbody != null)
        {
            Gizmos.DrawSphere((Vector3)hit.point, 0.2f);
        }
        else
        {
            Gizmos.DrawSphere(spawnPosition, 0.2f);
        }
    }
    public bool TryDespawnClone()
    {
        if (!cloneActive)
            return false;

        if (currentClone != null)
        {        
            playerCamera.transform.SetParent(gameObject.transform);
            playerCamera.transform.localPosition = new Vector3(2, 2, -5);
            perspectiveSwitch.SwitchToPlayer();
            energyController.UnregisterClone(currentClone);
            Destroy(currentClone);
            currentClone = null;
            cloneActive = false;
            return true;
        }

        return false;
    }

    public GameObject GetCurrentClone()
    {
        return currentClone;
    }
    public bool GetActiveClone()
    {
        return cloneActive;
    }
}