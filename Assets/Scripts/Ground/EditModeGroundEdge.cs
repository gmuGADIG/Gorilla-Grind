using System.Collections;
using System.Collections.Generic;
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
            groundEdge.edgeCollider.adjacentStartPoint = transform.InverseTransformPoint( groundEdge.previous.endPoint ) - groundEdge.transform.position;
            
            Debug.DrawLine(groundEdge.edgeCollider.adjacentStartPoint, groundEdge.previous.endPoint, Color.blue);

        }

        if (groundEdge.next != null)
        {

            groundEdge.edgeCollider.adjacentEndPoint = groundEdge.next.startPoint;// - (Vector2)groundEdge.transform.position;
        }

    }
}
