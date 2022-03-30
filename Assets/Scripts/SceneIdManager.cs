using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneIdManager : MonoBehaviour
{

    private static int SCENE_HOMEPAGE = 0;
    private static int SCENE_GAME = 1;
    public static int GetHomePageScene()
    {
        return SCENE_HOMEPAGE;
    }

    public static int GetGameScene()
    {
        return SCENE_GAME;
    }
}
