using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float DegreePerSecond = 50.0f;
    // Update is called once per frame
    void Update()
    {
        if (Gondola.shouldStartMoving)
        {
             transform.RotateAround(transform.position, Vector3.up, DegreePerSecond * Time.deltaTime);
        }
    }
}
