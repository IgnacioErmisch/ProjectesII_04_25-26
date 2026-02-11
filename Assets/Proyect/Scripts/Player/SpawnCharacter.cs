using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnCharacter : MonoBehaviour
{
    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private EnergyController energyController;
    [SerializeField] private GameObject player;
   

    void Update()
    {
        if (combatController.GetCurrentHealth() <= 0)
        {
            StartCoroutine(WaitForSpawn());
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            StartCoroutine(WaitForSpawn());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlastZone"))
        {
            energyController.ResetEnergy();
            combatController.ResetHealth();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private IEnumerator WaitForSpawn()
    {
        
        yield return new WaitForSeconds(2f);
        energyController.ResetEnergy();
        combatController.ResetHealth();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
