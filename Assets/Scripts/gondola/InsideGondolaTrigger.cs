using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideGondolaTrigger : MonoBehaviour
{

    public float durationToMove = 3.0f;
    private float timer;
    private GameObject player;
    private PlayerControl playerControl;
    public GameObject GondolaDoorCollider;
    public Gondola gondola;
    private Vector3 prevPosition;

    void Awake()
    {
        timer = durationToMove;
        player = GameObject.Find("Player");
        playerControl = player.GetComponent<PlayerControl>();
        GondolaDoorCollider.SetActive(false);
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other) {
        if (other.tag != "Player") return;
        playerControl.ToggleGondolaMode(true);
        timer -= Time.deltaTime;
        if (timer <= 0.0 && !Gondola.shouldStartMoving) 
        {
            player.transform.parent = this.transform;
            Gondola.shouldStartMoving = true;
            GondolaDoorCollider.SetActive(true);
            gondola.CloseDoor();

        }

    }

    private void OnTriggerExit(Collider other) {
        if (other.tag != "Player") return;
        playerControl.ToggleGondolaMode(false);
        timer = durationToMove;
        player.transform.parent = null;
    }

}
