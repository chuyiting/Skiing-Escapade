using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private Transform head;
    private GameObject player;
    public SteamVR_Action_Boolean pauseButton; 

    // pause menu objects
    public GameObject pointer;
    public Canvas pauseMenu;

    // menu status
    private bool menuOn = false;


    private void Awake() {
        head = SteamVR_Render.Top().head;
        player = GameObject.Find("Player");
    }

    private void Start() {
        pauseMenu.enabled = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (pauseButton.stateDown && menuOn == false) {
            Time.timeScale = 0.0f;
            OpenPauseMenu();
            InputSourceManager.Instance.TurnOnUIActionSet();
            menuOn = true;
        }
    }

    private void OpenPauseMenu() 
    {
        pointer.SetActive(true);
        pauseMenu.enabled = true;


        Vector3 forward = head.forward - Vector3.Project(head.forward, Vector3.up);
        pauseMenu.gameObject.transform.position = head.position + forward * 15f + Vector3.up * 1f;
        pauseMenu.gameObject.transform.forward = forward;
    }

    public void Resume()
    {
        InputSourceManager.Instance.TurnOnGameActionSet();
        turnOffMenu();
        Time.timeScale = 1.0f;
    }

    public void Restart() 
    {
        InputSourceManager.Instance.TurnOnGameActionSet();
        Time.timeScale = 1.0f;
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = GlobalStorage.StoredPlayerPosition;
        player.transform.rotation = GlobalStorage.StoredPlayerRotation;
        player.GetComponent<CharacterController>().enabled = true;
        turnOffMenu();
    }

    public void Exit()
    {
        Application.Quit();
    }


    private void turnOffMenu()
    {
        pointer.SetActive(false);
        pauseMenu.enabled = false;
        menuOn = false;
    }

}
