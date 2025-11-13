using UnityEngine;
using System.Collections.Generic;

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
  
        Vector3 spawnPosition = transform.position + spawnOffset;
          
        if (!energyController.TryConsumeInitialCost(isSmallClone))
            return false;

       
        currentClone = Instantiate(clonePrefab, spawnPosition, Quaternion.identity);
        cloneActive = true;
        Rigidbody2D cloneRb = currentClone.GetComponent<Rigidbody2D>();
        Rigidbody2D playerRb = perspectiveSwitch.player.GetComponent<Rigidbody2D>();
        cloneRb.bodyType = RigidbodyType2D.Dynamic;
        playerRb.bodyType = RigidbodyType2D.Static;
        playerCamera.transform.SetParent(currentClone.transform);
        playerCamera.transform.localPosition = new Vector3(2, 1, -5);
        perspectiveSwitch.SwitchToClone();
        energyController.RegisterClone(currentClone, isSmallClone);

        return true;
    }

    public bool TryDespawnClone()
    {
        if (!cloneActive)
            return false;

        if (currentClone != null)
        {
            Rigidbody2D playerRb = perspectiveSwitch.player.GetComponent<Rigidbody2D>();
            playerRb.bodyType = RigidbodyType2D.Dynamic;
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