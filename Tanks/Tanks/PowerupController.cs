using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{

    // Create a list
    public List<PowerUps> powerupList;

    // Create a variable to access the TankData script
    public TankData data;

    // Create a variable to access the TankMotor script
    public TankMotor motor;

    // Create a variable to access the PowerUps script
    public PowerUps powerup;

    // Start is called before the first frame update
    void Start()
    {
        powerupList = new List<PowerUps>();
        data = GetComponent<TankData>();
        motor = GetComponent<TankMotor>();
        
    }

    // Create a function to add active powerups to the list
    public void Add(PowerUps activepowerup)
    {
        // Activate the powerup
        powerup.OnActivate(gameObject);

        // Check if the powerup is not permamnent
        if (!activepowerup.isPermanent)
        {
            // Add the non permamnet powerup to the list
            powerupList.Add(activepowerup);
        }
    }
    
    // Create a function to remove powerups from the list
    public void RemovePower(PowerUps activePowerup)
    {
        // Deactivate the powerup
        powerup.OnDeactivate(gameObject);
        // Remove the powerup from the list of active powerups
        powerupList.Remove(activePowerup);
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown
        powerup.duration -= Time.deltaTime;

        List<PowerUps> expiredPowerups = new List<PowerUps>();
        // If the timer runs out
        if (powerup.duration <= 0)
        {
        // Create a loop that goes through each power up in the list
         foreach (PowerUps powerup in powerupList)
         {

                // Remove the powerup from the list
                expiredPowerups.Add(powerup);
            }
            // Remove powerups from the list of active powerups that are on the expired list
            foreach (PowerUps powerup in expiredPowerups)
            {
                // Deactivate the expired powerup
                powerup.OnDeactivate(gameObject);

                // Remeove powerup from the list
                RemovePower(powerup);
            }
        }
        expiredPowerups.Clear();
    }
}
