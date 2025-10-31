using UnityEngine;

public class BigCloneSpawner : MonoBehaviour
{
   
    public GameObject clonePrefab; 
    public Vector3 spawnOffset = new Vector3(2f, 0f, 0f);
    private GameObject currentClone;
    public bool cloneActive = false;
    private SmallCloneSpawner ss;

    private void Start()
    {
        ss = FindFirstObjectByType<SmallCloneSpawner>();
    }

    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.E) && !cloneActive && !ss.cloneActive)
        {
            SpawnClone();
        }

       
        if (Input.GetKeyDown(KeyCode.Q) && cloneActive)
        {
            DespawnClone();
        }
    }

    private void SpawnClone()
    {
        
        Vector3 spawnPosition = transform.position + spawnOffset;
        currentClone = Instantiate(clonePrefab, spawnPosition, Quaternion.identity);
        cloneActive = true;

      
    }

    private void DespawnClone()
    {
        if (currentClone != null)
        {
            Destroy(currentClone);
            cloneActive = false;
            
        }
    }
}
