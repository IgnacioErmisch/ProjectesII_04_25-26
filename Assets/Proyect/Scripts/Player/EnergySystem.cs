using UnityEngine;

public class EnergySystem
{
    private float currentEnergy;
    private float maxEnergy;
    private float regenerationRate;
    private float regenerationDelay;
    private float timeSinceLastDrain;
    private bool isRegenerating;

    public EnergySystem(float maxEnergy, float regenerationRate = 8f, float regenerationDelay = 1.5f)
    {
        this.maxEnergy = maxEnergy;
        this.currentEnergy = maxEnergy;
        this.regenerationRate = regenerationRate;
        this.regenerationDelay = regenerationDelay;
        this.timeSinceLastDrain = 0f;
        this.isRegenerating = false;
    }

    public void ConsumeEnergy(float amount)
    {
        currentEnergy -=amount;
 
        timeSinceLastDrain = 0f;
        isRegenerating = false;
    }
    public void Update()
    {
        
        if (!isRegenerating)
        {
            timeSinceLastDrain += Time.deltaTime;

            if (timeSinceLastDrain >= regenerationDelay)
            {
                isRegenerating = true;
            }
        }
      
        if (isRegenerating && currentEnergy < maxEnergy)
        {
            currentEnergy += regenerationRate * Time.deltaTime;           
        }

    }



}
