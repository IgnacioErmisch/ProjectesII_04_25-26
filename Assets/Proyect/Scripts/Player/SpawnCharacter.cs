using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnCharacter : MonoBehaviour
{
    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private EnergyController energyController;
    [SerializeField] private GameObject player;
    [SerializeField] private CheckpointManager checkpointManager;

   

    void Update()
    {
        if (combatController.GetCurrentHealth() <= 0 || energyController.GetCurrentEnergy() <= 0)
        {
            StartCoroutine(WaitForSpawn());
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes"))
        {
            StartCoroutine(WaitForSpawn());
        }
        if (collision.CompareTag("BlastZone"))
        {
            energyController.ResetEnergy();
            combatController.ResetHealth();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private IEnumerator WaitForSpawn()
    {
        
        yield return new WaitForSeconds(1f);
        //player.transform.position = checkpointManager.GetLastCheckpoint();
        energyController.ResetEnergy();
        combatController.ResetHealth();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
