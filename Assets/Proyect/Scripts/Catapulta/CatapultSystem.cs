using UnityEngine;

public class CatapultSystem : MonoBehaviour
{
    [Header("Platform References")]
    [SerializeField] private CatapultPlatform catapultPlatform;
    [SerializeField] private LaunchPlatform launchPlatform;

    [Header("Settings")]
    [SerializeField] private float launchDelay = 0.2f;

    private float currentImpactForce = 0f;

    private void Awake()
    {
        if (catapultPlatform == null)
        {
            catapultPlatform = FindFirstObjectByType<CatapultPlatform>();
        }

        if (launchPlatform == null)
        {
            launchPlatform = FindFirstObjectByType<LaunchPlatform>();
        }
    }

    public void OnCatapultActivated(float impactForce)
    {
        currentImpactForce = impactForce;

        if (launchPlatform != null)
        {
            Invoke(nameof(TriggerLaunch), launchDelay);
        }
    }

    private void TriggerLaunch()
    {
        if (launchPlatform != null)
        {
            launchPlatform.LaunchPlayer(currentImpactForce);
        }

        currentImpactForce = 0f;
    }

    public void ResetSystem()
    {
        CancelInvoke();
        currentImpactForce = 0f;

        if (catapultPlatform != null)
            catapultPlatform.ForceReset();

        if (launchPlatform != null)
            launchPlatform.ForceReset();
    }
}