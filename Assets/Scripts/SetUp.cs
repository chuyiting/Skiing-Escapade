using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;

public class SetUp : MonoBehaviour
{
    public SteamVR_Action_Boolean setUpAction;
    public float HEIGHT = 2.0f;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI calibrateText;
    private bool startCalibrate = false;
    private bool heightSet = false;
    

    private Transform head = null; 

    // Start is called before the first frame update
    void Start()
    {
        head = SteamVR_Render.Top().head;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startCalibrate) return;
        heightSet = false;
        
        if (heightText.gameObject.activeSelf && !heightSet) 
        {
            heightText.text = head.localPosition.y.ToString("0.00");
        }

        if (gameObject.activeSelf && setUpAction.stateDown && !heightSet)
        {
            HEIGHT = head.localPosition.y;
            heightText.color = Color.yellow;
            heightSet = true;
        }
    }

    public void Calibrate() 
    {
        if (startCalibrate)
        {
            calibrateText.text = "Calibrate";
            startCalibrate = false;
        } else 
        {
            calibrateText.text = "Done";
            startCalibrate = true;
        }
    }

    public bool hasHeightSet()
    {
        return heightSet;
    }

}
