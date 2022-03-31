using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PauseMenu : MonoBehaviour
{
    private Canvas pauseMenu;
    private GameObject player;
    public SteamVR_Action_Boolean menuButton; 

    private void Awake() {
        pauseMenu = GetComponent<Canvas>();
        player = GameObject.Find("Player");
    }



    // Update is called once per frame
    void Update()
    {
        if (menuButton.stateDown) {
            Show();
        }
    }

    public void Hide()
    {
        pauseMenu.enabled = false;
    }

    public void Show()
    {
        pauseMenu.enabled = true;
        transform.position = player.transform.position + player.transform.forward * 10 + Vector3.up * 7.0f;
    }
}
