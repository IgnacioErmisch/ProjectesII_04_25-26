using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SpawnCharacter : MonoBehaviour
{
    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private EnergyController energyController;
    [SerializeField] private PlayerCombatController playerCombatController;
    [SerializeField] private GameObject player;
    [SerializeField] private CheckpointManager checkpointManager;
    void Awake()
    {
        GameManager.Instance.SetPlayer(player);
    }

    // Update is called once per frame
    void Update()
    {
        if(combatController.GetCurrentHealth() <= 0 || energyController.GetCurrentEnergy() <= 0)
        {
            player.transform.position = checkpointManager.GetLastCheckpoint();
            energyController.ResetEnergy();
            playerCombatController.ResetHealth();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BlastZone")|| collision.gameObject.CompareTag("Spikes"))
        {
            player.transform.position = checkpointManager.GetLastCheckpoint();
            energyController.ResetEnergy();
            playerCombatController.ResetHealth();

        }
    }
}
