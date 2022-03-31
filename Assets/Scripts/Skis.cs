using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skis : MonoBehaviour
{
    public Transform followHead;
    private PlayerControl playerControl;
    public LayerMask terrainLayer;
    
    private void Awake() {
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
    }
    public void ResetSkiPosition()
    {
        transform.position = new Vector3(followHead.position.x, transform.position.y, followHead.position.z);
    }

    private void Update() {
        if (playerControl.start)
        {
            var ray = new Ray(transform.position, Vector3.down);

            Vector3 forward = followHead.forward - Vector3.Project(followHead.forward, playerControl.normal);
            transform.forward = forward;
        }
    }
}
