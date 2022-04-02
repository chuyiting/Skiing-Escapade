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
    public int terrainLayer;
    public float brakeThresholdTime = 1.0f;
    private float timer = 1.0f;

    private void Awake() {
        lastPosition = transform.position;
        if (GameObject.Find("Player") == null) return;
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
        timer = brakeThresholdTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player") == null) return;
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    private void FixedUpdate() {
        speed = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;
        //Debug.Log("ski pole speed: " + speed);
    }

    private void OnTriggerEnter(Collider other) {
        //Debug.Log("speed: " + speed);
        if (playerControl == null) return;
        if (other.gameObject.layer == terrainLayer && getPushSpeed() < -skiSpeedThreshold) {
            playerControl.ExertSkiPoleForce();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (playerControl == null) return;
        if (other.gameObject.layer == terrainLayer) {
            if (timer <= 0 && playerControl != null)
            {
                playerControl.Brake();
            }
            else 
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == terrainLayer)
        {
            timer = brakeThresholdTime;
        }
    }
    private float getPushSpeed()
    {
        return speed.y;
    }
}
