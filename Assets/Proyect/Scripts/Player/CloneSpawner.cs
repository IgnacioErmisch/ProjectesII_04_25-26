using UnityEngine;
using System.Collections.Generic;

public class CloneSpawner : MonoBehaviour
{
    [SerializeField] private EnergyController energyController;
    [SerializeField] private PerspectiveSwitch perspectiveSwitch;
    [SerializeField] private CinemachineSingleton cinemachineSingleton;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(2f, 0f, 0f);
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, spawnPosition - transform.position,spawnPosition.x, groundLayer);
        Debug.Log(hit.rigidbody != null);
        if(hit.rigidbody == null)

        {
        Debug.Log("Bueno");   
            currentClone = Instantiate(clonePrefab, spawnPosition, Quaternion.identity);
            cloneActive = true;
            playerCamera.transform.SetParent(currentClone.transform);
            playerCamera.transform.localPosition = new Vector3(2, 1, -5);
            perspectiveSwitch.SwitchToClone();
            energyController.RegisterClone(currentClone, isSmallClone);

            return true;
        }
        else
        {
        Debug.Log("Malo");   
            currentClone = Instantiate(clonePrefab, transform.position, Quaternion.identity);
            cloneActive = true;
            playerCamera.transform.SetParent(currentClone.transform);
            playerCamera.transform.localPosition = new Vector3(2, 1, -5);
            perspectiveSwitch.SwitchToClone();
            energyController.RegisterClone(currentClone, isSmallClone);
            if(spawnPosition.x - transform.position.x > 0)
                transform.position += new Vector3(-3f, 0,0);
            else
                transform.position += new Vector3(3f, 0,0);

                return true;
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 spawnPosition = cloneSpawnPoint.position;
        Gizmos.DrawLine(transform.position, spawnPosition);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, spawnPosition - transform.position, 2, groundLayer);
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