using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Create a variable to keep track of the score
    public int score;
    
    // Create an array to store all of the active enemies
    public GameObject[] activeEnemies;

    // Ensure that only one GameManager is active at a time
    public static GameManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Find all enemies and add them to the array
        activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
