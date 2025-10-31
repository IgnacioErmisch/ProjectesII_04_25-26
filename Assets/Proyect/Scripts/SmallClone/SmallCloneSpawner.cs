using UnityEngine;

public class SmallCloneSpawner : MonoBehaviour
{
    public GameObject clonePrefab;
    public Vector3 spawnOffset = new Vector3(2f, 0f, 0f);
    private GameObject currentClone;
    public bool cloneActive = false;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && !cloneActive)
        {
            SpawnClone();
        }


        if (Input.GetKeyDown(KeyCode.F) && cloneActive)
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
