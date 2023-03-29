using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GroundSection : MonoBehaviour
{
    [HideInInspector]
    public GroundEdge[] groundEdges;

    public Vector2 startPoint { get { return groundEdges[0].startPoint; } }

    public Vector2 endPoint { get { Debug.Log(groundEdges[groundEdges.Length - 1]); return groundEdges[groundEdges.Length - 1].endPoint; } }


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
        Vector2 center = (startPoint+endPoint)/2;
        groundEdges[0].previous = null;
        groundEdges[0].next = groundEdges[1];
        for(int i = 1; i < groundEdges.Length-1; i++)
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

#if UNITY_EDITOR
    private void OnEnable()
    {
        EditorApplication.hierarchyChanged += VerifyConnections;
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyChanged -= VerifyConnections;
    }
#endif


    void Update()
    {
        
    }


}
