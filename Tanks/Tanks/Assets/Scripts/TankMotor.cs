using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
{
    // Create a variable to store the transform component
    private Transform tf;
    // Create a variable for the speed tanks will rotate
    public float turnSpeed;
    // Create a variable for the speed tanks will move
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        // Access the Transform component
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveForward()
    {
        // Move forward
        tf.Translate(Vector3.forward * moveSpeed* 1f * Time.deltaTime);
    }

    public void MoveBackwards()
    {
        // Move backwards
        tf.Translate(Vector3.back * moveSpeed *1f * Time.deltaTime);
    }

    public void RotateLeft()
    {
        // Rotate to the left
        tf.Rotate(0, -turnSpeed * 1f * Time.deltaTime, 0);
    }

    public void RotateRight()
    {
        // Rotate to the right
        tf.Rotate(0, turnSpeed * 1f * Time.deltaTime, 0);
    }
}
