using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUps
{
    // Create a variable for health changing pickups
    public int healthChanger;

    // Create a variable for speed changing pickups
    public float speedChanger;

    // Create a variable for changing how much damage the tank will deal
    public int damageChanger;
    // Create a variable for how long the non-permanent powerups will last
    public float duration;

    public float maxDuration;

    // Create a variable determining if the powerup will be permanent
    public bool isPermanent;

    // Create a variable to access the TankData Script
    public TankData data;

    public TankMotor motor;

    
    void Start()
    {
        duration = maxDuration;
    }

    public void OnActivate (GameObject tank)
    {
        motor = tank.GetComponent<TankMotor>();
        data = tank.GetComponent<TankData>();

        duration = maxDuration;
        motor.moveSpeed += speedChanger;
        motor.turnSpeed += speedChanger;
        data.currentHealth += healthChanger;
        data.damage += damageChanger;
    }

    public void OnDeactivate(GameObject tank)
    {
        motor = tank.GetComponent<TankMotor>();
        data = tank.GetComponent<TankData>();

        motor.moveSpeed -= speedChanger;
        motor.turnSpeed -= speedChanger;
        data.currentHealth -= healthChanger;
        data.damage -= damageChanger;
    }
}
