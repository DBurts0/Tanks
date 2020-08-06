using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{

    // Create a list
    public List<PowerUps> powerupList;

    // Create a variable to access the TankData script
    public TankData data;

    // Create a variable to access the PowerUps script
    public PowerUps powerup;

    // Start is called before the first frame update
    void Start()
    {
        powerupList = new List<PowerUps>();
        data = GetComponent<TankData>();
        powerup = GetComponent<PowerUps>();
    }

    // Create a function to add active powerups to the list
    public void Add(PowerUps activepowerup)
    {
        // Activate the powerup
        powerup.OnActivate(data);

        // Check if the powerup is not permamnent
        if (!activepowerup.isPermanent)
        {
            // Add the non permamnet powerup to the list
            powerupList.Add(activepowerup);
        }
    }
    
    // Create a function to remove powerups from the list
    public void Remove(PowerUps activePowerup)
    {
        powerup.OnDeactivate(data);
        powerupList.Remove(activePowerup);
    }

    // Update is called once per frame
    void Update()
    {
        List<PowerUps> expiredPowerups = new List<PowerUps>();
        // Create a loop that goes through each power up in the list
        foreach (PowerUps power in powerupList)
        {
            // Countdown
            power.duration -= Time.deltaTime;

            // If the timer runs out
            if (power.duration <= 0)
            {
                // Remove the powerup from the list
                expiredPowerups.Add(power);
            }
        }

        // Remove powerups from the list of active powerups that are on the expired list
        foreach (PowerUps power in expiredPowerups)
        {
            power.OnDeactivate(data);
            powerupList.Remove(power);
        }
        expiredPowerups.Clear();
    }
}
