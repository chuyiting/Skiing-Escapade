 using UnityEngine;
 using UnityEditor;
 using System.Collections;
 using System.Collections.Generic;
 
 public class SkiBoxColliderResizer : MonoBehaviour {
    public float colliderPadding = 0.5f;
    public GameObject leftSkiMesh;
    public GameObject rightSkiMesh;
    private BoxCollider boxCollider;
    private Vector3 skiCenter;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider>();
        skiCenter = (leftSkiMesh.transform.position + rightSkiMesh.transform.position) / 2;
    }
    private void Update() {
        skiCenter = (leftSkiMesh.transform.position + rightSkiMesh.transform.position) / 2;
        SetCollidersFrom();
    }

    public void SetCollidersFrom()
    {
        Bounds              totalBounds     = new Bounds(skiCenter, Vector3.zero);

        Bounds leftSkiBound = leftSkiMesh.GetComponent<BoxCollider>().bounds;
        Bounds rightSkiBound = rightSkiMesh.GetComponent<BoxCollider>().bounds;
        DrawBounds(leftSkiBound);
        DrawBounds(rightSkiBound);

        totalBounds.Encapsulate(leftSkiBound);
        totalBounds.Encapsulate(rightSkiBound);
        

        boxCollider.center = new Vector3(0f, 0.05f, 0.05f);
        boxCollider.size = new Vector3(totalBounds.size.x, totalBounds.size.y, totalBounds.size.z);
     }

     
    void DrawBounds(Bounds b, float delay=0)
    {
        // bottom
        var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
        var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
        var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
        var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

        Debug.DrawLine(p1, p2, Color.blue, delay);
        Debug.DrawLine(p2, p3, Color.red, delay);
        Debug.DrawLine(p3, p4, Color.yellow, delay);
        Debug.DrawLine(p4, p1, Color.magenta, delay);

        // top
        var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
        var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
        var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
        var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

        Debug.DrawLine(p5, p6, Color.blue, delay);
        Debug.DrawLine(p6, p7, Color.red, delay);
        Debug.DrawLine(p7, p8, Color.yellow, delay);
        Debug.DrawLine(p8, p5, Color.magenta, delay);

        // sides
        Debug.DrawLine(p1, p5, Color.white, delay);
        Debug.DrawLine(p2, p6, Color.gray, delay);
        Debug.DrawLine(p3, p7, Color.green, delay);
        Debug.DrawLine(p4, p8, Color.cyan, delay);
    }
}