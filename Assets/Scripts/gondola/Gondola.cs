using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Gondola : MonoBehaviour
{
    float distanceTravelled = 0.0f;
    public PathCreator pathCreator;
    public float speed = 5.0f;

    // structure
    public GameObject leftDoor;
    public GameObject rightDoor;

    // movement
    public static bool shouldStartMoving = false;
    private bool isDoorOpen = false;
    
    // position information
    public static bool hasArrivedDestination = false;
    private Vector3 prevPosition;
    private float movingSpeed = 0.0f;

    // player information
    private GameObject player;


    private void Awake() {
        prevPosition = this.transform.position;
        player = GameObject.Find("Player");
    }
    // Update is called once per frame
    void Update()
    {
        CalculateSpeed();
        UpdateHasArrivedDestination();
        if (hasArrivedDestination)
        {
             HandleArrival();
        }

        if (shouldStartMoving) 
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
        }
    }

    public void OpenDoor()
    { 
        if (isDoorOpen) return;
        isDoorOpen = true;
        leftDoor.transform.position = leftDoor.transform.position + leftDoor.transform.right * 1.2f;
        rightDoor.transform.position = rightDoor.transform.position - rightDoor.transform.right * 1.2f;

    }
    
    public void CloseDoor()
    {
        if (!isDoorOpen) return;
        isDoorOpen = false;
        leftDoor.transform.position = leftDoor.transform.position - leftDoor.transform.right * 1.2f;
        rightDoor.transform.position = rightDoor.transform.position + rightDoor.transform.right * 1.2f;
    }

    private void UpdateHasArrivedDestination()
    {
        if (distanceTravelled > 0.0f && movingSpeed == 0.0f)
        {
            hasArrivedDestination = true;
        } 
        else 
        {
            hasArrivedDestination = false;
        }
    }

    private void CalculateSpeed()
    {
        movingSpeed = (transform.position - prevPosition).magnitude  / Time.deltaTime;
        prevPosition = transform.position;
    }

    private void HandleArrival()
    {
        OpenDoor();
        player.transform.parent = null;

    }
}
