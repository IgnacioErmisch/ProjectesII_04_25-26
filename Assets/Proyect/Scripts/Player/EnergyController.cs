using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class EnergyController : MonoBehaviour
{

    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float currentEnergy;

    [SerializeField] private float smallCloneInitialCost = 15f;
    [SerializeField] private float largeCloneInitialCost = 30f;


    [SerializeField] private float smallCloneDrainPerSecond = 5f;
    [SerializeField] private float largeCloneDrainPerSecond = 10f;


    [SerializeField] private float regenerationRate = 8f;
    [SerializeField] private float regenerationDelay = 1.5f;

    private Dictionary<GameObject, float> activeClones = new Dictionary<GameObject, float>();
    private Coroutine regenerationCoroutine;
    private bool isDead = false;

    public TextMeshProUGUI energyText;
    public Image energyPlayer;
    public Image energyBigClone = null;
    public Image energySmallClone = null;

    public delegate void EnergyChangedDelegate(float current, float max);
    public event EnergyChangedDelegate OnEnergyChanged;

    public delegate void PlayerDeathDelegate();
    public event PlayerDeathDelegate OnPlayerDeath;

    private void Start()
    {
        currentEnergy = maxEnergy;
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        
    }

    private void Update()
    {
        if (isDead) return;

        if (activeClones.Count > 0)
        {
            float totalDrain = 0f;
            foreach (var drain in activeClones.Values)
                totalDrain += drain;

            DrainEnergy(totalDrain * Time.deltaTime);
        }

    }

    public bool TryConsumeInitialCost(bool isSmall)
    {
        if (isDead) return false;

        float cost = isSmall ? smallCloneInitialCost : largeCloneInitialCost;
        if (currentEnergy >= cost)
        {
            DrainEnergy(cost);
            return true;
        }

        return false;
    }

    public void RegisterClone(GameObject clone, bool isSmall)
    {
        if (isDead) return;

        float drainRate = isSmall ? smallCloneDrainPerSecond : largeCloneDrainPerSecond;
        if (!activeClones.ContainsKey(clone))
        {
            activeClones.Add(clone, drainRate);
            StopRegeneration();
        }
    }

    public void UnregisterClone(GameObject clone)
    {
        if (activeClones.ContainsKey(clone))
        {
            activeClones.Remove(clone);

            if (activeClones.Count == 0)
                StartRegeneration();
        }
    }

    private void DrainEnergy(float amount)
    {
        if (currentEnergy > 0)
        {
            currentEnergy -= amount;
            currentEnergy = Mathf.Max(currentEnergy, 0f);
            energyPlayer.fillAmount = Mathf.Clamp(currentEnergy / maxEnergy, 0f, 1f);
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
            if (energyBigClone != null)
            {
                energyBigClone.fillAmount = Mathf.Clamp(currentEnergy / maxEnergy, 0f, 1f); ;
            }
            if (energySmallClone != null)
            {
                energySmallClone.fillAmount = Mathf.Clamp(currentEnergy / maxEnergy, 0f, 1f); ;
            }
            CheckDeath();
        }
    }

    private void CheckDeath()
    {
        if (currentEnergy <= 0 && !isDead)
        {
            isDead = true;
            StopRegeneration();
            OnPlayerDeath?.Invoke();
        }
    }

    private void StartRegeneration()
    {
        StopRegeneration();
        regenerationCoroutine = StartCoroutine(RegenerateEnergyCoroutine());
    }

    private void StopRegeneration()
    {
        if (regenerationCoroutine != null)
        {
            StopCoroutine(regenerationCoroutine);
            regenerationCoroutine = null;
        }
    }

    private IEnumerator RegenerateEnergyCoroutine()
    {
        yield return new WaitForSeconds(regenerationDelay);

        while (currentEnergy < maxEnergy && activeClones.Count == 0 && !isDead)
        {
            currentEnergy += regenerationRate * Time.deltaTime;
            currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
            energyPlayer.fillAmount = Mathf.Clamp(currentEnergy / maxEnergy, 0f, 1f);
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
            yield return null;
        }
    }


    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public void GetBigClone(Image image)
    {
        energyBigClone = image;
    }
    public void GetSmallClone(Image image)
    {
        energySmallClone = image;
    }
}
