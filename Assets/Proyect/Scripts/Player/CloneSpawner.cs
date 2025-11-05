using UnityEngine;

public class CloneSpawner : MonoBehaviour
{
   
    public GameObject clonePrefab; 
    public Vector3 spawnOffset = new Vector3(2f, 0f, 0f);
    private GameObject currentClone;
    public bool cloneActive = false;
    [SerializeField] private CloneSpawner[] spawners;

    public bool TrySpawnClone()
    {
        bool anotherCloneActive = false;
        foreach (var spawner in spawners)
            anotherCloneActive |= spawner.cloneActive;

        if (cloneActive || anotherCloneActive)
            return false;
        
        Vector3 spawnPosition = transform.position + spawnOffset;
        currentClone = Instantiate(clonePrefab, spawnPosition, Quaternion.identity);
        cloneActive = true;

        return true;
    }

    public bool TryDespawnClone()
    {
        if (!cloneActive)
            return false;

        if (currentClone != null)
        {
            Destroy(currentClone);
            cloneActive = false;
            return true;
        }
        return false;
    }
}
