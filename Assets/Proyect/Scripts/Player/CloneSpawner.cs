using UnityEngine;

public class CloneSpawner : MonoBehaviour
{
    
    [SerializeField] private EnergyController energyController;  
    [SerializeField] private PerspectiveSwitch perspectiveSwitch;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(2f, 0f, 0f);
    [SerializeField] private bool isSmallClone = true; 
    [SerializeField] private CloneSpawner[] spawners;
    public Camera playerCamera;

    private GameObject currentClone;
    public bool cloneActive = false;


    public bool TrySpawnClone()
    {
        foreach (var spawner in spawners)
        {
            if (spawner.cloneActive)
                return false;
        }

        if (cloneActive)
            return false;

        if (!energyController.TryConsumeInitialCost(isSmallClone))
            return false;

        Vector3 spawnPosition = transform.position + spawnOffset;
        currentClone = Instantiate(clonePrefab, spawnPosition, Quaternion.identity);
        cloneActive = true;

        playerCamera.transform.SetParent(currentClone.transform);
        playerCamera.transform.localPosition = new Vector3(2, 1, -5);

        energyController.RegisterClone(currentClone, isSmallClone);

        return true;
    }

    public bool TryDespawnClone()
    {
        if (!cloneActive)
            return false;

        if (currentClone != null)
        {
            playerCamera.transform.SetParent(gameObject.transform);
            playerCamera.transform.localPosition = new Vector3(2, 2, -5);

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