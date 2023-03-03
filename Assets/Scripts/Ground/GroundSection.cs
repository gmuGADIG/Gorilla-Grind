using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GroundSection : MonoBehaviour
{

    public GroundEdge[] groundEdges;

    /**
     * Reconnects every ground edge to the next one
     */
    public void VerifyConnections()
    {
        if (groundEdges.Length == 0) return;
        if (groundEdges.Length == 1)
        {
            groundEdges[0].previous = groundEdges[0].next = null;
        }
        groundEdges[0].previous = null;
        groundEdges[0].next = groundEdges[1];
        for(int i = 1; i < groundEdges.Length-1; i++)
        {
            groundEdges[i].previous = groundEdges[i - 1];
            groundEdges[i].next = groundEdges[i + 1];
        }
        groundEdges[groundEdges.Length - 1].previous = groundEdges[groundEdges.Length - 2];
        groundEdges[groundEdges.Length - 1].next = null;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
