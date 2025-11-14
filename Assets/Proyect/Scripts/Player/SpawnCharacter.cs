using UnityEngine;
using UnityEngine.SceneManagement;
public class SpawnCharacter : MonoBehaviour
{
    [SerializeField] private PlayerCombatController combatController;
    [SerializeField] private EnergyController energyController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(combatController.GetCurrentHealth() <= 0 || energyController.GetCurrentEnergy() <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BlastZone")|| collision.gameObject.CompareTag("Spikes"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
