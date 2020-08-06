using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Create variables to access the Shoot script and the FirePoint object
    private Shoot fire;
    public GameObject firePoint;

    // Create variables to access the TankMotor and TankData scripts
    public TankMotor motor;
    public TankData data;

    // Create a variable to restore the tank's speed that the designers set 
    private float restoreSpeed;

    // Create a variable to increase/decrease the tank's speed
    public float speedChange;

    // Create a minimum distance the tanks should be to the waypoint to think they at it
    public float minDistance;

    // Create an array for the waypoints the AI will travel to
    public Transform[] waypoints;

    // Create a variable for the current waypoint in the patrol route
    public int currentWaypoint;

    // Create a variable to store the angle to the current waypoint
    public float angleToWaypoint;

    public float angleToPlayer;

    // Create a variable for the direction of the player
    private Vector3 direction;

    // Create a variable for the direction away from the player
    private Vector3 fleeDirection;

    // Create a variable for the direction of the object the tanks will rotate towards
    private Vector3 targetDirection;

    // Create a variable to access the player object
    public GameObject player;

    // Create a variable to store what the Raycast is hitting
    private RaycastHit hitInfo;

    // Create a variable for the AI's sound detection range
    public float detectionRange;

    // Create a variable for the tank's vision range
    public float visionRange;

    // Create variables for the tank's field of vision
    public float fOV;
    private float negativeFOV;

    // Create a variable for the tank's current state
    public string state;

    // Create variables to dictate how long the tank will be in a step of the Avoidance function
    public float stageTimer;
    public float resetTimer;

    // Create a variable to let the AI know what step in the avoidance sequence they are in
    public int avoidanceStage;


    // Start is called before the first frame update
    void Start()
    {
        // Access the Shoot, TankMotor, and TankData scripts
        fire = firePoint.GetComponent<Shoot>();
        motor = GetComponent<TankMotor>();
        data = GetComponent<TankData>();

        // Set restoreSpeed to the designer set speed 
        restoreSpeed = motor.moveSpeed;

        // Get the direction of the player
        direction = player.transform.position - transform.position;

        // Set the avoidance stage to 0
        avoidanceStage = 0;

        // Set the state to "Patrol" 
        state = "Patrol";

        // Set the negativeFOV equal to 180 - the fOV
        negativeFOV = 180 - fOV;
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown
        fire.timer -= Time.deltaTime;
        stageTimer -= Time.deltaTime;
        // Create a Raycast infront of the tank equal to their detection range
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, visionRange))
        {
            // Draw the Raycast
            Debug.DrawRay(transform.position, transform.forward * visionRange, Color.green);
        }

        // Continue patrolling if the player can't be heard
        if (state == "Patrol") {
            // Patrol the area
            Patrol();
            // Check if the playe is close enough to be heard
            CanHear();
        }
        if (state == "Investigating")
        {
            // Rotate towards the player and check if they can be seen
            RotateTo(player.transform.position, motor.turnSpeed);
            CanSee();
        }
        if (state == "Chase")
        {
            // Follow and attack the player
            Chase();
            // CHeck for walls
            WallCheck();
            // Shoot forwards
            Attack();
            // Check if the player can still be seen
            CanSee();
        }
        if (state == "Flee")
        {
            // Run from the player
            Flee();
            // Check if the player can still be heard
            CanHear();
        }
    }

    public void RotateTo(Vector3 target, float speed)
    {
        // Get the direction of the target
        targetDirection = target - transform.position;
        // Rotate towards the target over time
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, speed * Time.deltaTime);
    }

    public void MoveTo(Vector3 target, float speed)
    {
        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * 0.5f * Time.deltaTime);
    }

    void Patrol()
    {
        // Return to normal speed
        motor.moveSpeed = restoreSpeed;

        // Create a Raycast infront of the tank equal to their vision range
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, visionRange))
        {
            // Draw the Raycast
            Debug.DrawRay(transform.position, transform.forward * visionRange, Color.green);
        }

        // Create the angle from the tank to the current waypoint
        angleToWaypoint = Vector3.Angle(firePoint.transform.position, waypoints[currentWaypoint].position);

        // Rotate towards the current waypoint
        RotateTo(waypoints[currentWaypoint].position, motor.turnSpeed);

        // Check if the tank is looking at the next waypoint
        if ((angleToWaypoint <= fOV || angleToWaypoint <= negativeFOV))
        {
            // Move towards the waypoint
            MoveTo(waypoints[currentWaypoint].position, motor.moveSpeed);
        }

        // If the tank moves close enough to the waypoint, set their target to the next waypoint
        if (Vector3.Distance(waypoints[currentWaypoint].position, transform.position) <= minDistance)
        //if (transform.position  == waypoints[currentWaypoint].position)
        {
            currentWaypoint++;
        }

        // If the tank reached the last checkpoint, go through the route again 
        if (currentWaypoint == waypoints.Length)
        {
            currentWaypoint = 0;
        }
    }

    void WallCheck()
    {
        // Check if the tank's Raycast is hitting anything other than the player
        if (hitInfo.transform != player.transform)
        {
            // Start the avoidance sequence
            avoidanceStage = 1;
            // Stop moving
            motor.moveSpeed = 0;
            Avoidance(state);
        }
        else
        {
            // Return to the tank's normal speed
            motor.moveSpeed = restoreSpeed;
        }
    }
    
    void Avoidance(string previousState)
    {
        ChangeState("Avoiding");
        // Begin the first part of avoiding
        if (avoidanceStage == 1)
        {
            // Rotate to the left for a designer set amount of time
            stageTimer = resetTimer;
            motor.RotateLeft();

            // Go to the next stage
            if (stageTimer <= 0)
            {
                avoidanceStage = 2;
            }
        }
        if (avoidanceStage == 2)
        {
            // Move forward for a designer set amount of time
            stageTimer = resetTimer;
            motor.moveSpeed = restoreSpeed;
            motor.MoveForward();
            if (stageTimer <= 0)
            {
                avoidanceStage = 3;
            }
        }
        if (avoidanceStage == 3)
        {
            // Rotate to the right for set amount of time
            stageTimer = resetTimer;
            motor.RotateRight();
            if (stageTimer <= 0)
            {
                avoidanceStage = 0;
                ChangeState(previousState);
            }
        }
    }

    void Flee()
    {
        // Return to normal speed
        motor.moveSpeed = restoreSpeed;

        // Increase speed
        motor.moveSpeed = motor.moveSpeed + speedChange;

        // Get the vector away from the player
        Vector3 vectorAway = -1 * direction;
        // Normalize the vector
        vectorAway.Normalize();
        // Set the vector to the tank's detection rangee
        vectorAway *= detectionRange;
        // Set the flee direction to where the flee point is based on the tank's current position
        fleeDirection = vectorAway + transform.position;
        // Rotate towards the flee point
        RotateTo(fleeDirection, motor.turnSpeed);
        // Move towards the flee point
        MoveTo(fleeDirection, motor.moveSpeed);
        // Check for walls
        WallCheck();
    }

    void Chase()
    {
        // Return to normal speed
        motor.moveSpeed = restoreSpeed;
        // Increase speed
        motor.moveSpeed = motor.moveSpeed + speedChange;
        // Rotate and move towards the player
        RotateTo(player.transform.position, motor.turnSpeed);
        MoveTo(player.transform.position, motor.moveSpeed);
    }

    void ChangeState(string newState)
    {
        // Change the current state to a new state
        state = newState;
    }
    
    void CanHear()
    {
        // Check if the tank is not currently fleeing
        if (state != "Flee") {
            // Check if the player is within the detection range
            if (Vector3.Distance(player.transform.position, transform.position) <= detectionRange)
            {
                // Stop moving and change state to "Investigating"
                motor.moveSpeed = 0;
                ChangeState("Investigating");
            }
            else
            {
                // Return to normal speed and continue patrolling
                motor.moveSpeed = restoreSpeed;
                ChangeState("Patrol");
            }
        }
        // Check if the tank is in the "Flee" state
        else if (state == "Flee")
        {
            // Ceheck if the player is within the detection range
            if (Vector3.Distance(player.transform.position, transform.position) > detectionRange)
            {
                ChangeState("Patrol");
            }

        }

    }

    void CanSee()
    {
        // Get the angle to the player from the tank
        angleToPlayer = Vector3.Angle(transform.forward, direction);

        // Check if the player is within the Field of Vision
        if (angleToPlayer <= fOV || angleToPlayer <= negativeFOV)
        {
            // Check if the Raycast is hitting the player
            if (hitInfo.transform == player.transform)
            {
                // If the tank is a flee-type, start running form the player
                if (gameObject.layer == 9)
                {
                    // Change state to "Flee"
                    ChangeState("Flee");
                }
                // If the tank is a chase-type, start follolwing the player
                else if (gameObject.layer == 10)
                {
                    // Change state to "Chase"
                    ChangeState("Chase");
                }
            }
            else
            {
                // If the player is unseen, check if the tank can still hear the player
                CanHear();
            }
        }
        
    }

    void Attack()
    {
        if (fire.timer <= 0)
        {
            // Use the Fire function on the Shoot script
            fire.Fire();
        }
    }
}
