using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public SetUp setup;
    private Button btn;

    private void Awake() 
    {
        btn = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (setup.hasHeightSet() && !btn.interactable)
        {
            btn.interactable = true;
        }
        else if (!setup.hasHeightSet() && btn.interactable)
        {
            btn.interactable = false;
        }
    }

    public void StartTheGame()
    {
        SceneManager.LoadScene(SceneIdManager.GetGameScene());
    }
}
