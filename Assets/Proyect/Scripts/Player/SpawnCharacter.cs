using System.Collections;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private EnergyController energyController;
    [SerializeField] private GameObject player;
    [SerializeField] private CheckpointManager checkpointManager;

    private bool isRespawning = false;

    void Update()
    {
        if (!isRespawning && (combatController.GetCurrentHealth() <= 0 || energyController.GetCurrentEnergy() <= 0))
        {
            StartRespawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isRespawning && (collision.CompareTag("BlastZone") || collision.CompareTag("Spikes")))
        {
            StartRespawn();
        }
    }

    private void StartRespawn()
    {
        isRespawning = true;
        StartCoroutine(WaitForSpawn());
    }

    private IEnumerator WaitForSpawn()
    {
        
        yield return new WaitForSeconds(2f);
     
        player.transform.position = checkpointManager.GetLastCheckpoint();
        energyController.ResetEnergy();
        combatController.ResetHealth();

        isRespawning = false;
    }
}
