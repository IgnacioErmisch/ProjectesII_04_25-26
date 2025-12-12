using UnityEngine;
using System.Collections.Generic;

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
    [SerializeField] private Transform cloneSpawnPoint;
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

        Vector3 spawnPosition = cloneSpawnPoint.position;
          
        if (!energyController.TryConsumeInitialCost(isSmallClone))
            return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, spawnPosition - transform.position,2f, groundLayer);
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

                currentClone = Instantiate(cloneSmallPrefab, spawnPosition + Vector3.up , Quaternion.identity);
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
        
            if (switchInterface.IsBigCloneSelected)
            { 
                currentClone = Instantiate(cloneBigPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
                energyController.RegisterClone(currentClone, isSmallClone);
            }
            else if(!switchInterface.IsBigCloneSelected)
            { 
                currentClone = Instantiate(cloneSmallPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
                energyController.RegisterClone(currentClone, isSmallClone);
            }
            cloneActive = true;
            playerCamera.transform.SetParent(currentClone.transform);
            playerCamera.transform.localPosition = new Vector3(2, 1, -5);
            perspectiveSwitch.SwitchToClone();
            if(spawnPosition.x - transform.position.x > 0)
                transform.position += new Vector3(-2f, 0,0);
            else
                transform.position += new Vector3(2f, 0,0);

                return true;
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 spawnPosition = cloneSpawnPoint.position;
        Gizmos.DrawLine(transform.position, spawnPosition);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, spawnPosition - transform.position, 2f, groundLayer);
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