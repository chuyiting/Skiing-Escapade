using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputSourceManager : MonoBehaviour
{
    private static InputSourceManager instance;
    public static InputSourceManager Instance 
    {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<InputSourceManager>();
            }
            return instance;
        }
    }

    public enum Scene {
        HOMEPAGE, GAME
    }

    public Scene scene = Scene.GAME;
    public SteamVR_ActionSet HomepageActionSet; 
    public SteamVR_ActionSet GameActionSet;


    private void Awake() {
        switch (scene) {
        case Scene.HOMEPAGE:
            HomepageActionSet.Activate();
            break;
        case Scene.GAME:
            GameActionSet.Activate();
            break;

        }
    }

    public void TurnOnUIActionSet()
    {
        HomepageActionSet.Activate();
    }

    public void TurnOnGameActionSet()
    {
        GameActionSet.Activate();
    }
}

