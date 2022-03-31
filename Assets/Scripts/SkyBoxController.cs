using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxController : MonoBehaviour
{
    public Material mat1;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = mat1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
