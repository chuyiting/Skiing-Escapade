using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SetUp : MonoBehaviour
{
    public SteamVR_Action_Boolean setUpAction;
    public float HEIGHT = 2.0f;

    private Transform head = null; 

    // Start is called before the first frame update
    void Start()
    {
        head = SteamVR_Render.Top().head;
    }

    // Update is called once per frame
    void Update()
    {
        if (setUpAction.stateDown)
        {
            HEIGHT = head.localPosition.y;
        }
    }
}
