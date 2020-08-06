 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps
{
    // Create a variable for health changing pickups
    public float healthChanger;

    // Create a variable for speed changing pickups
    public float speedMultiplier;

    // Create a variable for firerate changing pickups
    public float firerateMultiplier;

    // Create a variable for detection range changing pickups
    public float detectionRangeChanger;

    // Create a variable for how long the non-permanent powerups will last
    public float duration;

    // Create a variable determining if the powerup will be permanent
    public bool isPermanent;
    
    // Create a variable to store information of the tanks
    public GameObject tank;

    // Create a variable to access the TankData Script
    public TankData data;

    void Start()
    {
        data = tank.GetComponent<TankData>();
    }

    public void OnActivate (TankData target)
    {

    }

    public void OnDeactivate(TankData target)
    {

    }
}
