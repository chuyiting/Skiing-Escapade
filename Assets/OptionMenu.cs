using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : MonoBehaviour
{

    public SteamVR_Action_Boolean setUpAction;
    public float HEIGHT = 2.0f;
    [SerializeField] private TextMeshProUGUI _heightText;

    private Transform head = null; 

    // Start is called before the first frame update
    void Start()
    {
        head = SteamVR_Render.Top().head;
    }

    public void SetHeight() {
        if (setUpAction.stateDown)
        {
            HEIGHT = head.localPosition.y;
            _heightText.text = HEIGHT.ToString("0.00");
        }
    }


    // Update is called once per frame
    void Update()
    {
        SetHeight();
    }
}
