using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    // create a variable for maximum health
    public int maxHealth;
    // create a variable for current health
    public int currentHealth;
    // create variables to access the GameManager
    public GameObject GMHolder;
    public GameManager GMCaller;
    // create a variable to store how many points each enemy is worth
    public int points;

    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        // set the current health to the maximum health
        currentHealth = maxHealth;
        // access the GameManager
        GMCaller = GMHolder.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    void OnCollisionEnter(Collision col)
    {
        // Check if the player's shell is hitting the enemy
        if (col.gameObject.tag == "Shell" && gameObject.tag == "Enemy")
        {
            // Take damage and destroy the shell
            TakeDamage(damage);
        }
        // check if the enemy's shell is hitting the player
        else if (col.gameObject.tag == "EnemyShell" && gameObject.tag == "Player")
        {
            // Take damage and destroy the shell
            TakeDamage(damage);
        }
    }
    void TakeDamage(int healthLoss)
    {
        // Check if the health is above 1
        if (currentHealth > 1)
        {
            // reduce health based on the shell's damage value
            currentHealth -= healthLoss;
        }
        // Check if the tank is an enemy and has at most 1 health
        else if (gameObject.tag == "Enemy"  && currentHealth <= 1)
        {
            // Add points to the score on the GameManager
            GMCaller.score = GMCaller.score + points;
            // Destroy the tank
            Destroy(gameObject);
        }
        else
        {
            // Destroy the tank
            Destroy(gameObject);
        }
    }
}
