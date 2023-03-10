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
            groundEdge.SnapEdge();
        } else
        {

        }
        

        

        
    }



    //void OnDrawGizmos()
    //{

    //    if (GroundEdge.shouldRenderEdge)
    //    {
    //        Gizmos.color = groundEdge.noCollision ? GroundEdge.gapColor : GroundEdge.solidColor;
    //        for (int i = 0; i < groundEdge.edgeCollider.pointCount - 1; i++)
    //        {
    //            Gizmos.DrawLine(groundEdge.edgeCollider.transform.localToWorldMatrix.MultiplyPoint(groundEdge.edgeCollider.points[i]),
    //                groundEdge.edgeCollider.transform.localToWorldMatrix.MultiplyPoint(groundEdge.edgeCollider.points[i + 1]));
    //        }
    //    }
    //}
}
