using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float buffer = 0.1f; // 10 centimeters
    public static bool isGrounded = false;

    private CharacterController characterController;
    private PlayerControl playerControl;
    public LayerMask terrainLayerMask;

    public AudioSource landAudio;

    //-------------------------------------------------
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerControl = GetComponent<PlayerControl>();
    }


    //-------------------------------------------------
    void FixedUpdate()
    {
        float angle = Mathf.Deg2Rad * Vector3.Angle(playerControl.normal, Vector3.up);
        float castDistance = characterController.height / 2 - characterController.radius + (1 / Mathf.Cos(angle) * characterController.radius) + buffer;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, castDistance, terrainLayerMask))
        {
            if (!isGrounded)
            {
                landAudio.Play();
            } 
            isGrounded = true;
        }
        else 
        {
            isGrounded = false;
        }
    }
}
