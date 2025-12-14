using UnityEngine;

public class InputSpawnClone : MonoBehaviour
{
    [SerializeField] private CloneSpawner CloneSpawner;
    //[SerializeField] private CloneSpawner smallCloneSpawner;
    [SerializeField] private SwitchInterface switchInterface;
    [SerializeField] private Transform cloneSpawnerPoint;
    [SerializeField] private PlayerJump playerJump;
    public GameObject QBigClone;
    public GameObject QSmallClone;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsAnyCloneActive() && switchInterface.IsBigCloneSelected && playerJump.isGrounded)
        {
            CloneSpawner.TrySpawnClone();
            QBigClone.SetActive(true);
            QSmallClone.SetActive(false);
        }

        else if (Input.GetKeyDown(KeyCode.E) && !IsAnyCloneActive() &&!switchInterface.IsBigCloneSelected && playerJump.isGrounded)
        {
            CloneSpawner.TrySpawnClone();
            QBigClone.SetActive(false);
            QSmallClone.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            QBigClone.SetActive(false);
            QSmallClone.SetActive(false);
            
            CloneSpawner.TryDespawnClone();
        }
    }
    public bool IsAnyCloneActive()
    {
        return CloneSpawner.cloneActive;
    }
}
