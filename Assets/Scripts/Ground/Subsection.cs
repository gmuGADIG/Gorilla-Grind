using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;


/**
 * Subsections represent connects ground edges horizontally. Subsections can be stacked vertically
 */
[ExecuteInEditMode]
public class Subsection : MonoBehaviour
{
    [HideInInspector]
    public GroundEdge[] groundEdges;

    public Vector2 startPoint { get { return groundEdges[0].startPoint; } }

    public Vector2 endPoint { get {  return groundEdges[groundEdges.Length - 1].endPoint; } }

    public float heightDiff => groundEdges.Sum(g => g.heightDiff);

    /**
     * Reconnects every ground edge to the next one
     */
    public void VerifyConnections()
    {

        groundEdges = GetComponentsInChildren<GroundEdge>();

        if (groundEdges.Length == 0) return;
        if (groundEdges.Length == 1)
        {
            groundEdges[0].previous = groundEdges[0].next = null;
            return;
        }
        Vector2 center = (startPoint + endPoint) / 2;
        groundEdges[0].previous = null;
        groundEdges[0].next = groundEdges[1];
        for (int i = 1; i < groundEdges.Length - 1; i++)
        {
            groundEdges[i].previous = groundEdges[i - 1];
            groundEdges[i].next = groundEdges[i + 1];

        }
        groundEdges[groundEdges.Length - 1].previous = groundEdges[groundEdges.Length - 2];
        groundEdges[groundEdges.Length - 1].next = null;

        Vector2 newCenter = (startPoint + endPoint) / 2;
        Debug.DrawRay(groundEdges[0].startPoint, (newCenter - center), Color.red, 4);
        groundEdges[0].transform.position += (Vector3)(newCenter - center);
        groundEdges[0].SnapSurroundingEdges();

    }


    // Start is called before the first frame update
    void Start()
    {

    }


    private void OnEnable()
    {
        #if UNITY_EDITOR
        EditorApplication.hierarchyChanged += VerifyConnections;
        #endif
    }

    private void OnDisable()
    {
        #if UNITY_EDITOR
        EditorApplication.hierarchyChanged -= VerifyConnections;
        #endif
    }
}
