using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(GroundEdge))]
public class EditModeGroundEdge : MonoBehaviour
{
    GroundEdge groundEdge;
    // Start is called before the first frame update
    void Start()
    {
        groundEdge = GetComponent<GroundEdge>();
    }

    // Update is called once per frame
    void Update()
    {
        
        groundEdge.edgeCollider.useAdjacentStartPoint = groundEdge.previous != null;
        groundEdge.edgeCollider.useAdjacentEndPoint = groundEdge.next != null;

        if (groundEdge.previous != null)
        {
            groundEdge.edgeCollider.adjacentStartPoint = groundEdge.edgeCollider.transform.worldToLocalMatrix.MultiplyPoint(groundEdge.previous.endPoint);
            
            
        }

        if (groundEdge.next != null)
        {

            groundEdge.edgeCollider.adjacentEndPoint = groundEdge.edgeCollider.transform.worldToLocalMatrix.MultiplyPoint(groundEdge.next.startPoint);
        }

    }

    private void OnDrawGizmos()
    {
        if (groundEdge.showBounds)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(groundEdge.startPoint, Vector3.one/10);
            Gizmos.DrawCube(groundEdge.endPoint, Vector3.one/10);

        }
    }
}
