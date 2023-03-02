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
    void Awake()
    {
        groundEdge = GetComponent<GroundEdge>();
    }
    
    // Update is called once per frame
    void Update()
    {
        EdgeCollider2D edgeCollider = groundEdge.edgeCollider;
        edgeCollider.useAdjacentStartPoint = groundEdge.previous != null;
        edgeCollider.useAdjacentEndPoint = groundEdge.next != null;

        if (groundEdge.previous != null)
        {
            edgeCollider.adjacentStartPoint = edgeCollider.transform.worldToLocalMatrix.MultiplyPoint(groundEdge.previous.endPoint);

            groundEdge.transform.position += (Vector3)(groundEdge.previous.endPoint - groundEdge.startPoint);
        }

        if (groundEdge.next != null)
        {

            edgeCollider.adjacentEndPoint = edgeCollider.transform.worldToLocalMatrix.MultiplyPoint(groundEdge.next.startPoint);
        }

        if(GroundEdge.shouldRenderEdge)
        {
            for(int i = 0; i < groundEdge.edgeCollider.pointCount-1; i++)
            {
                Debug.DrawLine(groundEdge.edgeCollider.transform.localToWorldMatrix.MultiplyPoint(groundEdge.edgeCollider.points[i]),
                    groundEdge.edgeCollider.transform.localToWorldMatrix.MultiplyPoint(groundEdge.edgeCollider.points[i + 1]));
            }
        }
    }

    

    private void OnDrawGizmos()
    {
        
    }
}
