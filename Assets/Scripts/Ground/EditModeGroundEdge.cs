using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(GroundEdge))]
public class EditModeGroundEdge : MonoBehaviour
{
    GroundEdge groundEdge;

    public bool snapToPrevious;
    
    // Start is called before the first frame update
    void Awake()
    {
        groundEdge = GetComponent<GroundEdge>();
    }
    
    // Update is called once per frame
    void Update()
    {
        EdgeCollider2D edgeCollider = groundEdge.edgeCollider;
        //edgeCollider.useAdjacentStartPoint = groundEdge.previous != null;
        //edgeCollider.useAdjacentEndPoint = groundEdge.next != null;
        if (snapToPrevious)
        {
            //Dont want cyclical links
            if (groundEdge.previous == groundEdge)
            {
                groundEdge.previous = null;
            }
            if (groundEdge.next == groundEdge)
            {
                groundEdge.next = null;
            }

            if (groundEdge.next != null)
            {
                groundEdge.next.previous = groundEdge;
                //edgeCollider.adjacentEndPoint = edgeCollider.transform.worldToLocalMatrix.MultiplyPoint(groundEdge.next.startPoint);
            }

            if (groundEdge.previous != null)
            {

                groundEdge.transform.position += (Vector3)(groundEdge.previous.endPoint - groundEdge.startPoint);

                //edgeCollider.adjacentStartPoint = edgeCollider.transform.worldToLocalMatrix.MultiplyPoint(groundEdge.previous.endPoint);

                //Plan on creating a visual tool to automatically connect and disconnect edges
                if (groundEdge.previous.next == null)
                {
                    groundEdge.previous = null;
                }
            }
        } else
        {

        }
        

        

        if(GroundEdge.shouldRenderEdge)
        {
            for(int i = 0; i < groundEdge.edgeCollider.pointCount-1; i++)
            {
                Debug.DrawLine(groundEdge.edgeCollider.transform.localToWorldMatrix.MultiplyPoint(groundEdge.edgeCollider.points[i]),
                    groundEdge.edgeCollider.transform.localToWorldMatrix.MultiplyPoint(groundEdge.edgeCollider.points[i + 1]),
                    groundEdge.noCollision ? GroundEdge.gapColor : GroundEdge.solidColor);
            }
        }
    }

    

    private void OnDrawGizmos()
    {
        
    }
}
