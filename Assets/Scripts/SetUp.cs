using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using TMPro;

public class SetUp : MonoBehaviour
{
    public static float HEIGHT = 1.75f;
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

        if (heightText.gameObject.activeSelf && !heightSet) 
        {
            heightText.text = head.localPosition.y.ToString("0.00");
            heightText.color = Color.white;
        }

        Debug.Log("height set: " + heightSet);
        if (heightText.gameObject.activeSelf && heightSet)
        {
            HEIGHT = head.localPosition.y;
            heightText.color = Color.yellow;
        }
    }

    public void Calibrate() 
    {
        // finish calibrate
        if (startCalibrate)
        {
            calibrateText.text = "Calibrate";
            startCalibrate = false;
            heightSet = true;
        } 
        // start to calibrate
        else 
        {
            calibrateText.text = "Done";
            startCalibrate = true;
            heightSet = false;
        }
    }

    public bool hasHeightSet()
    {
        return heightSet;
    }

}
