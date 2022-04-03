using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStorage : MonoBehaviour
{
    private static Vector3 StartPointOne = new Vector3(1456.29f, 478.4f, 311.02f);
    private static Vector3 StartPointTwo = new Vector3(3952f, 92f, 3225f);
    public static Vector3 StoredPlayerPosition = StartPointOne;
    public static Quaternion StoredPlayerRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
