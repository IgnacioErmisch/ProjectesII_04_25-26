using UnityEngine;

public class InputSpawnClone : MonoBehaviour
{
    [SerializeField] private CloneSpawner bigCloneSpawner;
    [SerializeField] private CloneSpawner smallCloneSpawner;
    [SerializeField] private SwitchInterface switchInterface;
    [SerializeField] private Transform cloneSpawnerPoint;
    public GameObject QBigClone;
    public GameObject QSmallClone;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsAnyCloneActive() && switchInterface.GetBigCloneSelected())
        {
            bigCloneSpawner.TrySpawnClone();
            QBigClone.SetActive(true);
            QSmallClone.SetActive(false);
        }

        else if (Input.GetKeyDown(KeyCode.E) && !IsAnyCloneActive() && switchInterface.GetSmallCloneSelected())
        {
            smallCloneSpawner.TrySpawnClone();
            QBigClone.SetActive(false);
            QSmallClone.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            QBigClone.SetActive(false);
            QSmallClone.SetActive(false);
            bigCloneSpawner.TryDespawnClone();
            smallCloneSpawner.TryDespawnClone();
        }
    }
    public bool IsAnyCloneActive()
    {
        return bigCloneSpawner.cloneActive || smallCloneSpawner.cloneActive;
    }
}
