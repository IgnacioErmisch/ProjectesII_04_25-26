using UnityEngine;

public class InputSpawnClone : MonoBehaviour
{
    [SerializeField] private CloneSpawner CloneSpawner;
    [SerializeField] private SwitchInterface switchInterface;
    [SerializeField] private Transform cloneSpawnerPoint;
    [SerializeField] private PlayerJump playerJump;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsAnyCloneActive() && switchInterface.IsBigCloneSelected && playerJump.isGrounded)
        {
            CloneSpawner.TrySpawnClone();
           
        }

        else if (Input.GetKeyDown(KeyCode.E) && !IsAnyCloneActive() &&!switchInterface.IsBigCloneSelected && playerJump.isGrounded)
        {
            CloneSpawner.TrySpawnClone();
           
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
           
            
            CloneSpawner.TryDespawnClone();
        }
    }
    public bool IsAnyCloneActive()
    {
        return CloneSpawner.cloneActive;
    }
}
