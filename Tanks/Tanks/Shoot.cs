using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Create a variable for a shell template
    public GameObject shell;
    // Create a variable for the shells' speed
    public float shellSpeed;
    // Create a variable to access the shells' Rigidbody
    public Rigidbody rb;
    // Create variables for a timer and wait time
    public float waitTime;
    public float timer;
    // Create a variable to determine how last shells will last
    public float lifeSpan;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Fire()
    {
        // Reset the timer
        timer = waitTime;
        // Instantiate a shell at the fire point
        GameObject shellClone = Instantiate(shell, transform.position, transform.rotation) as GameObject;
        if (shellClone != null)
        {
            // Access the Shell clone's Rigidbody
            rb = shellClone.GetComponent<Rigidbody>();
            // Move the shell clone forwards
            rb.velocity = transform.forward * shellSpeed;
        }
        // Destroy the shell clone after time has passed
        Destroy(shellClone, lifeSpan);
    }
}
