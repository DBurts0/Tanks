using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Create a variable to determine how many rows the map will have
    public int rows;

    // Create a variable to determine how many columns the map will have
    public int columns;

    // Create varibles to determine the length and width of the rooms
    public float roomWidth;

    public float roomHeight;

    // Create a list of room prefabs to choose from
    public GameObject[] gridPrefabs;

    // Create a list using two numbers to create a grid
    private Room[,] grid;

    // Create a variable to access the doors of each room
    private Room doorOpener;

    // Create a randomizer seed
    public int mapSeed;

    // Create bool to choose if the player wants a preset seed
    public bool presetSeed;

    // Create bool to choose if the player wants a randomized map
    public bool randomizedMap;

    // Create bool to choose if the player wants a map of the day
    public bool MapOfTheDay;

    // Create a variable to store a random point;
    public Vector3 randomPoint;

    // Create a variable to store the camera
    public Camera cam;

    // Create a variable to store the player tank prefab
    public GameObject playerTank;

    // Create a variable to store the enemy tank prefab
    public GameObject enemyTank;

    // Variables to store the pickup prefabs
    public GameObject HealthPickup;

    public GameObject SpeedPickup;

    public GameObject DamagePickup;

    // Variables for counting down and reseting a timer
    public float pickupTimer;

    public float timerReset;

    // Variable for limiting how many pickups are allowed at a time
    public int pickupLimit;

    // List for all active pickups
    public List<GameObject> activePickupList;

    // List for possible pickups
    public List<GameObject> pickupList;

    // Arrays for the patrol paths

    public Transform[] choosenPath1;

    public Transform[] choosenPath2;

    public Transform[] choosenPath3;

    public Transform[] choosenPath4;

    // Variables to get the width and height of the map
    public float mapWidth;
    public float mapHeight;

    // Variable for randomly choosing a layer
    public int LayerChooser;
    // List for possible layer
    public List<int> Layerlist;

    // Start is called before the first frame update
    void Start()
    {
        // Create a randomized grid using the rooms chosen
        GenerateGrid();
        mapHeight = (columns - 1) * roomHeight;
        mapWidth = (rows - 1) * roomWidth;
        // Access the Room script
        doorOpener = GetComponent<Room>();

        // Add the Flee and Chase tags to the list
        Layerlist.Add(9);
        Layerlist.Add(10);

        SpawnPlayer();
        SpawnEnemies();
    }

    void SpawnPlayer()
    {
        randomPoint = new Vector3(UnityEngine.Random.Range(0, mapWidth), 1, UnityEngine.Random.Range(0, mapHeight));
        // Create a player tank at a random point within the map
        GameObject player = Instantiate(playerTank, randomPoint, Quaternion.identity) as GameObject;

        player.GetComponent<TankData>().GMHolder = gameObject;
    }

    // Randomly choose the flee and chase layer for the tanks
    public void LayerRandomizer()
    {
        LayerChooser = UnityEngine.Random.Range(0, Layerlist.Count);
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < 4; i++)
        {
            // Randomly choose a layer
            LayerRandomizer();
            randomPoint = new Vector3(UnityEngine.Random.Range(0, mapWidth), 1, UnityEngine.Random.Range(0, mapHeight));
            GameObject player = GameObject.FindWithTag("Player");
            // Spawn an Enemy Tank
            GameObject EnemyTank = Instantiate(enemyTank, randomPoint, Quaternion.identity) as GameObject;
            EnemyTank.GetComponent<TankData>().GMHolder = gameObject;
            EnemyTank.GetComponent<AIController>().player = player;
            EnemyTank.layer = Layerlist[LayerChooser];
            // Select a patrol route for the tanks
            if (i == 0)
            {
                EnemyTank.GetComponent<AIController>().waypoints = choosenPath1;
            }
            else if (i == 1)
            {
                EnemyTank.GetComponent<AIController>().waypoints = choosenPath2;
            }
            else if (i == 2)
            {
                EnemyTank.GetComponent<AIController>().waypoints = choosenPath3;
            }
            else
            {
                EnemyTank.GetComponent<AIController>().waypoints = choosenPath4;
            }
        }
    }

    void SpawnPickups()
    {
        // Randomly Select a pickup
        int randomPickup = UnityEngine.Random.Range(0, pickupList.Count);
        // Randomly select a point on the map
        randomPoint = new Vector3(UnityEngine.Random.Range(0, mapWidth), 1, UnityEngine.Random.Range(0, mapHeight));
        
            // Reset the pickupTimer
            pickupTimer = timerReset;
            // Instantiate the randomly selected pickup
            GameObject choosenPickup = Instantiate(pickupList[randomPickup], randomPoint, Quaternion.identity) as GameObject;
            // Add the new pickup to the list
            activePickupList.Add(choosenPickup);
    }

    void RemovePickup(GameObject pickup)
    {
        // Destroy the chosen pickup
        Destroy(pickup);
        // Remove the choosen pickup
        activePickupList.Remove(pickup);
    }

    // Function for generating a random room
    public GameObject RandomRoomPrefab()
    {
        // Randomly generate a room prefab by selecting it's index in the list

        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }

    public int DateToInt(DateTime dateToUse)
    {
        // Add the date and time
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }

    public void GenerateGrid()
    {
        if (presetSeed == true && MapOfTheDay == false && randomizedMap == false)
        {
            // Set the preset seed
            UnityEngine.Random.InitState(mapSeed);
            Debug.Log("Using preset seed");
        }
        if (MapOfTheDay == true && presetSeed == false && randomizedMap == false)
        {
            // Set the seed to the map of the day
            mapSeed = DateToInt(DateTime.Now.Date);
            UnityEngine.Random.InitState(mapSeed);
            Debug.Log("Using map of the day");
        }

        // Create a grid using the designer set number of columns and rows
        grid = new Room[columns,rows];

        // Go through each column per row in the grid
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Find a position based on the size of the room
                float xPosition = roomWidth * j;
                float zPosition = roomHeight * i;
                Vector3 newPosition = new Vector3(xPosition, 0, zPosition);

                // Instatiate a randomly chosen room at the location
                GameObject newRoom = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                // Store the room in the grid
                Room newRoomLocation = newRoom.GetComponent<Room>();
                grid[j, i] = newRoomLocation;

                // Set this object as its parent
                newRoom.transform.parent = this.transform;

                // Rename the room
                newRoom.name = "Room_" +i+","+j;

                // Open the appropriate doors:

                // If the room is on the bottom row, open the north doors
                if (i == 0)
                {
                    newRoomLocation.doorNorth.SetActive(false);
                }
                // Else if the room is on the top row, open the south doors
                else if (i == rows - 1)
                {
                    newRoomLocation.doorSouth.SetActive(false);
                }
                // Else open both north and south doors
                else
                {
                    newRoomLocation.doorNorth.SetActive(false);
                    newRoomLocation.doorSouth.SetActive(false);
                }

                // If the room is in the first (far left) column, open the east doors
                if (j == 0)
                {
                    newRoomLocation.doorEast.SetActive(false);
                }
                // Else if the room is in the last column (far right), open the west doors
                else if (j == columns - 1)
                {
                    newRoomLocation.doorWest.SetActive(false);
                }
                // Else open both the east and west doors
                else
                {
                    newRoomLocation.doorEast.SetActive(false);
                    newRoomLocation.doorWest.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        pickupTimer -= Time.deltaTime;
        // Check how much time has passed since the last time a pickup was spawned
        if (pickupTimer <= 0)
        {
            // Check if the current list of available pickups is less than the limit
            if (activePickupList.Count <= pickupLimit)
            {
                // Spawn a new pickup
                SpawnPickups();
            }
            // Go through each pick up in the active pickup list and check if they're null
            for (int i = 0; i < activePickupList.Count; i++)
            {
                if (activePickupList[i] == null)
                {
                    // Remove the object from the list
                    RemovePickup(activePickupList[i]);
                }
            }
            // Check if there are more pickups than the limit
            if (activePickupList.Count > pickupLimit)
            {
                // Remove the oldest pickup
                RemovePickup(activePickupList[0]);
            }
        }
    }
}
