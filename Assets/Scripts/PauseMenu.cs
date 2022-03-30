using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private Canvas pauseMenu;

    private void Awake() {
        pauseMenu = GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        pauseMenu.enabled = false;
    }

    public void Show()
    {
        pauseMenu.enabled = true;
    }
}
