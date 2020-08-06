using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Create a variable to access the TankMotor script
    private TankMotor motor;
    // Create a variable to access the firepoint object
    public GameObject firePoint;
    // Create a variable to access the Shoot script
    private Shoot fire;
    // Start is called before the first frame update
    void Start()
    {
        // Access the TankMotor script
        motor = GetComponent<TankMotor>();
        // Access the Shoot script on the FirePoint object
        fire = firePoint.GetComponent<Shoot>();
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown
        fire.timer -= Time.deltaTime;
        // Check if the player is pressing w or the up arrow
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // Use the MoveForward function on the TankMotor script
            motor.MoveForward();
        }
        // Check if the player is pressing s or the down arrow
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // Use the MoveBackwards function on the TankMotor script
            motor.MoveBackwards();
        }
        // Check if the player is pressing a or the left arrow
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            // Use the RotateLeft function on the TankMotor script
            motor.RotateLeft();
        }
        // Check if the player is pressing d or the right arrow
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            // Use the RotateRight function on the TankMotor script
            motor.RotateRight();
        }
        // Check if the count down is less than or equal to 0
        if (fire.timer <= 0)
        {
            // Check if the player is pressing space
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Use the Fire function on the Shoot script
                fire.Fire();
            }
        }
    }
}
