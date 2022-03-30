using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SkiPole : MonoBehaviour
{
    public SteamVR_Input_Sources controller;
    private Vector3 lastPosition;
    public float skiSpeedThreshold = 1.0f;
    private Vector3 speed = Vector3.zero;
    private PlayerControl playerControl;

    private void Awake() {
        lastPosition = transform.position;
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        speed = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log("trigger: " + other.tag);
        Debug.Log("speed: " + speed);
        if (getPushSpeed() < -skiSpeedThreshold) {
            playerControl.ExertSkiPoleForce();
        }
    }

    private float getPushSpeed()
    {
        return speed.y;
    }
}
