using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Ski : MonoBehaviour
{
    public SteamVR_Action_Boolean skiRotationMovement;
    public SteamVR_Action_Boolean skiMovement;

    // ski rotation
    public Transform controller;

    // ski
    private bool hasSkiStarted;
    public PlayerControl player;
    public SteamVR_Action_Pose pose;
    public SteamVR_Input_Sources hand;
    public SteamVR_Input_Sources head;

    private Transform headTransform;
    public bool test = true;
    // Start is called before the first frame update
    void Start()
    {
        // for ski
        hasSkiStarted = false;
        pose.AddOnUpdateListener(hand, DoSki);
        headTransform = SteamVR_Render.Top().head;

    }

    // Update is called once per frame
    void Update()
    {
    }


    void RotateSki()
    {

        Vector3 controllerDirection = controller.forward - Vector3.Project(controller.forward, transform.up);
        Quaternion rotation = Quaternion.FromToRotation(transform.forward, controllerDirection);
        transform.rotation = rotation * transform.rotation;
    }

    void DoSki(SteamVR_Action_Pose fromAction, SteamVR_Input_Sources fromSource)
    {
        //makeSkiFollowController(fromAction);
        if (skiMovement.state)
        {
            Debug.Log("controller velocity: " + fromAction.velocity);
            ski(new Vector3(-fromAction.velocity.x, 0.0f, fromAction.velocity.z));
        } 
        else 
        {
            stopSki();
        }

    }

    void makeSkiFollowController(SteamVR_Action_Pose movement)
    {
        Vector3 displacement = movement.localPosition - movement.lastLocalPosition;
        displacement -= Vector3.Project(displacement, transform.up); // cancel out local y directional movement

        transform.position += displacement;
    }

    void ski(Vector3 velocity)
    {  
        //Debug.Log("velocity: " + velocity);
        player.SetSkiForce(velocity);
    }

    void stopSki()
    {
        player.SetSkiForce(Vector3.zero);
    }


}
