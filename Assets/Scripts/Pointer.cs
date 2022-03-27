using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{

    public GameObject hand;

    public float defaultLength = 0.5f;
     public VRInputModule inputModule;

    private LineRenderer lineRenderer;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        transform.position = hand.transform.position + hand.transform.forward.normalized * 0.07f;
    }

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {


        // use default or distance
        PointerEventData data = inputModule.GetData();
        float targetLength = data.pointerCurrentRaycast.distance == 0? defaultLength : data.pointerCurrentRaycast.distance;

        // Raycast
        RaycastHit hit = createRaycast(targetLength);

        // Default 
        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        // or based on hit
        if (hit.collider != null)
        {
            endPosition = hit.point;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
        lineRenderer.enabled = true;

    }

    private RaycastHit createRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);


        return hit;
    }
}