using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GondolaDoorOpenTrigger : MonoBehaviour
{
    public Gondola gondola;
    public GameObject doorCollider;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            gondola.OpenDoor();
            doorCollider.SetActive(false);
            PlayerControl.inGondolaRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player")
        {
            gondola.CloseDoor();
            doorCollider.SetActive(true);
            PlayerControl.inGondolaRange = false;
        }
    }

}
